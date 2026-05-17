using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathUI;



    public void ShowDeathScreen()
    {
        deathUI.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("True scene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("main menu");
    }
}