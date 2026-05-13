using UnityEngine;

public class Speed : MonoBehaviour
{
    public float baseSpeed = 2f;
    public float sprintSpeed = 3.5f;
    public float sprintDistance = 6f;

    public Transform player;

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        float speed = dist < sprintDistance ? sprintSpeed : baseSpeed;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}