using System;
using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class TileComponent : MonoBehaviour, IPointerDownHandler
{
	public TileType type = TileType.TT_Empty;
	
	/**< Tower currently on the tile */
	[SerializeField]
	private GameObject towerObj = null;
	
	[SerializeField] 
	private GameObject obstacleObj;

	[Serializable]
	public struct ObstacleData
	{
		public GameObject prefab;
		public bool isTraversable;
	}
	
	[SerializeField] public ObstacleData[] obstaclePrefabList;
	
	public int xPos;
	public int yPos;
	private SpriteRenderer sr;
	private bool checkerboardTiles;

	private bool isInteractable = true;
	private bool isTraversable = true;
	
	// Start is called before the first frame update
	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		AddPhysics2DRaycaster();

		if (checkerboardTiles && sr != null)
		{
			if (yPos % 2 == 0)
			{
				if (xPos % 2 == 1)
					sr.color = Color.gray;
				else
					sr.color = Color.white;
			}
			else
			{
				if (xPos % 2 == 0)
					sr.color = Color.gray;
				else
					sr.color = Color.white;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void SetupTileComponent(TileType tt, int x, int y, bool _checkerboardTiles, bool isObstacle)
	{
		type = tt;
		xPos = x;
		yPos = y;
		checkerboardTiles = _checkerboardTiles;

		if (isObstacle)
		{
			
			var obstacle = obstaclePrefabList[Random.Range(0, obstaclePrefabList.Length)];
			
			this.obstacleObj = Instantiate(obstacle.prefab);
			this.obstacleObj.transform.position = this.transform.position;
			isInteractable = false;
			isTraversable = obstacle.isTraversable;
		}
		else
		{
			isInteractable = true;
			isTraversable = true;
		}


	}
	


	/**
	 * @brief Function that gets fired when the mouse clicks on this object.
	 * @param eventData Idk man!
	 */
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!isInteractable) return;
		
		PointerModes mode = PointerMode.Instance.Mode;
		int balance = Bank.Instance.CurrentBalance;

		if (mode == PointerModes.ADD)
		{
			Purchasable p = ShopInventory.Instance.GetCurrentSelectedItem();

			if (towerObj != null) 
			{
				TileEntity old = towerObj.GetComponent<Building>();
				
				if (old && old.Replacement.Equals(p.Name)) 
				{
					Debug.Log("Cannot replace");
					return;
				}
			}

			ShopInventory.Instance.PurchaseCurrentSelectedItem();

			if (p != null && p.ItemObject != null)
			{

				Debug.Log("Spawning tower");
				Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
				towerObj = Instantiate(p.ItemObject.gameObject, newPos, transform.rotation);
				towerObj.transform.parent = transform;
				Building tower = towerObj.GetComponent<Building>();
				if (tower)
				{
					Debug.Log("Spawned tower");
					tower.CurrentDirection = PointerMode.Instance.Direction;
				}
			}
		}
		else if (mode == PointerModes.DESTROY && towerObj != null && balance >= TileEntity.STANDARD_DESTROY_COST)
		{
			Building tower = towerObj.GetComponent<Building>();
			if (tower && tower.CanBeDestroyed)
			{
				Bank.Instance.RemoveFromBalance(TileEntity.STANDARD_DESTROY_COST);
				Destroy(towerObj);
				towerObj = null;
			}
		}
		else if (mode == PointerModes.UPGRADE && towerObj != null)
		{
			Building tower = towerObj.GetComponent<Building>();
			if (tower)
			{
				tower.TryUpgradeTower();
			} 
		}
		
		LevelGenerator.Instance.UpdateTile(new Vector2Int(xPos,yPos), this);
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
		if (physicsRaycaster != null)
			physicsRaycaster.eventMask = LayerMask.GetMask("Tile");
	}


	public int GetWeight() // Can support negatives up to a point
	{
		return 1;
	}

	public bool IsTraversal() 
	{
		return isTraversable;
	}

	public IAttackable GetAttackable()
	{
		if (towerObj == null) return null;
		return towerObj.GetComponent<IAttackable>(); // Expensive but fuck we ball
	}
}
