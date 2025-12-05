using System;
using UnityEngine;

public class GoldBank : MonoBehaviour
{
    public static GoldBank Instance;

    public int currentGold = 0;
    public event Action<int> OnGoldChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
    }
}
