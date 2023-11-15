using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [Tooltip("The purchasables available in the store.")]
    [SerializeField] private Purchasable[] _inventory;
    [Tooltip("The UI item prefab that will be used as the basis for each list item.")]
    [SerializeField] private ShopInventoryItem _listItemPrefab;
    [Tooltip("The script responsible for loading the details of an item in the details section.")]
    [SerializeField] private ShopLoadDetails _shopLoader;

    void Start()
    {
        bool loadFirst = false;
        foreach (Purchasable item in _inventory)
        {
            ShopInventoryItem listing = Instantiate(_listItemPrefab, gameObject.transform);
            listing.Init(item, _shopLoader);
            if (!loadFirst)
            {
                listing.LoadThis();
                loadFirst = true;
            }
        }
    }
}
