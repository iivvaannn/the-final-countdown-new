using UnityEngine;
using TMPro;

public class DayUI : MonoBehaviour
{
    public TMP_Text dayText;

    void OnEnable()
    {
        LightingManager.OnDayStart += UpdateDay;
    }

    void OnDisable()
    {
        LightingManager.OnDayStart -= UpdateDay;
    }

    void Start()
    {
        UpdateDay();
    }

    void UpdateDay()
    {
        int day = LightingManager.Instance.CurrentDay;
        dayText.text = "DAY " + day;
    }
}