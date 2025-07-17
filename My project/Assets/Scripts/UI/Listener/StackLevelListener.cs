using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class StackLevelListener : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI maxStackText;
    [SerializeField] TextMeshProUGUI currentStackText;
    [SerializeField] Image fillImage;

    [Header("Fill Settings")]
    [SerializeField] float fillDuration = 0.3f;

    private int currentStack = 0;
    private int maxStack = 3; // fix: initial stack value should be stored in other place
    private int lastMaxStack = 1;

    void OnEnable()
    {
        UpgradeController.levelChanged += UpdateLevelText;
        UpgradeController.maxStackChanged += UpdateMaxStackValue;
        BodyStacker.StackCountChanged += UpdateCurrentStackValue;
    }

    void OnDisable()
    {
        UpgradeController.levelChanged -= UpdateLevelText;
        UpgradeController.maxStackChanged -= UpdateMaxStackValue;
        BodyStacker.StackCountChanged -= UpdateCurrentStackValue;
    }

    private void UpdateLevelText(int level)
    {
        levelText.SetText($"Level {level}");
    }

    private void UpdateMaxStackValue(int maxStackValue)
    {
        maxStack = maxStackValue;
        maxStackText.SetText(maxStack.ToString());

        // Detecta mudança
        if (maxStack != lastMaxStack)
        {
            // Debug.Log($"MaxStack upgrade: {lastMaxStack} → {maxStack}");
            lastMaxStack = maxStack;
        }

        // Atualiza o fill se já tinha valor anterior
        UpdateFill(currentStack, maxStack);
    }

    private void UpdateCurrentStackValue(int currentStackValue)
    {
        currentStack = currentStackValue;
        currentStackText.SetText(currentStack.ToString());

        UpdateFill(currentStack, maxStack);

        if (currentStack >= maxStack)
        {
            // Debug.Log("Stack cheio! Level up visual!");
            TriggerLevelUpEffect();
        }
    }

    private void UpdateFill(int current, int max)
    {
        if (fillImage == null || max == 0) return;

        float targetFill = Mathf.Clamp01(current / (float)max);
        fillImage.DOKill();
        fillImage.DOFillAmount(targetFill, fillDuration).SetEase(Ease.OutQuad);
    }

    private void TriggerLevelUpEffect()
    {
        if (fillImage == null) return;

        fillImage.transform.DOKill();
        fillImage.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 8, 1f);
    }
}
