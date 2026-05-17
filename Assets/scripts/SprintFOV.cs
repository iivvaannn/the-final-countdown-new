using UnityEngine;

public class SprintFOV : MonoBehaviour
{
    public Camera cam;
    public Movement movement;

    public float sprintFOVIncrease = 15f;
    public float smoothSpeed = 8f;

    float baseFOV;

    void Start()
    {
        // gets the player's chosen FOV from settings
        baseFOV = PlayerPrefs.GetFloat("PlayerFOV", 90f);

        cam.fieldOfView = baseFOV;
    }

    void Update()
    {
        // keeps updating in case player changes FOV in settings live
        baseFOV = PlayerPrefs.GetFloat("PlayerFOV", 90f);

        float targetFOV =
            movement.IsSprinting
            ? baseFOV + sprintFOVIncrease
            : baseFOV;

        cam.fieldOfView =
            Mathf.Lerp(
                cam.fieldOfView,
                targetFOV,
                Time.deltaTime * smoothSpeed
            );
    }
}