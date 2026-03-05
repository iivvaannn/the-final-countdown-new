using UnityEngine;

public class Enemyhealthscript : MonoBehaviour
{
    public float health = 100f;
    bool isDead = false;

    public void takeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0f)
            die();
    }

    void die()
    {
        isDead = true;

        Animator anim = GetComponent<Animator>();
        if (anim) anim.SetTrigger("Die");

        // stop movement script
        var ai = GetComponent<ZombieController>();
        if (ai) ai.enabled = false;

        // stop navmesh if used
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent) agent.enabled = false;

        // kill rigidbody physics
        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Destroy(gameObject, 4f);
    }
}
