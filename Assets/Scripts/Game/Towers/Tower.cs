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

public class Tower : MonoBehaviour, IPointerDownHandler
{
	[Header("Internal Values")]
	public string towerName;		/**< String name of the tower. */
	[SerializeField]
	protected TowerType towerType;	/**< Type of tower. */
	[SerializeField]
	protected TowerState towerState;	/**< State the tower is currently in. */

	[Header("Tower Settings")]
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
		towerState = TowerState.TS_Idle;
		towerLevel = 0;
		AddPhysics2DRaycaster();
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (towerState == TowerState.TS_PerformingAction || towerState == TowerState.TS_Broken)
			return; // Do not perform an action if tower is broken or is currently performing an action

		if (other.tag == "Enemy")
		{
			//EnemyBehavior eb = other.gameObject.GetComponent<EnemyBehavior>();
			//if (eb != null)
			//{
			//	
			//}
			ExecuteTowerAction(other.gameObject);
		}
	}
}
