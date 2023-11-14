using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPurchasable", menuName = "CommandObsolescence/Purchasable")]
public class Purchasable : ScriptableObject
{
    [Tooltip("The name of the purchasable.")]
    public readonly string Name;
    [Tooltip("The base price of the purchasable.")]
    public readonly int BasePrice;
    [Tooltip("The list of prices for each level of upgrade available for this purchasable.")]
    public readonly int[] UpgradePrices;   
}
