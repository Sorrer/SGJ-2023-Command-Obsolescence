using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.CompilerServices;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public enum TowerType
{
	TowT_Unknown = 0,
	TowT_GenericTurret = 1,
	TowT_BombermanBlast = 2,
	TowT_BigSucc = 3,
	TowT_Pickaxe = 4,
	TowT_MissleLauncher = 5
};

[Serializable]
public enum TowerState
{
	TS_Idle = 0,
	TS_PerformingAction = 1,
	TS_Broken = 2
}

[Serializable]
public enum TowerTargetPriority
{
	TTP_First = 0,
	TTP_Last = 1,
	TTP_Strong = 2,
	TTP_Close = 3
}

[Serializable]
public enum TowerDirection
{
	TD_AllCardinal = 0,
	TD_Up = 1,
	TD_Right = 2,
	TD_Down = 3,
	TD_Left = 4
}

public class Tower : MonoBehaviour/*, IPointerDownHandler*/
{
	[Header("Internal Values")]
	[Tooltip("The string name of the tower.")]
	[SerializeField] protected string _towerName;
	[Tooltip("The type of the tower.")]
	[SerializeField] protected TowerType _towerType;
	[Tooltip("The state the tower is currently in.")]
	[SerializeField] protected TowerState _towerState;
	[Tooltip("The sprite renderer for this tower.")]
	[SerializeField] protected SpriteRenderer _sr;
	[Tooltip("The color to tint this tower when it is idle.")]
	[SerializeField] protected Color _idleColor = Color.white;
	[Tooltip("The color to tint this tower when it is broken.")]
	[SerializeField] protected Color _brokenColor = Color.gray;

	// Tower logic settings
	protected List<GameObject> _currEnemiesInRange = new List<GameObject>();

	[Header("Tower Settings")]
	[SerializeField] protected TowerTargetPriority _towerTargetPriority;
	[Tooltip("The direction this tower is facing.")]
	[SerializeField] protected TowerDirection _currentDirection;
	[Tooltip("Level of tower, for upgrades. Base is level 0.")]
	[SerializeField] protected int _towerLevel;
	[Tooltip("Maximum level the tower can be upgraded to.")]
	[SerializeField] protected int _maxTowerLevel = 3;
	[Tooltip("The projectile spawned by the tower.")]
	[SerializeField] protected GameObject _projectileObj; // Maybe projectiles should be defined on a per-tower basis? They might not all shoot things
	[Tooltip("Whether or not the tower should delete itself after the tower performs an action.")]
	[SerializeField] protected bool _destroyOnBreak = false;
	[Tooltip("Whether or not this tower can be destroyed.")]
	[SerializeField] protected bool _canBeDestroyed = true;
	[Tooltip("The purchasable for this tower.")]
	[SerializeField] protected Purchasable _purchaseInfo;

	/// <summary>
	/// The destroy cost for all towers. 
	/// 
	/// Wasn't sure where to put this; feel free to move this to a more suitable script if need be.
	/// </summary>
	public const int STANDARD_DESTROY_COST = 200;

	/// <summary>
	/// Whether or not this tower can be destroyed.
	/// </summary>
	public bool CanBeDestroyed => _canBeDestroyed;
	/// <summary>
	/// The Purchasable tied to this tower, containing its pricing information.
	/// </summary>
	public Purchasable PurchaseInfo => _purchaseInfo;
	/// <summary>
	/// The state this tower is currently in.
	/// </summary>
	public TowerState State => _towerState;
	/// <summary>
	/// Level of tower, for upgrades. Base is level 0.
	/// </summary>
	public int Level => _towerLevel;
	/// <summary>
	/// Maximum level the tower can be upgraded to.
	/// </summary>
	public int MaxLevel => _maxTowerLevel;
	
	// Start is called before the first frame update
	protected virtual void Start()
	{
		_sr = GetComponent<SpriteRenderer>();
		if (_sr)
			_sr.color = _idleColor;
		_towerState = TowerState.TS_Idle;
		_towerTargetPriority = TowerTargetPriority.TTP_First;
		_currentDirection = TowerDirection.TD_Right;
		_towerLevel = 0;
		AddPhysics2DRaycaster();
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		if (_towerState == TowerState.TS_Idle)
		{
			GameObject searchTarget = GetTarget();
			if (searchTarget != null)
				ExecuteTowerAction();
		}
	}

	/**
	 * @brief Function that gets fired when the mouse clicks on this object.
	 * @param eventData Idk man!
	 */
	/*public virtual void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Clicked on name:" + gameObject.name);
		// maybe remove these on click functions... they might interfere with the on click functions fired by the tiles
	}*/

	/**
	 * @brief Overrideable function proto for executing the action of a tower. Such as firing a bullet, etc.
	 */
	public virtual void ExecuteTowerAction()
	{
		_towerState = TowerState.TS_PerformingAction;
	}

	/**
	 * @brief Resets a tower back to the "TS_Idle" state after a delay.
	 * @param waitTime Time to wait before going to the "TS_Idle" state, in seconds.
	 */
	protected void ResetTower()
	{
		_towerState = TowerState.TS_Idle;
		if (_sr)
			_sr.color = _idleColor;
		ExecuteTowerAction();
	} 

	/**
	 * @brief Upgrades the tower by one level. Cannot go past max tower level.
	 */
	public virtual void TryUpgradeTower()
	{
		if (_towerLevel >= _maxTowerLevel) return;

		int upgradeCost = _purchaseInfo.UpgradePrices[_towerLevel];
		int balance = Bank.Instance.CurrentBalance;

		if (_towerState != TowerState.TS_PerformingAction && balance >= upgradeCost)
		{
			Bank.Instance.RemoveFromBalance(upgradeCost);

			_towerLevel++;

			Debug.Log("Upgraded to level " + _towerLevel);

			if (_towerState == TowerState.TS_Broken)
				ResetTower();
		}
	}

	/**
	 * @brief Overrideable function proto for an action that can occur when the tower is broken.
	 */
	public virtual void BreakTower()
	{
		_towerState = TowerState.TS_Broken;
		if (_sr)
			_sr.color = _brokenColor;
		if (_destroyOnBreak)
			Destroy(gameObject);
	}

	/**
	 * @brief Needed for IPointerDownHandler related functions. Find the Physics2DRaycaster component that
	 * exists in the world, or add one to the main camera GameObject if one does not exist.
	 */
	private void AddPhysics2DRaycaster()
	{
		Physics2DRaycaster physicsRaycaster = FindFirstObjectByType<Physics2DRaycaster>();
		if (physicsRaycaster == null)
		{
			Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
		}
	}

	// Thinking of moving this to a specific Tower class that has targeting (like the missiles)
	protected GameObject GetTarget()
	{
		GameObject ret = null;
		
		if (_currEnemiesInRange.Count == 0)
			return null;
		else if (_currEnemiesInRange.Count == 1)
			return _currEnemiesInRange[0];
		
		switch (_towerTargetPriority)
		{
			case TowerTargetPriority.TTP_Last:
				ret = _currEnemiesInRange[_currEnemiesInRange.Count - 1];
				break;

			case TowerTargetPriority.TTP_First:
			default:
				ret = _currEnemiesInRange[0];
				break;
		}

		return ret;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//if (towerState == TowerState.TS_PerformingAction || towerState == TowerState.TS_Broken)
		//	return; // Do not perform an action if tower is broken or is currently performing an action

		if (other.CompareTag("Enemy"))
		{
			//EnemyBehavior eb = other.gameObject.GetComponent<EnemyBehavior>();
			//if (eb != null)
			//{
			//	
			//}
			//ExecuteTowerAction(other.gameObject);
			_currEnemiesInRange.Add(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			_currEnemiesInRange.Remove(other.gameObject);
		}
	}
}
