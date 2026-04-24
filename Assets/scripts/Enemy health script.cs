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

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerGold gold = player.GetComponent<PlayerGold>();

            if (gold != null)
            {
                int reward = Random.Range(5, 15);
                gold.AddGold(reward);

                Debug.Log("Zombie killed ? +" + reward + " gold");
            }
        }

        Animator anim = GetComponent<Animator>();
        if (anim) anim.SetTrigger("Die");

        var ai = GetComponent<ZombieController>();
        if (ai) ai.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent) agent.enabled = false;

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
