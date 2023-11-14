using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [Tooltip("The purchasables available in the store.")]
    [SerializeField] private Purchasable[] _inventory;
    [Tooltip("The UI item prefab that will be used as the basis for each")]
    [SerializeField] private ShopInventoryItem _listItemPrefab; 

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
