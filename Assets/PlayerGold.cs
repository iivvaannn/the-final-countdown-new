using UnityEngine;
using TMPro;

public class PlayerGold : MonoBehaviour
{
    public int gold = 100;
    public TMP_Text goldText;

    void Update()
    {
        if (goldText != null)
            goldText.text = gold + " $";
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            return true;
        }

        return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}