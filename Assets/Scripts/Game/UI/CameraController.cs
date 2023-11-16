using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
	private Rigidbody2D rb;
	private Camera cam;

	[SerializeField]
	private float xMax = 10.0f;
	[SerializeField]
	private float yMax = 10.0f;
	[SerializeField]
	private float xMin = -10.0f;
	[SerializeField]
	private float yMin = -10.0f;

	[SerializeField]
	private float minOrthographicSize = 3.0f;
	[SerializeField]
	private float maxOrthographicSize = 20.0f;
	[SerializeField]
	private float scrollSensitivity = 2.0f;
	[SerializeField]
	private float scrollSpeed = 30.0f;
	[SerializeField]
	private bool scrollInverted = false;
	private float currentScrollDelta;

	[SerializeField]
	private float moveSpeed = 0.125f;
	//[SerializeField]
	//private string horizontalAxis = "Horizontal";
	//[SerializeField]
	//private string verticalAxis = "Vertical";

	[HideInInspector] public bool LockScroll;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		cam = GetComponent<Camera>();
		currentScrollDelta = cam.orthographicSize;
	}

	void Update()
	{
		// Zoom
		PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = Input.mousePosition;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raycastResults);

		foreach (RaycastResult result in raycastResults)
		{
			if (result.gameObject.layer == LayerMask.NameToLayer("UI")) return;
		}

		if (scrollInverted) currentScrollDelta -= Input.mouseScrollDelta.y * scrollSensitivity;
		else currentScrollDelta += Input.mouseScrollDelta.y * scrollSensitivity;
		currentScrollDelta = Mathf.Clamp(currentScrollDelta, minOrthographicSize, maxOrthographicSize);
		
		float newZoomSize = Mathf.MoveTowards(cam.orthographicSize, currentScrollDelta, scrollSpeed * Time.deltaTime);
		cam.orthographicSize = newZoomSize;
	}

	void FixedUpdate()
	{
		/*float hor = Input.GetAxis(horizontalAxis);
		float ver = Input.GetAxis(verticalAxis);
		rb.velocity = new Vector2(
			Mathf.Lerp(0, hor * moveSpeed, 0.8f),
			Mathf.Lerp(0, ver * moveSpeed, 0.8f)
		);*/

		float hor = 0.0f;
		float ver = 0.0f;

		if (Input.GetKey(KeyCode.A))
			hor -= (1.0f * moveSpeed);
		if (Input.GetKey(KeyCode.D))
			hor += (1.0f * moveSpeed);
		if (Input.GetKey(KeyCode.W))
			ver += (1.0f * moveSpeed);
		if (Input.GetKey(KeyCode.S))
			ver -= (1.0f * moveSpeed);
		
		transform.position += new Vector3(hor, ver, 0);
	}

	// Update is called once per frame
	void LateUpdate()
	{
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
	}
}
