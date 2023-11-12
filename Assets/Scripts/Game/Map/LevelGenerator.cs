using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField]
	private GameObject levelGenerator;

	private Tile[,] levelGrid; /**<The main level grid*/
	[SerializeField]
	private int levelWidth = 15;
	[SerializeField]
	private int levelHeight = 15;

	// Start is called before the first frame update
	void Start()
	{
		LevelGridInit();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	/**
	 * @brief Clears and resets the levelGrid with brand new blank Tile objects.
	 */
	void LevelGridClear()
	{
		System.Array.Clear(levelGrid, 0, levelGrid.Length);
		for (int i = 0; i < levelWidth; i++)
		{
			for (int j = 0; j < levelHeight; j++)
			{
				levelGrid[i, j] = new Tile();
			}
		}
	}

	/**
	 * @brief Initializes the levelGrid.
	 */
	void LevelGridInit()
	{
		levelGrid = new Tile[levelWidth, levelHeight];
		/*for (int i = 0; i < levelWidth; i++)
		{
			for (int j = 0; j < levelHeight; j++)
			{
				levelGrid[i, j] = new Tile();
			}
		}*/
		LevelGridClear();
	}
}
