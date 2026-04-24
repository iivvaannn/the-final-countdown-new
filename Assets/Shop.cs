using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;

    void Start()
    {
        shopUI.SetActive(false);
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}