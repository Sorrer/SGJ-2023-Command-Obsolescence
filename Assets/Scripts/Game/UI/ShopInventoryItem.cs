using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInventoryItem : MonoBehaviour
{
    [Tooltip("The label for the name of the item.")]
    [SerializeField] private TextMeshProUGUI _name;
    [Tooltip("The label for the price of the item.")]
    [SerializeField] private TextMeshProUGUI _price;

    private Purchasable _item;

    public void Init(Purchasable item)
    {
        _item = item;
        _name.text = item.name;
        _price.text = "$" + item.BasePrice;
    }
}
