using UnityEngine;
using TMPro;

public class Medkit : MonoBehaviour
{
    [Header("Medkits")]
    public int medkitCount = 0;

    [Header("UI")]
    public TMP_Text medkitText;

    [Header("Healing")]
    public int healAmount = 75;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseMedkit();
        }
    }

    public void AddMedkits(int amount)
    {
        medkitCount += amount;

        UpdateUI();
    }

    public void UseMedkit()
    {
        if (medkitCount <= 0)
            return;

        if (playerHealth == null)
            return;

        // FULL HEALTH
        if (playerHealth.currentHealth >= playerHealth.maxHealth)
        {
            Debug.Log("Health already full");
            return;
        }

        medkitCount--;

        playerHealth.Heal(healAmount);

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (medkitText != null)
        {
            medkitText.text = medkitCount.ToString();
        }
    }
}