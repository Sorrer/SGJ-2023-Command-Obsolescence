using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePointerMode : MonoBehaviour
{
    [Tooltip("The mode to change to on click.")]
    [SerializeField] private PointerModes _mode;

    void Start()
    {
        Button me = GetComponent<Button>();
        if (me) me.onClick.AddListener(() => { PointerMode.Instance.Mode = _mode; });
    }
}
