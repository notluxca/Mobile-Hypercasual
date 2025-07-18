using UnityEngine;
using System.Collections;
using TMPro;

public class UpgradeCharacterArea : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int upgradesPerCycle = 5;
    public float delayBetweenPayments = 0.3f;
    public ObjectPool MoneyPoolProvider;
    public TextMeshProUGUI progressText;

    public Transform playerTransform;
    public Transform targetPosition;

    private Coroutine upgradeRoutine;
    private bool isUpgrading = false;
    private int currentProgress;

    void Awake()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindWithTag("Player")?.transform;

        currentProgress = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUpgrading)
        {
            UpgradeController upgradeController = other.GetComponent<UpgradeController>();
            if (upgradeController != null)
            {
                upgradeRoutine = StartCoroutine(UpgradeRoutine(upgradeController));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isUpgrading)
        {
            StopCoroutine(upgradeRoutine);
            isUpgrading = false;
        }
    }

    private IEnumerator UpgradeRoutine(UpgradeController upgradeController)
    {
        isUpgrading = true;
        UpdateText(currentProgress);

        while (true)
        {
            if (!CurrencyManager.Instance.TrySpendCash(1))
                break;

            // Spawn via pool
            GameObject moneyObj = MoneyPoolProvider.GetObject();
            MoneyPrompt prompt = moneyObj.GetComponent<MoneyPrompt>();

            if (prompt != null)
            {
                prompt.Target = targetPosition;
                prompt.AddCash = false;
                prompt.FlyFrom(playerTransform.position);
            }

            currentProgress++;

            if (currentProgress >= upgradesPerCycle)
            {
                upgradeController.UpgradeCharacter();
                currentProgress = 0;
            }

            UpdateText(currentProgress);
            yield return new WaitForSeconds(delayBetweenPayments);
        }

        isUpgrading = false;
    }

    private void UpdateText(int current)
    {
        if (progressText != null)
        {
            progressText.text = $"{current} / {upgradesPerCycle}";
        }
    }
}
