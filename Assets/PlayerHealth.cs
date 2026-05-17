using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Hit Sound")]
    public AudioClip hurtSound;
    [Range(0f, 1f)] public float hurtVolume = 0.7f;

    private AudioSource audioSource;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Health")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public float currentHealth;
    [Header("Blood Overlay")]
    [SerializeField] private Image bloodOverlay;
    [SerializeField] private float bloodFadeSpeed = 2f;
    [SerializeField] private float bloodIntensityPerHit = 0.25f;

    float targetBloodAlpha = 0f;

    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthBar.SetMaxHealth((int)maxHealth);

        // ✅ Audio setup
        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryHeal();
        }
        UpdateBloodOverlay();
    }
    void UpdateBloodOverlay()
    {
        if (!bloodOverlay) return;

        // GORE OFF
        if (!AccessibilitySettings.goreEnabled)
        {
            Color hidden = bloodOverlay.color;
            hidden.a = 0f;
            bloodOverlay.color = hidden;
            return;
        }

        Color c = bloodOverlay.color;

        c.a = Mathf.Lerp(
            c.a,
            targetBloodAlpha,
            Time.deltaTime * bloodFadeSpeed
        );

        bloodOverlay.color = c;

        targetBloodAlpha =
            Mathf.Clamp01(targetBloodAlpha - Time.deltaTime * 0.15f);
    }
    void TryHeal()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            3f))
        {
            if (hit.transform.CompareTag("Medkit"))
            {
                Medkit medkit = hit.transform.GetComponent<Medkit>();

                if (medkit != null)
                {
                    medkit.UseMedkit();
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.SetHealth((int)currentHealth);
        healthSlider.value = currentHealth;
        // Blood overlay increase
        targetBloodAlpha += bloodIntensityPerHit;
        targetBloodAlpha = Mathf.Clamp01(targetBloodAlpha);

        // ✅ play hurt sound
        if (hurtSound)
            Debug.Log("Hurt sound played");
        audioSource.PlayOneShot(hurtSound, hurtVolume);

        // ✅ small camera hit reaction
        if (Camera.main)
        {
            Camera.main.transform.localRotation *= Quaternion.Euler(
                Random.Range(-2f, -1f),
                Random.Range(-1f, 1f),
                0f
            );
        }

        if (currentHealth <= 0)
        {
            GameStats.survivedDays =
                LightingManager.Instance.CurrentDay;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex + 1);
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