using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;
    public int zombieCount = 10;

    private void Start()
    {
        for (int i = 0; i < zombieCount; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.GetComponent<ZombieFollow>().player = player;
        }
    }
}
