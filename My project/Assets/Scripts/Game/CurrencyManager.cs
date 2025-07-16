using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    public int CurrentCash { get; private set; }

    public static Action<int> cashChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // 

        CurrentCash = PlayerPrefs.GetInt("Cash", 0);
    }

    public void AddCash(int amount)
    {
        CurrentCash += Mathf.Max(0, amount);
        cashChanged?.Invoke(CurrentCash);
        // PlayerPrefs.SetInt("Cash", CurrentCash);
    }

    public bool TrySpendCash(int amount)
    {
        if (CurrentCash >= amount)
        {
            CurrentCash -= amount;
            // PlayerPrefs.SetInt("Cash", CurrentCash);
            cashChanged?.Invoke(CurrentCash);
            return true;
        }

        return false;
    }

    public bool HasEnoughCash(int amount)
    {
        return CurrentCash >= amount;
    }
}
