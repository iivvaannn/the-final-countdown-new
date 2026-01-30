using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;

    [SerializeField] Animator animator;

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;

    // ---------------- STAMINA ----------------
    [Header("Stamina")]
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currentStamina = 100f;
    [SerializeField] float sprintStaminaDrain = 25f;
    [SerializeField] float jumpStaminaCost = 20f;
    [SerializeField] float staminaRegen = 20f;
    [SerializeField] float regenDelay = 1.5f;

    [Header("Stamina Lock")]
    [SerializeField] float sprintCooldown = 2f;

    [Header("Stamina UI")]
    [SerializeField] Slider staminaSlider;

    float lastSprintTime;
    bool staminaLocked;
    float staminaLockTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(
            currentMouseDelta,
            targetMouseDelta,
            ref currentMouseDeltaVelocity,
            mouseSmoothTime
        );

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * Time.deltaTime;

        // --------- STAMINA SPRINT LOGIC ----------
        float realSpeed = Speed;

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            lastSprintTime = Time.time;
        }

        bool wantsSprint = Input.GetKey(KeyCode.LeftShift);
        bool canSprint = !staminaLocked && currentStamina > 0f;
        bool isSprinting = wantsSprint && canSprint;

        if (isSprinting)
        {
            realSpeed = Speed * 1.6f;
            currentStamina -= sprintStaminaDrain * Time.deltaTime;
            lastSprintTime = Time.time;

            if (currentStamina <= 0f)
            {
                staminaLocked = true;
                staminaLockTime = Time.time;
                currentStamina = 0f;
            }
        }

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * realSpeed
                         + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        // --------- GROUNDED CHECK AFTER MOVE ----------
        isGrounded = controller.isGrounded;
        if (isGrounded && Input.GetButtonDown("Jump") && !staminaLocked && currentStamina > 0f)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);

            currentStamina -= jumpStaminaCost;

            if (currentStamina <= 0f)
            {
                staminaLocked = true;
                staminaLockTime = Time.time;
                currentStamina = 0f;
            }
        }

        // --------- SNAP TO GROUND ----------
        if (isGrounded && velocityY < 0f)
        {
            velocityY = -8f;
        }

        // --------- STAMINA REGEN + COOLDOWN ----------
        if (staminaLocked)
        {
            if (Time.time >= staminaLockTime + sprintCooldown)
            {
                staminaLocked = false;
                lastSprintTime = Time.time;
            }
        }
        else
        {
            if (!Input.GetKey(KeyCode.LeftShift) && Time.time > lastSprintTime + regenDelay)
            {
                currentStamina += staminaRegen * Time.deltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }

        // --------- ANIMATION ----------
        Vector3 flatVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float currentSpeed = flatVelocity.magnitude;

        if (currentSpeed < 0.1f)
        {
            animator.SetFloat("Speed", 0f);
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("Speed", 0.5f);
        }
        else
        {
            animator.SetFloat("Speed", 1f);
        }
    }
}