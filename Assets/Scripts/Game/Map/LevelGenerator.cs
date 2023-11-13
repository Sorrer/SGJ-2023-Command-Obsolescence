using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField]
	private GameObject levelGenerator;
	[SerializeField]
	private GameObject tileObj;

	private Tile[,] levelGrid; /**<The main level grid*/
	[SerializeField]
	private int levelWidth = 15;
	[SerializeField]
	private int levelHeight = 15;

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

		LevelGridInit();
		SpawnTileObjects();
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
				levelGrid[i, j].SetupTile(TileType.TT_Floor, i, j);
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
					Vector3 newPos = new Vector3(i*offsetX, j*offsetY, 0);
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
					_tile.SetupTileComponent(levelGrid[i, j].type, i, j);
				//}
			}
		}
	}
}
