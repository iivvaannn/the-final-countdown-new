using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject mapPanel;

    bool open;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            open = !open;
            mapPanel.SetActive(open);

            Cursor.lockState = open
                ? CursorLockMode.None
                : CursorLockMode.Locked;

            Cursor.visible = open;
        }
    }
}