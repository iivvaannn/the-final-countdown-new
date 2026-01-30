using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class zombiecontroller : MonoBehaviour
{
    public Animator anim;
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    public float attackDelay = 0.5f;

    [Header("Audio")]
    public AudioClip idleGroan;
    public AudioClip attackSound;
    public AudioClip deathSound;

    private AudioSource audioSource;
    private NavMeshAgent agent;
    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 2f;
            audioSource.maxDistance = 25f;
        }

        StartCoroutine(PlayIdleGroans());
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!isAttacking && dist > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("isMove", true);
        }
        else if (!isAttacking && dist <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;
        anim.SetBool("isMove", false);
        anim.SetTrigger("Attack");

        PlayAttackSound();

        yield return new WaitForSeconds(attackDelay);

        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(attackDamage);
                Debug.Log($"Zombie attacked! Player health now: {ph.CurrentHealth}");
            }
        }

        yield return new WaitForSeconds(attackCooldown - attackDelay);
        isAttacking = false;
    }

    IEnumerator PlayIdleGroans()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6f, 14f));
            if (idleGroan != null)
                audioSource.PlayOneShot(idleGroan);
        }
    }

    void PlayAttackSound()
    {
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }
}
