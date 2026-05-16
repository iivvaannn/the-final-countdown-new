using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject dialoguePanel;
    public GameObject shopPanel;

    void Start()
    {
        pressEText.SetActive(false);
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void ShowPrompt()
    {
        if (pressEText == null)
        {
            Debug.LogError("pressEText är NULL");
            return;
        }

        pressEText.SetActive(true);
    }

    public void HidePrompt()
    {
        if (pressEText != null)
            pressEText.SetActive(false);
    }

    public void OpenDialogue()
    {
        pressEText.SetActive(false);

        dialoguePanel.SetActive(true);
        shopPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenShop()
    {
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void CloseAllUI()
    {
        pressEText.SetActive(false);
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}