using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image healthFill; // Den fyllda delen av healthbaren

    [Header("Player Health")]
    [SerializeField] private float maxHealth = 100f; // Maxh�lsa
    [SerializeField] private float currentHealth = 100f; // Nuvarande h�lsa

    private void Start()
    {
        // S�tter h�lsan till full i b�rjan
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Funktion f�r att ta skada
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Se till att h�lsan inte g�r under 0
        UpdateHealthUI();

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Optionally: stop the game
        Time.timeScale = 0f; // freezes the game
        // Or trigger your Game Over screen
    }

    // Funktion f�r att l�ka
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Se till att h�lsan inte g�r �ver max
        UpdateHealthUI();
    }

    // Uppdaterar UI:t
    private void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
    }

    // Om du vill kan du l�gga till en funktion f�r att s�tta h�lsan direkt
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    // Public getter to read current health
    public float CurrentHealth
    {
        get { return currentHealth; }
    }

}

