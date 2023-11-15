using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceInfoItem : MonoBehaviour
{
    [Tooltip("The label for the name of the item.")]
    [SerializeField] private TextMeshProUGUI _label;
    [Tooltip("The label for the price of the item.")]
    [SerializeField] private TextMeshProUGUI _price;

    public void Init(string label, string price)
    {
        _label.text = label;
        _price.text = price;
    }
}
