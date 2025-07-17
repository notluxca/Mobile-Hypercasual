using System;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int stackUpgradeIncrease = 2;
    public Color[] upgradeColors;

    private PlayerController playerController;
    private int currentLevel;
    public static Action<int> levelChanged;
    public static Action<int> maxStackChanged;
    private int currentColorIndex = 0;
    private Renderer characterRenderer;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterRenderer = GetComponentInChildren<Renderer>();
    }

    void Start()
    {
        currentLevel = 1; // initialize on level 1
        if (upgradeColors.Length > 0) // initialize on first level color
            characterRenderer.material.color = upgradeColors[currentColorIndex];
    }

    public void UpgradeCharacter()
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
