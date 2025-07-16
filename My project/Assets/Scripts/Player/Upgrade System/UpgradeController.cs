using System;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int stackLimit = 15;
    public int stackUpgradeIncrease = 2;
    public PlayerController playerController;

    public int currentLevel;
    public static Action<int> levelChanged;
    public static Action<int> maxStackChanged;

    [Header("Visual Feedback")]
    public Color[] upgradeColors;
    private int currentColorIndex = 0;

    private Renderer characterRenderer;

    void Start()
    {
        currentLevel = 1; // initialize on level 1
        characterRenderer = GetComponent<Renderer>();
        if (characterRenderer == null)
        {
            characterRenderer = GetComponentInChildren<Renderer>();
        }

        if (upgradeColors.Length > 0)
            characterRenderer.material.color = upgradeColors[currentColorIndex];
    }

    public void UpgradeCharacter(int value)
    {
        ChangeColor();
        playerController.maxStackLimit += stackUpgradeIncrease;
        currentLevel++;
        levelChanged?.Invoke(currentLevel);
        maxStackChanged?.Invoke(playerController.maxStackLimit);
    }

    public void ChangeColor()
    {
        if (upgradeColors.Length == 0 || characterRenderer == null)
            return;

        currentColorIndex = (currentColorIndex + 1) % upgradeColors.Length;
        characterRenderer.material.color = upgradeColors[currentColorIndex];
    }
}
