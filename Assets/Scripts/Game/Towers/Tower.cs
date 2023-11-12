using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TowerType
{
	TT_Unknown = 0,
	TT_GenericTurret = 1,
	TT_BombermanBlast = 2,
	TT_BigSucc = 3,
	TT_Pickaxe = 4
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
