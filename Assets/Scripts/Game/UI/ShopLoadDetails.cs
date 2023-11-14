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
    [SerializeField] private Scrollbar _descriptionScroll;

    public void LoadDetails(Purchasable item)
    {
        _nameplate.text = item.name;
        _subheader.text = item.name;
        _description.text = item.Description;
        _descriptionScroll.value = 1;
    }
}
