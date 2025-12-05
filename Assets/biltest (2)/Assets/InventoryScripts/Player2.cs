using TMPro;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int Gold;
    public TextMeshProUGUI GoldAmountText;

    void Start()
    {
        Gold = 20;
        Debug.Log("Initial Gold: " + Gold);

        if (GoldAmountText != null)
        {
            GoldAmountText.text = Gold + " Gold";
        }
    }

    void Update()
    {
        GoldAmountText.text = Gold + " Gold";
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        Debug.Log("Added Gold: " + amount + ", Total Gold: " + Gold);
    }
}
