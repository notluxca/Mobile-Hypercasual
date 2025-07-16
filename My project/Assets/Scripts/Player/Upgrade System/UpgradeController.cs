using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int stackLimit = 15;
    public int stackUpgradeIncrease = 2;
    public PlayerController playerController;

    [Header("Visual Feedback")]
    public Color[] upgradeColors; // Lista de cores para trocar
    private int currentColorIndex = 0;

    private Renderer characterRenderer;

    void Start()
    {
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
        playerController.maxStackLimit += stackUpgradeIncrease;
        ChangeColor();
    }

    public void ChangeColor()
    {
        if (upgradeColors.Length == 0 || characterRenderer == null)
            return;

        currentColorIndex = (currentColorIndex + 1) % upgradeColors.Length;
        characterRenderer.material.color = upgradeColors[currentColorIndex];
    }
}
