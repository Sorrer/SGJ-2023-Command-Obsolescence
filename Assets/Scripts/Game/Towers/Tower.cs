using System;
using System.Collections;
using System.Collections.Generic;
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

public class Tower : MonoBehaviour, IPointerDownHandler
{
	[Header("Internal Values")]
	public string towerName;		/**< String name of the tower. */
	[SerializeField]
	protected TowerType towerType;	/**< Type of tower. */
	[SerializeField]
	protected TowerState towerState;	/**< State the tower is currently in. */
	protected SpriteRenderer sr;
	[SerializeField]
	protected Color idleColor = Color.white;
	[SerializeField]
	protected Color brokenColor = Color.gray;

	// Tower logic settings
	protected List<GameObject> currEnemiesInRange = new List<GameObject>();

	[Header("Tower Settings")]
	[SerializeField]
	protected TowerTargetPriority towerTargetPriority;
	[SerializeField]
	protected int towerLevel;	/**< Level of tower, for upgrades. Base is level 0. */
	[SerializeField]
	protected int maxTowerLevel = 3; /**< Maximum level the tower can be upgraded to. */
	[SerializeField]
	protected GameObject projectileObj;	/**< Projectile spawned by the tower.*/
	[SerializeField]
	protected bool destroyOnBreak = false;	/**< Whether or not to delete self after the tower performs an action. */
	
	// Start is called before the first frame update
	protected virtual void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		if (sr)
			sr.color = idleColor;
		towerState = TowerState.TS_Idle;
		towerTargetPriority = TowerTargetPriority.TTP_First;
		towerLevel = 0;
		AddPhysics2DRaycaster();
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		if (towerState == TowerState.TS_Idle)
		{
			GameObject searchTarget = GetTarget();
			if (searchTarget != null)
				ExecuteTowerAction(searchTarget);
		}
	}

	/**
	 * @brief Function that gets fired when the mouse clicks on this object.
	 * @param eventData Idk man!
	 */
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Clicked on name:" + gameObject.name);
		// maybe remove these on click functions... they might interfere with the on click functions fired by the tiles
	}

	/**
	 * @brief Overrideable function proto for executing the action of a tower. Such as firing a bullet, etc.
	 */
	public virtual void ExecuteTowerAction(GameObject target)
	{
		towerState = TowerState.TS_PerformingAction;
	}

	/**
	 * @brief Resets a tower back to the "TS_Idle" state after a delay.
	 * @param waitTime Time to wait before going to the "TS_Idle" state, in seconds.
	 */
	protected IEnumerator ResetTower(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		towerState = TowerState.TS_Idle;
		if (sr)
			sr.color = idleColor;
	} 

	/**
	 * @brief Upgrades the tower by one level. Cannot go past max tower level.
	 */
	public virtual void UpgradeTower()
	{
		if (towerLevel < maxTowerLevel)
			towerLevel++;
	}

	/**
	 * @brief Overrideable function proto for an action that can occur when the tower is broken.
	 */
	public virtual void BreakTower()
	{
		towerState = TowerState.TS_Broken;
		if (sr)
			sr.color = brokenColor;
		if (destroyOnBreak)
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

	protected GameObject GetTarget()
	{
		GameObject ret = null;
		
		if (currEnemiesInRange.Count == 0)
			return null;
		else if (currEnemiesInRange.Count == 1)
			return currEnemiesInRange[0];
		
		switch (towerTargetPriority)
		{
			case TowerTargetPriority.TTP_Last:
				ret = currEnemiesInRange[currEnemiesInRange.Count - 1];
				break;

			case TowerTargetPriority.TTP_First:
			default:
				ret = currEnemiesInRange[0];
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
			currEnemiesInRange.Add(other.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			currEnemiesInRange.Remove(other.gameObject);
		}
	}
}
