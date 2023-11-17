using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopLoadDetails : MonoBehaviour
{
    [Tooltip("The text field that will display the name of the current shop selection.")]
    [SerializeField] private TextMeshProUGUI _nameplate;
    [Tooltip("A secondary text field for displaying the name of the current shop selection, specifically under the details header.")]
    [SerializeField] private TextMeshProUGUI _subheader;
    [Tooltip("A text box for displaying an item's description.")]
    [SerializeField] private TextMeshProUGUI _description;
    [Tooltip("The scrollbar used for the description area.")]
    [SerializeField] private Scrollbar _descriptionScroll;
    [Tooltip("The UI item prefab that will be used as a basis for each price info entry.")]
    [SerializeField] private PriceInfoItem _priceInfoPrefab;
    [Tooltip("The UI element under which the price info entries will be added.")]
    [SerializeField] private RectTransform _priceInfoArea;
    [Tooltip("The image containing the sprite of the item.")]
    [SerializeField] private Image _image;

    public void LoadDetails(Purchasable item)
    {
        _nameplate.text = item.Name;
        _subheader.text = item.Name;
        _description.text = item.Description;
        _descriptionScroll.value = 1;
        _image.sprite = item.StoreSprite;

        ShopInventory.Instance.SetCurrentSelectedItem(item);

        int childCount = _priceInfoArea.childCount;
        //Debug.Log(childCount);
        for (int i = 0; i < childCount; i++)
        {
            Destroy(_priceInfoArea.gameObject.transform.GetChild(i).gameObject);
        }

        PriceInfoItem baseInfoItem = Instantiate(_priceInfoPrefab, _priceInfoArea);
        baseInfoItem.Init("Base", "$" + item.BasePrice);

        for (int i = 0; i < item.UpgradePrices.Length; i++)
        {
            PriceInfoItem priceInfoItem = Instantiate(_priceInfoPrefab, _priceInfoArea);
            priceInfoItem.Init("Lvl. " + (i + 1), "+$" + item.UpgradePrices[i]);
        }
    }
}
