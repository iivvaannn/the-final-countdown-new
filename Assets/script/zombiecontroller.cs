using System.Collections;
using UnityEngine;

public class zombiecontroller : MonoBehaviour
{
    public Animator anim;
    public Transform player;
    public float moveSpeed = 1.5f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;   // damage per attack
    public float attackDelay = 0.5f;   // time before applying damage to match animation

    private bool isAttacking = false;

    void Start()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        anim.SetBool("isMove", false); // start idle
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!isAttacking && dist > attackRange)
        {
            // Walk toward player
            Vector3 dir = (player.position - transform.position).normalized;
            dir.y = 0f;
            transform.position += dir * moveSpeed * Time.deltaTime;

            // Rotate toward player
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 8f * Time.deltaTime);
            }

            anim.SetBool("isMove", true); // play Walk
        }
        else if (!isAttacking && dist <= attackRange)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        anim.SetBool("isMove", false);   // stop walking
        anim.SetTrigger("Attack");       // play Attack animation

        // Wait until approx. the "hit" frame
        yield return new WaitForSeconds(attackDelay);

        // Apply damage
        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(attackDamage);
                Debug.Log($"Zombie attacked! Player health now: {ph.CurrentHealth}");
            }
        }

        // Wait for cooldown before next attack
        yield return new WaitForSeconds(attackCooldown - attackDelay);

        isAttacking = false;
    }
}
