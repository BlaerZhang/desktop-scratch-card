using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private int _currency = 0;
    public int Currency
    {
        get => _currency;
        set
        {
            if (_currency == value) return;
            _currency = Math.Max(value, 0);
            UpdateCurrencyUI();
            onCurrencyChanged?.Invoke();
        }
    }

    public static Action onCurrencyChanged;

    [SerializeField] private TMP_Text currencyUIText;

    void Start()
    {
        UpdateCurrencyUI();
    }

    void UpdateCurrencyUI()
    {
        DOVirtual.Int(Int32.Parse(currencyUIText.text), Currency, 0.5f,
            value => currencyUIText.text = value.ToString());
    }
}
