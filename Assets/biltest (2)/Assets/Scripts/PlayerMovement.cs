using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the movement

    void Update()
    {
        // Get input from the user
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow

        // Create a movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Move the capsule
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}