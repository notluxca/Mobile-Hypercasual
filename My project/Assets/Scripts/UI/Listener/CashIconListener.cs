using System;
using TMPro;
using UnityEngine;
using DG.Tweening; // Add this for DOTween

public class CashIconListener : MonoBehaviour
{
    public TextMeshProUGUI cashText;

    void OnEnable()
    {
        CurrencyManager.cashChanged += UpdateText;
    }

    void OnDisable()
    {
        CurrencyManager.cashChanged -= UpdateText;
    }

    private void UpdateText(int CashValue)
    {
        cashText.SetText(CashValue.ToString());

        // DOTween pop animation
        cashText.transform.DOKill(); // Stop any ongoing tweens
        cashText.transform.localScale = Vector3.one;
        cashText.transform.DOScale(1.2f, 0.15f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                cashText.transform.DOScale(1f, 0.15f).SetEase(Ease.InBack);
            });
    }
}
