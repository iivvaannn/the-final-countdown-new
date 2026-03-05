using UnityEngine;

public class SprintFOV : MonoBehaviour
{
    public Camera cam;
    public Movement movement;

    public float normalFOV = 75f;
    public float sprintFOV = 90f;
    public float smoothSpeed = 8f;

    void Update()
    {
        float targetFOV =
            movement.IsSprinting ? sprintFOV : normalFOV;

        cam.fieldOfView =
            Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}