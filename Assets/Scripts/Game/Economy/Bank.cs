using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script for controlling the player's money.
/// </summary>
public class Bank : MonoBehaviour
{
    #region Singleton Stuff 

    public static Bank Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    #endregion

    [Tooltip("The amount of money the player starts the game with.")]
    [SerializeField] private int _startingBalance = 1000;
    [Tooltip("The amount of money earns every passive income tick.")]
    [SerializeField] private int _passiveIncomeAmount;
    [Tooltip("How often passive income ticks occur.")]
    [SerializeField] private float _passiveIncomeRate;

    [Tooltip("An event that fires whenever the player gains money from any source.")]
    public UnityEvent<int> OnBalanceIncrease = new UnityEvent<int>();
    [Tooltip("An event that fires whenever the player loses money for any reason.")]
    public UnityEvent<int> OnBalanceDecrease = new UnityEvent<int>();

    public int CurrentBalance { get { return _balance; } }

    private int _balance = 0;
    private float _passiveIncomeTimer = 0;

    private void Start()
    {
        _balance = _startingBalance;
        _passiveIncomeTimer = _passiveIncomeRate;
    }

    private void Update()
    {
        _passiveIncomeTimer -= Time.deltaTime;
        if (_passiveIncomeTimer <= 0)
        {
            AddToBalance(_passiveIncomeAmount);
            _passiveIncomeTimer = _passiveIncomeRate;
        }
    }

    /// <summary>
    /// Adds money to the player's balance. Also triggers the event <c>OnBalanceIncrease<c>.
    /// </summary>
    /// <param name="amount">The amount of money to add.</param>
    public void AddToBalance(int amount)
    {
        _balance += amount;
        OnBalanceIncrease.Invoke(amount);
    }

    /// <summary>
    /// Subtracts money from the player's balance. If the player does not have the full amount, just sets the balance to zero.
    /// Also triggers the event <c>OnBalanceDecrease<c>.
    /// </summary>
    /// <param name="amount">The amount of money to attempt to subtract.</param>
    public void RemoveFromBalance(int amount)
    {
        if (amount > _balance) amount = _balance;
        _balance -= amount;

        OnBalanceDecrease.Invoke(amount);
    }
}
