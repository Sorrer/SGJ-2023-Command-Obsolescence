using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Enemies;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TileComponent : MonoBehaviour, IPointerDownHandler
{
	public TileType type = TileType.TT_Empty;
	
	/**< Tower currently on the tile */
	[FormerlySerializedAs("towerObj")] [SerializeField]
	private TileEntity tileEntity = null;
	

	[Serializable]
	public struct ObstacleData
	{
		public GameObject prefab;
		public int weight;
	}
	
	[SerializeField] public ObstacleData[] obstaclePrefabList;
	
	public int xPos;
	public int yPos;
	private SpriteRenderer sr;
	private bool checkerboardTiles;

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
			int totalWeight = obstaclePrefabList.Sum(e => e.weight);

			int weight = Random.Range(0, totalWeight);

			foreach (var obj in obstaclePrefabList)
			{
				weight -= obj.weight;

				if (weight <= 0)
				{
					CreateTileEntity(obj.prefab);
					break;
				}
			}

		}


	}


	public void CreateTileEntity(GameObject prefab)
	{
		if (tileEntity != null)
		{
			Debug.LogError("Tried to create a tile entity on a tile component when the tile entity already exists");
			return;
		}
		
		var entity = Instantiate(prefab, this.transform);
		Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
		entity.transform.position = newPos;

		this.tileEntity = entity.GetComponent<TileEntity>();
		
		if (tileEntity == null)
		{
			Debug.LogError("Tried to create a tile entity on a title component, but not tile entity component on the prefab was found");
		}
	}


	/**
	 * @brief Function that gets fired when the mouse clicks on this object.
	 * @param eventData Idk man!
	 */
	public void OnPointerDown(PointerEventData eventData)
	{
		if (tileEntity != null &&!tileEntity.isInteractable) return;
		
		PointerModes mode = PointerMode.Instance.Mode;
		int balance = Bank.Instance.CurrentBalance;

		if (mode == PointerModes.ADD)
		{
			Purchasable p = ShopInventory.Instance.GetCurrentSelectedItem();

			if (tileEntity != null) 
			{
				TileEntity old = tileEntity.GetComponent<Building>();
				
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
				CreateTileEntity(p.ItemObject.gameObject);
				Building tower = tileEntity.GetComponent<Building>();
				if (tower)
				{
					Debug.Log("Spawned tower");
					tower.CurrentDirection = PointerMode.Instance.Direction;
				}
			}
		}
		else if (mode == PointerModes.DESTROY && tileEntity != null && balance >= TileEntity.STANDARD_DESTROY_COST)
		{
			Building tower = tileEntity.GetComponent<Building>();
			if (tower && tower.CanBeDestroyed)
			{
				Bank.Instance.RemoveFromBalance(TileEntity.STANDARD_DESTROY_COST);
				tower.OnTowerDestroy();
				DestroyEntity();
				tileEntity = null;
			}
		}
		else if (mode == PointerModes.UPGRADE && tileEntity != null)
		{
			Building tower = tileEntity.GetComponent<Building>();
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
		if (tileEntity == null) return true;
		return !tileEntity.IsWall;
	}

	public IAttackable GetAttackable()
	{
		if (tileEntity == null) return null;
		return tileEntity.GetComponent<IAttackable>(); // Expensive but fuck we ball
	}

	public void DestroyEntity()
	{
		Destroy(tileEntity.gameObject);
	}
}
