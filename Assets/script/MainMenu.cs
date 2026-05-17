using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject mainButtons;

    void Start()
    {
        optionsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);

        mainButtons.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);

        mainButtons.SetActive(true);
    }
}