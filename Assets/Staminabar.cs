using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider staminaSlider;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float drainRate = 20f;
    [SerializeField] private float regenRate = 15f;
    [SerializeField] private float regenDelay = 1f;
    [SerializeField] private float jumpCost = 25f;

    private float regenTimer;
    private bool isSprinting;

    private void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    private void Update()
    {
        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= drainRate * Time.deltaTime;
            regenTimer = 0f;
        }
        else
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenDelay && currentStamina < maxStamina)
            {
                currentStamina += regenRate * Time.deltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaSlider.value = currentStamina;
    }

    public void StartSprinting()
    {
        isSprinting = true;
    }

    public void StopSprinting()
    {
        isSprinting = false;
    }

    public bool HasStamina()
    {
        return currentStamina > 0;
    }

    public bool CanJump()
    {
        return currentStamina >= jumpCost;
    }

    public void UseJumpStamina()
    {
        currentStamina -= jumpCost;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaSlider.value = currentStamina;
    }
}