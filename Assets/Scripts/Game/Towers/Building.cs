using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.CompilerServices;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public enum EntityType // Do we need these and name at the same time?
{
	Unknown = 0,
	GunTurret = 1,
	Flamethrower = 2,
	BigSucc = 3,
	Pickaxe = 4,
	MissleLauncher = 5
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
	TD_Up = 0,
	TD_Right = 1,
	TD_Down = 2,
	TD_Left = 3
}

public class Building : TileEntity
{
	[Header("Building Values")]
	[Tooltip("The state the tower is currently in.")]
	[SerializeField] protected TowerState _towerState;
	[Tooltip("The color to tint this tower when it is idle.")]
	[SerializeField] protected Color _idleColor = Color.white;
	[Tooltip("The color to tint this tower when it is broken.")]
	[SerializeField] protected Color _brokenColor = Color.gray;

	// Tower logic settings
	protected List<GameObject> _currEnemiesInRange = new List<GameObject>();

	[Header("Tower Settings")]
	[SerializeField] protected TowerTargetPriority _towerTargetPriority;
	[Tooltip("Level of tower, for upgrades. Base is level 0.")]
	[SerializeField] protected int _towerLevel;
	[Tooltip("Maximum level the tower can be upgraded to.")]
	[SerializeField] protected int _maxTowerLevel = 3;
	[Tooltip("The purchasable for this tower.")]
	[SerializeField] protected Purchasable _purchaseInfo;

	[Tooltip("The sprite to use when facing upward.")]
	[SerializeField] protected Sprite _upSprite;
	[Tooltip("The sprite to use when facing downward.")]
	[SerializeField] protected Sprite _downSprite;
	[Tooltip("The sprite to use when facing leftward.")]
	[SerializeField] protected Sprite _leftSprite;
	[Tooltip("The sprite to use when facing rightward.")]
	[SerializeField] protected Sprite _rightSprite;

	/// <summary>
	/// The sprites for all four directions of the tower. The array is orderered up, right, down, left.
	/// </summary>
	public Sprite[] DirectionalSprites => new Sprite[] {_upSprite, _rightSprite, _downSprite, _leftSprite, };
	[Tooltip("The direction this tower is facing.")]
	public TowerDirection CurrentDirection {get { return _currentDirection; } set 
	{
		_currentDirection = value;
		_sr.sprite = DirectionalSprites[(int) _currentDirection];
	}}

	private TowerDirection _currentDirection = TowerDirection.TD_Up;
	
	// Start is called before the first frame update
	protected virtual void Start()
	{
		_sr.color = _idleColor;

		_towerState = TowerState.TS_Idle;
		_towerTargetPriority = TowerTargetPriority.TTP_First;
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
		_sr.color = _brokenColor;
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
