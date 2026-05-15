using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject dialoguePanel;
    public GameObject shopPanel;


    void Start()
    {
        if (pressEText != null) pressEText.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    public void ShowPrompt()
    {
        if (pressEText != null) pressEText.SetActive(true);
    }

    public void HidePrompt()
    {
        if (pressEText != null) pressEText.SetActive(false);
    }

    public void OpenDialogue()
    {
        if (pressEText != null) pressEText.SetActive(false);

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (shopPanel != null) shopPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OpenShop()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(true);
    }

    public void CloseAll()
    {
        if (pressEText != null) pressEText.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);

    }

    public void CloseAllUI()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}