using System;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Start()
    {
        if (GoldBank.Instance != null)
        {
            GoldBank.Instance.OnGoldChanged += UpdateGoldDisplay;
            UpdateGoldDisplay(GoldBank.Instance.currentGold); // Visa initialt värde
        }
    }

    void OnDestroy()
    {
        if (GoldBank.Instance != null)
            GoldBank.Instance.OnGoldChanged -= UpdateGoldDisplay;
    }

    void UpdateGoldDisplay(int newGold)
    {
        if (goldText != null)
            goldText.text = newGold + " Gold";
    }
}
