using UnityEngine;

public class ZombieFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float spawnDistance = 100f;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Spawn zombie at random position around the player (spawnDistance away)
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPos = new Vector3(
            player.position.x + randomCircle.x,
            player.position.y,
            player.position.z + randomCircle.y
        );

        transform.position = spawnPos;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
}
