using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
	//get the TowerManager instance
	private static TowerManager instance;
	public static TowerManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindFirstObjectByType<TowerManager>();
			}
			return instance;
		}
	}

	[Serializable]
	public struct TowermapEntry
	{
		public TowerType type;
		public GameObject towerPrefab;
	}
	[SerializeField]
	private TowermapEntry[] towerList;
	public Dictionary<TowerType, GameObject> towerDictionary;

	// Start is called before the first frame update
	void Start()
	{
		towerDictionary = new Dictionary<TowerType, GameObject>();
		foreach (TowermapEntry te in towerList)
		{
			towerDictionary.Add(te.type, te.towerPrefab);
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
