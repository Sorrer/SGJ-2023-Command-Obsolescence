using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Tower : MonoBehaviour
{
	// Internal values
	public string towerName;		/**< String name of the tower. */
	[SerializeField]
	protected TowerType towerType;	/**< Type of tower. */
	
	// Start is called before the first frame update
	protected virtual void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
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
}
