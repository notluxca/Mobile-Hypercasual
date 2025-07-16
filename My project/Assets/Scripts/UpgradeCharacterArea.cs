using UnityEngine;
using System.Collections;
using TMPro; // para texto UI

public class UpgradeCharacterArea : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public int upgradesPerCycle = 5;
    public float delayBetweenPayments = 0.3f;
    public GameObject moneyPrompt;
    public TextMeshProUGUI progressText;

    private Coroutine upgradeRoutine;
    private bool isUpgrading = false;

    public Transform playerTransform;
    public Transform targetPosition;

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
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
        int progress = 0;

        UpdateText(progress);

        while (true)
        {
            // tenta gastar 1 de dinheiro
            if (!CurrencyManager.Instance.TrySpendCash(1))
            {
                break; // sem dinheiro
            }

            MoneyPrompt _moneyPrompt = Instantiate(moneyPrompt, playerTransform.position, Quaternion.identity).GetComponent<MoneyPrompt>(); //fix memory management
            _moneyPrompt.Target = targetPosition;
            _moneyPrompt.AddCash = false;

            progress++;
            if (progress >= upgradesPerCycle)
            {
                upgradeController.UpgradeCharacter(1);
                progress = 0;
            }

            UpdateText(progress);
            yield return new WaitForSeconds(delayBetweenPayments);
        }

        isUpgrading = false;
    }

    private void UpdateText(int currentProgress)
    {
        if (progressText != null)
        {
            progressText.text = $"{Mathf.Max(currentProgress, 1)} / {upgradesPerCycle}";
        }
    }
}
