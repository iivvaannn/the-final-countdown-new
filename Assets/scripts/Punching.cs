using UnityEngine;

public class Punch : MonoBehaviour
{
    [Header("Punch Settings")]
    public float damage = 15f;
    public float range = 2f;
    public float cooldown = 0.6f;

    [Header("References")]
    public Camera fpsCam;
    public Animator armsAnimator;
    public AudioSource audioSource;
    public AudioClip punchSound;

    private float nextPunchTime;

    void Start()
    {
        if (!fpsCam) fpsCam = Camera.main;

        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (Time.time < nextPunchTime) return;

        if (Input.GetMouseButtonDown(0))
        {
            DoPunch();
        }
    }

    void DoPunch()
    {
        nextPunchTime = Time.time + cooldown;

        // play animation
        if (armsAnimator)
            armsAnimator.SetTrigger("Punch");

        // sound
        if (punchSound)
            audioSource.PlayOneShot(punchSound, 0.7f);

        // hit detection
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            var enemy = hit.transform.GetComponent<Enemyhealthscript>();
            if (enemy)
            {
                enemy.takeDamage(damage);
            }

            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * 5f, ForceMode.Impulse);
            }
        }
    }
}