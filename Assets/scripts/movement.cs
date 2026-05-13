using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public bool IsSprinting { get; private set; }

    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;

    // CHANGED
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.02f;

    // CHANGED
    [SerializeField] float gravity = -20f;

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

    // ---------------- HEAD BOB ----------------
    [Header("Head Bob")]
    [SerializeField] float walkBobSpeed = 8f;
    [SerializeField] float runBobSpeed = 14f;
    [SerializeField] float walkBobAmount = 0.025f;
    [SerializeField] float runBobAmount = 0.05f;

    float bobTimer;
    Vector3 cameraStartPos;

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

    // ---------------- FOOTSTEPS ----------------
    [Header("Footsteps")]
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioClip[] walkSteps;
    [SerializeField] AudioClip[] runSteps;

    [SerializeField] float walkStepTime = 0.55f;
    [SerializeField] float runStepTime = 0.32f;

    float stepTimer;

    // ---------------- BREATHING ----------------
    [Header("Breathing")]
    [SerializeField] AudioSource breathingSource;
    [SerializeField] AudioClip lightBreathing;
    [SerializeField] AudioClip heavyBreathing;
    [SerializeField] float heavyBreathThreshold = 30f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        cameraStartPos = playerCamera.localPosition;

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
        Vector2 targetMouseDelta =
            new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(
            currentMouseDelta,
            targetMouseDelta,
            ref currentMouseDeltaVelocity,
            mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        Vector2 targetDir =
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(
            currentDir,
            targetDir,
            ref currentDirVelocity,
            moveSmoothTime);

        velocityY += gravity * Time.deltaTime;

        float realSpeed = Speed;

        bool wantsSprint = Input.GetKey(KeyCode.LeftShift);
        bool canSprint = !staminaLocked && currentStamina > 0f;

        IsSprinting = wantsSprint && canSprint;

        if (IsSprinting)
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

        Vector3 velocity =
            (transform.forward * currentDir.y +
             transform.right * currentDir.x) * realSpeed
             + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        isGrounded = controller.isGrounded;

        if (isGrounded &&
            Input.GetButtonDown("Jump") &&
            !staminaLocked &&
            currentStamina > 0f)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            currentStamina -= jumpStaminaCost;
        }

        if (isGrounded && velocityY < 0f)
            velocityY = -8f;

        // stamina regen
        if (staminaLocked)
        {
            if (Time.time >= staminaLockTime + sprintCooldown)
            {
                staminaLocked = false;
                lastSprintTime = Time.time;
            }
        }
        else if (!IsSprinting && Time.time > lastSprintTime + regenDelay)
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (staminaSlider)
            staminaSlider.value = currentStamina;

        Vector3 flatVel =
            new Vector3(controller.velocity.x, 0, controller.velocity.z);

        float speed = flatVel.magnitude;

        if (speed < 0.1f)
            animator.SetFloat("Speed", 0f);
        else if (!IsSprinting)
            animator.SetFloat("Speed", 0.5f);
        else
            animator.SetFloat("Speed", 1f);

        // ADDED
        HandleCameraBob();

        HandleFootsteps(speed);
        HandleBreathing();
    }

    // ---------------- HEAD BOB ----------------
    void HandleCameraBob()
    {
        if (!isGrounded)
            return;

        bool moving = currentDir.magnitude > 0.1f;

        if (!moving)
        {
            bobTimer = 0f;

            playerCamera.localPosition =
                Vector3.Lerp(
                    playerCamera.localPosition,
                    cameraStartPos,
                    Time.deltaTime * 8f
                );

            return;
        }

        float speed = IsSprinting ? runBobSpeed : walkBobSpeed;
        float amount = IsSprinting ? runBobAmount : walkBobAmount;

        bobTimer += Time.deltaTime * speed;

        float bobY = Mathf.Sin(bobTimer) * amount;

        Vector3 targetPos =
            cameraStartPos + new Vector3(0f, bobY, 0f);

        playerCamera.localPosition =
            Vector3.Lerp(
                playerCamera.localPosition,
                targetPos,
                Time.deltaTime * 10f
            );
    }

    // ---------------- FOOTSTEPS ----------------
    void HandleFootsteps(float speed)
    {
        if (!isGrounded)
        {
            footstepSource.Stop();
            return;
        }

        bool moving = currentDir.magnitude > 0.1f;

        if (!moving)
        {
            stepTimer = 0f;

            if (footstepSource.isPlaying)
                footstepSource.Stop();

            return;
        }

        stepTimer -= Time.deltaTime;

        float interval =
            IsSprinting ? runStepTime : walkStepTime;

        if (stepTimer <= 0f)
        {
            AudioClip[] clips =
                IsSprinting ? runSteps : walkSteps;

            if (clips.Length > 0)
            {
                int i = Random.Range(0, clips.Length);

                footstepSource.pitch =
                    Random.Range(0.95f, 1.05f);

                // IMPORTANT
                footstepSource.clip = clips[i];
                footstepSource.Play();
            }

            stepTimer = interval;
        }
    }

    // ---------------- BREATHING ----------------
    void HandleBreathing()
    {
        if (!breathingSource)
            return;

        AudioClip target = null;

        if (currentStamina <= heavyBreathThreshold)
        {
            target = heavyBreathing;
        }
        else if (IsSprinting)
        {
            target = lightBreathing;
        }

        if (target == null)
        {
            if (breathingSource.isPlaying)
                breathingSource.Stop();

            return;
        }

        if (breathingSource.clip != target)
        {
            breathingSource.clip = target;
            breathingSource.loop = true;
            breathingSource.Play();
        }
    }
}