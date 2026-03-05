using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public Transform player;

    NavMeshAgent agent;
    AudioSource audioSource;

    // ================= DETECTION =================

    [Header("Detection")]
    public float aggroDistance = 40f;
    public float loseDistance = 50f;
    public float roamRadius = 12f;
    public float groupAggroRadius = 20f;

    bool hasAggro;

    // ================= COMBAT =================

    [Header("Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    public float attackDelay = 0.4f;

    bool isAttacking;
    bool isDead;

    // ================= MOVEMENT =================

    [Header("Movement")]
    public float updateRate = 0.25f;
    float nextPathUpdate;

    Vector3 roamTarget;
    bool hasRoamTarget;

    // ================= AUDIO =================

    [Header("Audio")]
    public AudioClip idleGroan;
    public AudioClip attackSound;
    public AudioClip deathSound;

    // =====================================================

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;

        agent.stoppingDistance = attackRange - 0.2f;
        agent.updateRotation = false;

        StartCoroutine(IdleGroans());
    }

    void Update()
    {
        if (isDead || player == null)
            return;

        HandleAggro();
        HandleBehaviour();
        RotateTowardsMovement();
        UpdateAnimation();
    }

    // =====================================================
    // AGGRO SYSTEM + GROUP ALERT
    // =====================================================

    void HandleAggro()
    {
        float distance =
            Vector3.Distance(transform.position, player.position);

        if (!hasAggro)
        {
            if (distance <= 6f)
            {
                SetAggro();
                return;
            }

            if (distance <= aggroDistance && CanSeePlayer())
            {
                SetAggro();
            }
        }
        else
        {
            if (distance > loseDistance)
            {
                hasAggro = false;
            }
        }
    }

    void SetAggro()
    {
        if (hasAggro) return;

        hasAggro = true;
        AlertNearbyZombies();
    }

    void AlertNearbyZombies()
    {
        Collider[] hits =
            Physics.OverlapSphere(transform.position, groupAggroRadius);

        foreach (Collider hit in hits)
        {
            ZombieController z =
                hit.GetComponent<ZombieController>();

            if (z != null && !z.hasAggro)
            {
                z.hasAggro = true;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 direction = player.position - origin;

        float distance = direction.magnitude;
        direction.Normalize();

        RaycastHit hit;

        if (Physics.SphereCast(origin, 0.6f, direction, out hit, aggroDistance))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // =====================================================
    // BEHAVIOUR
    // =====================================================

    void HandleBehaviour()
    {
        float distance =
            Vector3.Distance(transform.position, player.position);

        if (hasAggro)
        {
            if (distance <= attackRange && !isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
            else if (!isAttacking)
            {
                MoveToPlayer();
            }
        }
        else
        {
            Roam();
        }
    }

    void MoveToPlayer()
    {
        agent.isStopped = false;

        if (Time.time >= nextPathUpdate)
        {
            agent.SetDestination(player.position);
            nextPathUpdate = Time.time + updateRate;
        }
    }

    void Roam()
    {
        agent.isStopped = false;

        if (!hasRoamTarget || agent.remainingDistance < 1f)
        {
            Vector3 randomPoint =
                transform.position + Random.insideUnitSphere * roamRadius;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, roamRadius, NavMesh.AllAreas))
            {
                roamTarget = hit.position;
                hasRoamTarget = true;
                agent.SetDestination(roamTarget);
            }
        }
    }

    void RotateTowardsMovement()
    {
        Vector3 dir = agent.velocity;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.1f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation =
            Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 6f);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        agent.isStopped = true;

        anim.SetFloat("Speed", 0f);
        anim.SetTrigger("Attack");

        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);

        yield return new WaitForSeconds(attackDelay);

        if (player != null)
        {
            float dist =
                Vector3.Distance(transform.position, player.position);

            if (dist <= attackRange + 1f)
            {
                PlayerHealth ph =
                    player.GetComponent<PlayerHealth>();

                if (ph != null)
                    ph.TakeDamage(attackDamage);
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);
    }

    IEnumerator IdleGroans()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(Random.Range(6f, 14f));

            if (!hasAggro && idleGroan != null)
                audioSource.PlayOneShot(idleGroan);
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        StopAllCoroutines();

        agent.isStopped = true;
        agent.enabled = false;

        anim.SetFloat("Speed", 0f);
        anim.SetTrigger("Die");

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        Destroy(gameObject, 5f);
    }
}