using UnityEngine;

public class AccessibilitySettings : MonoBehaviour
{
    public static bool goreEnabled = true;

    public void ToggleGore(bool enabled)
    {
        goreEnabled = enabled;

        Debug.Log("Gore Enabled: " + enabled);
    }
}