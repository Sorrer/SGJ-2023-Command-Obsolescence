using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Script for handling all of the shop UI interactions.
/// </summary>
public class ShopOpenClose : MonoBehaviour
{
    [Tooltip("The curve that dictates how the shop eases in and out when sliding on- and off-screen.")]
    [SerializeField] private AnimationCurve _easing;
    [Tooltip("The speed of the shop open/close animation.")]
    [SerializeField] private float _animationSpeed;

    public bool IsAnimating => _animationDirection != 0;
    
    private RectTransform _shop;
    private int _animationDirection = 0;
    private float _animationFactor = 1;

    void Start()
    {
        _shop = GetComponent<RectTransform>();    
    }

    void Update()
    {
        Debug.Log(_animationDirection);
        if (_animationDirection > 0)
        {
            _animationFactor += _animationSpeed * Time.deltaTime;
            if (_animationFactor >= 1)
            {
                _animationFactor = 1;
                _animationDirection = 0;
            }
        }
        else if (_animationDirection < 0)
        {
            _animationFactor -= _animationSpeed * Time.deltaTime;
            if (_animationFactor <= 0)
            {
                _animationFactor = 0;
                _animationDirection = 0;
            }
        }

        float newX = _easing.Evaluate(_animationFactor) * _shop.rect.width;
        _shop.anchoredPosition = new Vector2(newX, _shop.anchoredPosition.y);
    }

    private void Open()
    {
        _animationDirection = -1;
    }

    private void Close()
    {
        _animationDirection = 1;
    }

    public void Toggle()
    {
        if (IsAnimating) return;
        if (_animationFactor == 0) Close();
        else Open();
    }
}
