using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class KnifeSwing : MonoBehaviour
{
    [Header("Swing")]
    public float swingSpeed = 9f;
    public float attackCooldown = 0.55f;

    [Header("Stamina")]
    public float staminaCost = 12f;

    [Header("Damage")]
    public float knifeRange = 2f;
    public float knifeDamage = 35f;

    [Header("Effects")]
    public GameObject bloodPrefab;

    public Camera playerCamera;

    [Header("Sound")]
    public AudioClip leftSlashSound;
    public AudioClip rightSlashSound;
    public AudioClip stabSound;

    [Range(0f, 1f)]
    public float slashVolume = 0.7f;

    // ADD THIS
    public AudioMixerGroup mixerGroup;

    private bool swinging = false;

    private Quaternion startRot;
    private Vector3 startPos;

    private AudioSource audioSource;

    private int combo = 0;

    private Movement movement;

    void Start()
    {
        startRot = transform.localRotation;
        startPos = transform.localPosition;

        movement = FindObjectOfType<Movement>();

        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // ADD THIS
        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;

        if (!playerCamera)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !swinging)
        {
            if (movement == null)
                return;

            if (movement.UseStamina(staminaCost))
            {
                StartCoroutine(Swing());
            }
        }
    }

    IEnumerator Swing()
    {
        swinging = true;

        combo++;

        if (combo > 2)
            combo = 0;

        Quaternion attackRot = startRot;

        Vector3 attackPos = startPos;

        AudioClip targetSound = null;

        // LEFT TO RIGHT SLASH
        if (combo == 0)
        {
            attackRot =
                startRot *
                Quaternion.Euler(
                    -45f,
                    35f,
                    -35f
                );

            attackPos =
                startPos +
                new Vector3(
                    0.08f,
                    -0.04f,
                    0.12f
                );

            targetSound = leftSlashSound;
        }

        // RIGHT TO LEFT SLASH
        if (combo == 1)
        {
            attackRot =
                startRot *
                Quaternion.Euler(
                    -45f,
                    -35f,
                    35f
                );

            attackPos =
                startPos +
                new Vector3(
                    -0.08f,
                    -0.04f,
                    0.12f
                );

            targetSound = rightSlashSound;
        }

        // STAB
        if (combo == 2)
        {
            attackRot =
                startRot *
                Quaternion.Euler(
                    -15f,
                    0f,
                    0f
                );

            attackPos =
                startPos +
                new Vector3(
                    0f,
                    -0.02f,
                    0.2f
                );

            targetSound = stabSound;
        }

        // PLAY SOUND
        if (targetSound != null)
        {
            audioSource.pitch =
                Random.Range(0.97f, 1.03f);

            audioSource.PlayOneShot(
                targetSound,
                slashVolume
            );
        }

        // DELAY SO HIT MATCHES ANIMATION
        yield return new WaitForSeconds(0.08f);

        // HIT DETECTION
        RaycastHit hit;

        if (Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            out hit,
            knifeRange))
        {
            Enemyhealthscript enemy =
                hit.transform.GetComponentInParent<Enemyhealthscript>();

            if (enemy)
            {
                enemy.takeDamage(knifeDamage);

                // BLOOD EFFECT
                if (bloodPrefab)
                {
                    Instantiate(
                        bloodPrefab,
                        hit.point + hit.normal * 0.02f,
                        Quaternion.LookRotation(hit.normal)
                    );
                }
            }
        }

        float t = 0f;

        // ATTACK
        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;

            transform.localRotation =
                Quaternion.Slerp(
                    startRot,
                    attackRot,
                    t
                );

            transform.localPosition =
                Vector3.Lerp(
                    startPos,
                    attackPos,
                    t
                );

            yield return null;
        }

        t = 0f;

        // RETURN
        while (t < 1f)
        {
            t += Time.deltaTime * (swingSpeed * 0.65f);

            transform.localRotation =
                Quaternion.Slerp(
                    attackRot,
                    startRot,
                    t
                );

            transform.localPosition =
                Vector3.Lerp(
                    attackPos,
                    startPos,
                    t
                );

            yield return null;
        }

        yield return new WaitForSeconds(attackCooldown);

        swinging = false;
    }
}