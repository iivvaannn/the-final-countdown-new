using UnityEngine;
using TMPro;

public class DeathDayText : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Death text running");

        TMP_Text text = GetComponent<TMP_Text>();

        text.text =
            "Survived Until Day " +
            GameStats.survivedDays;
    }
}