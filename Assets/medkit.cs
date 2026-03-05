using UnityEngine;

public class Medkit : MonoBehaviour
{
    public float healAmount = 25f;

    public void HealPlayer(PlayerHealth playerHealth)
    {
        playerHealth.Heal(healAmount);

        Destroy(gameObject);
    }
}