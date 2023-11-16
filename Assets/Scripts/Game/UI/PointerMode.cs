using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PointerModes { NONE, ADD, DESTROY, UPGRADE }

public class PointerMode : MonoBehaviour
{
    #region Singleton Stuff 

    public static PointerMode Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    #endregion

    public PointerModes Mode { get { return _mode; } set 
    {
        Debug.Log("setting mode to " + value);
        _mode = value;
        if (value == PointerModes.ADD)
        {
            _modeCursorObject.sprite = _addSprite;
        }
        else if (value == PointerModes.DESTROY)
        {
            _modeCursorObject.sprite = _destroySprite;
        }
        else if (value == PointerModes.UPGRADE)
        {
            _modeCursorObject.sprite = _upgradeSprite;
        }
        else 
        {
            _modeCursorObject.sprite = null;
            _modeCursorObject.color = Color.clear;
            return;
        }

        _modeCursorObject.color = Color.white;
    }}

    private PointerModes _mode;

    [Tooltip("The object for displaying the cursor itself.")]
    [SerializeField] private RectTransform _mainCursorObject;
    [Tooltip("The object for displaying the cursor mode icon.")]
    [SerializeField] private Image _modeCursorObject;
    [Tooltip("An additional graphic that will appear next to the cursor when adding buildings.")]
    [SerializeField] private Sprite _addSprite;
    [Tooltip("An additional graphic that will appear next to the cursor when destroying buildings.")]
    [SerializeField] private Sprite _destroySprite;
    [Tooltip("An additional graphic that will appear next to the cursor when upgrading buildings.")]
    [SerializeField] private Sprite _upgradeSprite;

    void Start()
    {
        Mode = PointerModes.NONE;
        Cursor.visible = false;
    }

    void Update()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
		eventData.position = Input.mousePosition;
		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, raycastResults);

        bool inMenu = false;
		foreach (RaycastResult result in raycastResults)
		{
			if (result.gameObject.layer == LayerMask.NameToLayer("UI")) 
            {
                inMenu = true;
                break;
            }
		}

        if (inMenu)
        {
            _modeCursorObject.gameObject.SetActive(false);
        }
        else
        {
            _modeCursorObject.gameObject.SetActive(true);
        }

        _mainCursorObject.gameObject.transform.position = Input.mousePosition;
    }
}
