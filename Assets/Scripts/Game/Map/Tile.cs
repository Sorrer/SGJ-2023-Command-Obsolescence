using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TileType
{
	TT_Empty = 0,
	TT_Floor = 1,
	TT_Wall = 2
};

public class Tile// : MonoBehaviour
{
	public TileType type = TileType.TT_Empty;
	public TileComponent tileComponent;
	public int xPos;
	public int yPos;

	// Start is called before the first frame update
	/*void Start()
	{
		
	}*/

	// Update is called once per frame
	/*void Update()
	{
		
	}*/

	public void SetupTile(TileType tt, TileComponent tc, int x, int y)
	{
		type = tt;
		tileComponent = tc;
		xPos = x;
		yPos = y;
	}
}