using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class zombiecontroller : MonoBehaviour
{
    public Animator anim;            // Zombie Animator
    public Transform player;         // Spelaren
    public float attackRange = 2f;   // N�r attack ska b�rja
    public float attackCooldown = 2f;// Tid mellan attacker
    public float attackDamage = 10f; // Skada per attack
    public float attackDelay = 0.5f; // N�r i animation skadan sker

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
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!isAttacking && dist > attackRange)
        {
            // F�lj spelaren med NavMeshAgent
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("isMove", true);
        }
        else if (!isAttacking && dist <= attackRange)
        {
            // Attackera
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;           // Stoppa agent under attack
        anim.SetBool("isMove", false);    // Stoppa Walk animation
        anim.SetTrigger("Attack");        // Spela Attack animation

        // V�nta tills attacken ska tr�ffa
        yield return new WaitForSeconds(attackDelay);

        // Applicera skada p� spelaren
        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(attackDamage);
                Debug.Log($"Zombie attacked! Player health now: {ph.CurrentHealth}");
            }
        }

        // V�nta cooldown innan n�sta attack
        yield return new WaitForSeconds(attackCooldown - attackDelay);
        isAttacking = false;
    }
}
