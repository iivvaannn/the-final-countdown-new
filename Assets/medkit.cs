using UnityEngine;

public class medkit : MonoBehaviour
{
    public float healAmount = 5f;      // heal per användning
    public int uses = 5;               // hur många gånger den kan användas

    public void HealPlayer(PlayerHealth playerHealth)
    {
        if (uses <= 0) return;

        playerHealth.Heal(healAmount);
        uses--;

        if (uses <= 0)
        {
            Destroy(gameObject); // först nu försvinner den
        }
    }
}
