using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPurchasable", menuName = "CommandObsolescence/Purchasable")]
public class Purchasable : ScriptableObject
{
    [Tooltip("The name of the purchasable.")]
    public string Name;
    [Tooltip("The base price of the purchasable.")]
    public int BasePrice;
    [Tooltip("The list of prices for each level of upgrade available for this purchasable.")]
    public int[] UpgradePrices;   
    [TextArea(minLines: 10, maxLines: 30)]
    [Tooltip("The description of the item in the shop.")]
    public string Description;
    [Tooltip("The sprite that will be used for this item in the store.")]
    public Sprite StoreSprite;
}
