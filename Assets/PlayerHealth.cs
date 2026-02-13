using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [SerializeField] private HealthBar healthBar;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryHeal();
        }
    }

    void TryHeal()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f))
        {
            if (hit.transform.CompareTag("Medkit"))
            {
                medkit medkit = hit.transform.GetComponent<medkit>();

                if (medkit != null)
                {
                    medkit.HealPlayer(this);
                }
            }
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthBar.SetMaxHealth((int)maxHealth);

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth((int)currentHealth);


        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth((int)currentHealth);


        healthSlider.value = currentHealth;
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }
}