using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject zombiePrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnDistance = 60f;
    public int baseRoamers = 6;
    public int baseWaveSize = 10;
    public float waveSpawnDelay = 1.5f;

    [Header("Spawn Safety (Optional)")]
    public bool avoidPlayerVision = true;

    List<GameObject> aliveZombies = new List<GameObject>();

    bool waveActive;

    // ================= EVENTS =================

    void OnEnable()
    {
        LightingManager.OnDayStart += StartDay;
        LightingManager.OnNightStart += StartNight;
    }

    void OnDisable()
    {
        LightingManager.OnDayStart -= StartDay;
        LightingManager.OnNightStart -= StartNight;
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        StartDay();
    }

    void Update()
    {
        // Remove destroyed zombies automatically
        aliveZombies.RemoveAll(z => z == null);

        // Reward player if wave cleared
        if (waveActive && aliveZombies.Count == 0)
        {
            waveActive = false;
            GiveWaveReward();
        }
    }

    // ================= DAY =================

    void StartDay()
    {
        StopAllCoroutines();

        waveActive = false;

        ClearZombies();

        int day = LightingManager.Instance.CurrentDay;
        int roamers = baseRoamers + day * 2;

        Debug.Log("Roaming Zombies: " + roamers);

        for (int i = 0; i < roamers; i++)
            SpawnZombie();
    }

    // ================= NIGHT =================

    void StartNight()
    {
        StopAllCoroutines();
        StartCoroutine(NightWave());
    }

    IEnumerator NightWave()
    {
        waveActive = true;

        int day = LightingManager.Instance.CurrentDay;
        int waveSize = baseWaveSize + day * 5;

        Debug.Log("Night Wave Size: " + waveSize);

        for (int i = 0; i < waveSize; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(waveSpawnDelay);
        }
    }

    // ================= SPAWNING =================

    void SpawnZombie()
    {
        Vector2 circle =
            Random.insideUnitCircle.normalized * spawnDistance;

        Vector3 rayOrigin = new Vector3(
            player.position.x + circle.x,
            player.position.y + 50f,
            player.position.z + circle.y
        );

        RaycastHit hit;

        // Ground alignment
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 200f))
        {
            Vector3 spawnPos = hit.point;

            // OPTIONAL: avoid spawning in front of player
            if (avoidPlayerVision)
            {
                Vector3 dirToSpawn =
                    (spawnPos - player.position).normalized;

                if (Vector3.Dot(player.forward, dirToSpawn) > 0.6f)
                    return;
            }

            GameObject zombie =
                Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

            ZombieController z =
                zombie.GetComponent<ZombieController>();

            if (z != null)
                z.player = player;

            aliveZombies.Add(zombie);
        }
    }

    // ================= CLEANUP =================

    void ClearZombies()
    {
        foreach (GameObject z in aliveZombies)
        {
            if (z != null)
                Destroy(z);
        }

        aliveZombies.Clear();
    }

    // ================= REWARD =================

    void GiveWaveReward()
    {
        Debug.Log("Wave cleared! Reward granted.");

        // Future ideas:
        // give water
        // ammo drop
        // trader spawn
        // upgrade token
    }
}