using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class zombiespawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;
    public int zombieCount = 5;
    public float spawnDistance = 50f;

    void Start()
    {
        for (int i = 0; i < zombieCount; i++)
        {
            Vector2 circle = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPos = new Vector3(player.position.x + circle.x, player.position.y, player.position.z + circle.y);

            GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
            zombiecontroller zCtrl = zombie.GetComponent<zombiecontroller>();
            if (zCtrl != null) zCtrl.player = player;
        }
    }
}

