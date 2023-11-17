using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateLedger : MonoBehaviour
{
    [Tooltip("The textbox containing the ledger amount.")]
    [SerializeField] private TextMeshProUGUI _textbox;

    void Start()
    {
        Bank.Instance.OnBalanceDecrease.AddListener((_) => Rerender());
        Bank.Instance.OnBalanceIncrease.AddListener((_) => Rerender());
        Rerender();
    }

    public void Rerender()
    {
        _textbox.text = "$" + Bank.Instance.CurrentBalance;
    }
}
