using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    public KeyCode toggleKey = KeyCode.F;

    void Start()
    {
        Debug.Log("Flashlight script started.");

        if (flashlight == null)
        {
            Debug.LogError("Flashlight reference is NULL. Drag the Light into the inspector.");
            return;
        }

        flashlight.enabled = false;
        Debug.Log("Flashlight initialized OFF.");
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Debug.Log("Toggle key pressed.");

            if (flashlight == null)
            {
                Debug.LogError("Flashlight reference missing!");
                return;
            }

            flashlight.enabled = !flashlight.enabled;

            Debug.Log("Flashlight state: " + flashlight.enabled);
        }
    }
}