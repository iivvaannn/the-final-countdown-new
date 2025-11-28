using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] Animator animator;   // <-- LAGT TILL (behövs för animationer)

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
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

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;

        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        // --- SPRINT SPEED (endast tillägg, ändrar inget annat) ---
        float realSpeed = Speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            realSpeed = Speed * 1.6f;
        }

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * realSpeed + Vector3.up * velocityY;

        //animator.SetFloat("Speed") == Speed;

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }

        // -------------------------------------------------------
        // ----------------- ANIMATION SYSTEM --------------------
        // -------------------------------------------------------

        // Beräknar horizontal rörelse
        Vector3 flatVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float currentSpeed = flatVelocity.magnitude;

        if (currentSpeed < 0.1f)
        {
            animator.SetFloat("Speed", 0f);        // Idle
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("Speed", 0.5f);      // Walk
        }
        else
        {
            animator.SetFloat("Speed", 1f);        // Run
        }
    }
}
