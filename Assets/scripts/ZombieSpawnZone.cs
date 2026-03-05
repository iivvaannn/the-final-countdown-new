using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawnZone : MonoBehaviour
{
    [Header("Zone Settings")]
    public GameObject zombiePrefab;
    public int maxZombies = 15;
    public float radius = 120f;

    List<GameObject> zombies = new List<GameObject>();

    void Start()
    {
        PopulateZone();
    }

    void PopulateZone()
    {
        for (int i = 0; i < maxZombies; i++)
            SpawnZombie();
    }

    void SpawnZombie()
    {
        Vector2 circle = Random.insideUnitCircle * radius;

        Vector3 spawnPos =
            transform.position +
            new Vector3(circle.x, 100f, circle.y);

        RaycastHit hit;

        // drop zombie onto terrain
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, 200f))
        {
            GameObject zombie =
                Instantiate(zombiePrefab, hit.point, Quaternion.identity);

            zombies.Add(zombie);
        }
    }
}