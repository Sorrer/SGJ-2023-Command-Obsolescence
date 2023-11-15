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
	
	// Start is called before the first frame update
	private void Start()
	{
		AddPhysics2DRaycaster();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void SetupTileComponent(TileType tt, int x, int y)
	{
		type = tt;
		xPos = x;
		yPos = y;
	}

	/**
	 * @brief Function that gets fired when the mouse clicks on this object.
	 * @param eventData Idk man!
	 */
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Clicked tile x:" + xPos.ToString() + " y:" + yPos.ToString());
		
		if (towerObj == null)
		{
			Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
			towerObj = Instantiate(TowerManager.Instance.towerDictionary[TowerType.TowT_GenericTurret], newPos, transform.rotation);
			towerObj.transform.parent = transform;
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
	}
}
