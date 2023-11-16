using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    #region Singleton Stuff 

    public static ShopInventory Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    #endregion

    [Tooltip("The purchasables available in the store.")]
    [SerializeField] private Purchasable[] _inventory;
    [Tooltip("The UI item prefab that will be used as the basis for each list item.")]
    [SerializeField] private ShopInventoryItem _listItemPrefab;
    [Tooltip("The script responsible for loading the details of an item in the details section.")]
    [SerializeField] private ShopLoadDetails _shopLoader;
    
    private Purchasable _currentSelectedItem;

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

    public void SetCurrentSelectedItem(Purchasable purchasable)
    {
        _currentSelectedItem = purchasable;
    }

    public Purchasable GetCurrentSelectedItem()
    {
        return _currentSelectedItem;
    }

    public Purchasable PurchaseCurrentSelectedItem()
    {
        if (_currentSelectedItem == null)
            return null;
        
        if (_currentSelectedItem.BasePrice <= Bank.Instance.CurrentBalance)
        {
            Bank.Instance.RemoveFromBalance(_currentSelectedItem.BasePrice);
            return _currentSelectedItem;
        }

        return null;
    }
}
