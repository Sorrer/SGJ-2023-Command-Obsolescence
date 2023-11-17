using System;
using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public EnemyMapReference mapReference;
	//get the LevelGenerator instance
	private static LevelGenerator instance;
	public static LevelGenerator Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindFirstObjectByType<LevelGenerator>();
			}
			return instance;
		}
	}

	private BoxCollider2D box2d;
	private float offsetX = 1.0f, offsetY = 1.0f;

	[SerializeField]
	private GameObject levelGenerator;
	[SerializeField]
	private GameObject tileObj;

	private Tile[,] levelGrid; /**<The main level grid*/
	[SerializeField]
	private int levelWidth = 15;
	[SerializeField]
	private int levelHeight = 15;

	//[SerializeField]
	//private float offsetSpawnXStart = -15.0f;
	//[SerializeField]
	//private float offsetSpawnYStart = -15.0f;

	[Serializable]
	public struct TilemapEntry
	{
		public TileType type;
		public Sprite sprite;
	}
	[SerializeField]
	private TilemapEntry[] testTilemap;
	public Dictionary<TileType, Sprite> testTilemapDict;
	[SerializeField]
	private bool checkerboardTiles = true;

	// Start is called before the first frame update
	void Start()
	{
		testTilemapDict = new Dictionary<TileType, Sprite>();
		foreach (TilemapEntry te in testTilemap)
		{
			testTilemapDict.Add(te.type, te.sprite);
		}

		if (tileObj)
			box2d = tileObj.GetComponent<BoxCollider2D>();
		if (box2d != null)
		{
			offsetX = box2d.size.x;
			offsetY = box2d.size.y;
		}

		LevelGridInit();
		SpawnTileObjects();
		if(mapReference != null) mapReference.LoadMap(levelGrid);
	}

	// Update is called once per frame
	void Update()
	{

	}

	/**
	 * @brief Destroy the previous GameObjects creating the previous level.
	 */
	private IEnumerator DestroyPreviousLevel()
	{
		foreach (Transform child in levelGenerator.transform)
		{
			GameObject.Destroy(child.gameObject);
			/*i++;
			if (i > 300)
			{
				yield return new WaitForSeconds(0.1f);
				i = 0;
			}*/
		}
		//yield return null;
		LevelGridClear();
		yield return null;
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
				levelGrid[i, j].SetupTile(TileType.TT_Floor, null, i, j);
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

	/**
	 * @brief Instantiates tile GameObjects after chosen level generator completes.
	 */
	void SpawnTileObjects()
	{
		for (int i = 0; i < levelWidth; i++)
		{
			for (int j = 0; j < levelHeight; j++)
			{
				//if (levelGrid[i, j].type == TileType.TT_Floor || levelGrid[i, j].type == TileType.TT_Wall)
				//{
					BoxCollider2D box2d = tileObj.GetComponent<BoxCollider2D>();
					float offsetX = 1.0f, offsetY = 1.0f;
					if (box2d != null)
					{
						offsetX = box2d.size.x/* * 2*/;
						offsetY = box2d.size.y;
					}
					//Vector3 newPos = new Vector3(i+tileObj.GetComponent<SpriteRenderer>().sprite.rect.width, 
					//	j+tileObj.GetComponent<SpriteRenderer>().sprite.rect.height, 0);
					Vector3 newPos = new Vector3(i*offsetX /*+ offsetSpawnXStart*/, j*offsetY /*+ offsetSpawnYStart*/, 0);
					GameObject _tileObj = Instantiate(tileObj, newPos, transform.rotation);
					SpriteRenderer sr = _tileObj.GetComponent<SpriteRenderer>();
					if (sr == null)
						Debug.Log("sr was null!");
					sr.sprite = testTilemapDict[levelGrid[i, j].type];
					_tileObj.transform.parent = levelGenerator.transform;
					Collider2D col = _tileObj.GetComponent<Collider2D>();
					/*if (levelGrid[i, j].type == TileType.TT_Wall) // Enable colliders for walls
					{
						col.enabled = true;
						_tileObj.tag = "Wall";
					}
					else
						col.enabled = false;*/
					TileComponent _tile = _tileObj.GetComponent<TileComponent>();
					_tile.SetupTileComponent(levelGrid[i, j].type, i, j, checkerboardTiles);
					levelGrid[i, j].tileComponent = _tile;

					// Test functions
					//Debug.Log("Testing tile i:" + i.ToString() + " j:" + j.ToString() + " at pos:" + _tile.transform.position.ToString());
					//Vector2Int gridPos = GetGridPosition(_tile.transform.position);
					//Debug.Log("Calculated grid pos:" + gridPos.ToString());
					//Vector3 worldPos = GetWorldPosition(gridPos);
					//Debug.Log("Calculated world pos:" + worldPos.ToString());
				//}
			}
		}
	}

	/**
	 * @brief Given coords in world position, calculate which Tile on the grid it would be a part of, in x,y coords.
	 * @brief worldPosition Vector3 of the object's world position (not local position relative to its parent).
	 * @returns Vector2Int of (x,y) coords on the grid.
	 */
	public Vector2Int GetGridPosition(Vector3 worldPosition)
	{
		Vector2Int value = new(Mathf.FloorToInt((worldPosition.x + (offsetX/2)) / offsetX),
			Mathf.FloorToInt((worldPosition.y+ (offsetY/2)) / offsetY));
		return value;
	}

	/**
	 * @brief Given coords on the grid, calculate the world position that Tile would reside on.
	 * @param gridPosition Vector2Int of (x,y) coords on the grid.
	 * @returns Vector3 coords in world position.
	 */
	public Vector3 GetWorldPosition(Vector2Int gridPosition)
	{
		Vector3 value = new(gridPosition.x * offsetX, gridPosition.y * offsetY, 0);
		return value;
	}

	public Vector2Int ClampGridPositionToBounds(Vector2Int gridPosition)
	{
		gridPosition.x = Math.Clamp(gridPosition.x, 0, levelWidth - 1);
		gridPosition.y = Math.Clamp(gridPosition.y, 0, levelHeight - 1);
		return gridPosition;
	}

	public bool IsWithinWorld(Vector2Int position)
	{
		return position.x >= 0 && position.x < levelWidth && position.y >= 0 && position.y <= levelHeight;
	}


	public int GetWidth()
	{
		return levelWidth;
	}
	public int GetHeight()
	{
		return levelWidth;
	}

}
