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
    [Tooltip("The image for displaying the item's sprite.")]
    [SerializeField] private Image _image;

    private ShopLoadDetails _shopLoader;
    private Purchasable _item;

    public void Init(Purchasable item, ShopLoadDetails shopLoader)
    {
        _item = item;
        _shopLoader = shopLoader;
        
        _name.text = item.name;
        _price.text = "$" + item.BasePrice;
        _image.sprite = item.StoreSprite;
    }

    public void ClickThis()
    {
        PointerMode.Instance.Mode = PointerModes.ADD;
        LoadThis();
    }

    public void LoadThis()
    {
        _shopLoader.LoadDetails(_item);
    }
}
