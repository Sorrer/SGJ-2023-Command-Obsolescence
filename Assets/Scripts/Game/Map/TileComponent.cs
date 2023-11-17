using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour, IPointerDownHandler
{
	public TileType type = TileType.TT_Empty;
	[SerializeField]
	private GameObject towerObj = null; /**< Tower currently on the tile */
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

	public void SetupTileComponent(TileType tt, int x, int y, bool _checkerboardTiles)
	{
		type = tt;
		xPos = x;
		yPos = y;
		checkerboardTiles = _checkerboardTiles;
	}

	/**
	 * @brief Function that gets fired when the mouse clicks on this object.
	 * @param eventData Idk man!
	 */
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Clicked tile x:" + xPos.ToString() + " y:" + yPos.ToString());
		
		PointerModes mode = PointerMode.Instance.Mode;

		if (mode == PointerModes.ADD && towerObj == null)
		{
			// Find which tower is currently selected
			Purchasable p = ShopInventory.Instance.PurchaseCurrentSelectedItem();
			if (p != null && p.ItemObject != null)
			{
				Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
				towerObj = Instantiate(p.ItemObject, /*TowerManager.Instance.towerDictionary[TowerType.TowT_GenericTurret],*/ newPos, transform.rotation);
				towerObj.transform.parent = transform;
			}
		}
		else if (mode == PointerModes.DESTROY && towerObj != null)
		{
			Destroy(towerObj);
			towerObj = null;
		}
		else if (mode == PointerModes.UPGRADE && towerObj != null)
		{
			Tower tower = towerObj.GetComponent<Tower>();
			if (tower)
			{
				tower.UpgradeTower();
			} 
		}
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
}
