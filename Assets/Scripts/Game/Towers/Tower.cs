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

public class Tower : MonoBehaviour, IPointerDownHandler
{
	// Internal values
	public string towerName;		/**< String name of the tower. */
	[SerializeField]
	protected TowerType towerType;	/**< Type of tower. */
	
	// Start is called before the first frame update
	protected virtual void Start()
	{
		AddPhysics2DRaycaster();
	}

	// Update is called once per frame
	void Update()
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
	public virtual void ExecuteTowerAction()
	{

	}

	/**
	 * @brief Overrideable function proto for an action that can occur when the tower is destroyed.
	 */
	public virtual void OnTowerDestroy()
	{

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
}
