using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ShopInventoryItem : MonoBehaviour
{
    [Tooltip("The label for the name of the item.")]
    [SerializeField] private TextMeshProUGUI _name;
    [Tooltip("The label for the price of the item.")]
    [SerializeField] private TextMeshProUGUI _price;

    private ShopLoadDetails _shopLoader;
    private Purchasable _item;

    public void Init(Purchasable item, ShopLoadDetails shopLoader)
    {
        _item = item;
        _shopLoader = shopLoader;

        _name.text = item.name;
        _price.text = "$" + item.BasePrice;
    }

    public void LoadThis()
    {
        _shopLoader.LoadDetails(_item);
    }
}
