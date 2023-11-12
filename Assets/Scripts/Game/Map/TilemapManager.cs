using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
	//get the TilemapManager instance
	private static TilemapManager instance;
	public static TilemapManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindFirstObjectByType<TilemapManager>();
			}
			return instance;
		}
	}

	[Serializable]
	public struct TilemapEntry
	{
		public TileType type;
		public Sprite sprite;
	}
	[SerializeField]
	private TilemapEntry[] testTilemap;
	public Dictionary<TileType, Sprite> testTilemapDict;

	// Start is called before the first frame update
	void Start()
	{
		testTilemapDict = new Dictionary<TileType, Sprite>();
		foreach (TilemapEntry te in testTilemap)
		{
			testTilemapDict.Add(te.type, te.sprite);
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
