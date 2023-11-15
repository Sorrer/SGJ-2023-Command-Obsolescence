using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestBank : MonoBehaviour
{
    public TextMeshProUGUI Balance;
    public Color GoodColor;
    public Color BadColor;

    public void UpdateBalance()
    {
        Balance.text = "$" + Bank.Instance.CurrentBalance;
        if (Bank.Instance.CurrentBalance > 50) Balance.color = GoodColor;
        else Balance.color = BadColor;
    }

    public void Add50()
    {
        Bank.Instance.AddToBalance(50);
    }

    public void Remove50()
    {
        Bank.Instance.RemoveFromBalance(50);
    }
}
