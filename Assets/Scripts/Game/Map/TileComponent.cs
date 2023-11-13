using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileComponent : MonoBehaviour, IPointerDownHandler
{
	public TileType type = TileType.TT_Empty;
	public GameObject towerObj; /**< Tower currently on the tile */
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

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Clicked tile x:" + xPos.ToString() + " y:" + yPos.ToString());
	}

	private void AddPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindFirstObjectByType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }
}
