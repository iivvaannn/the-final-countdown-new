using UnityEngine;

public class ShopDialogue : MonoBehaviour
{
    public GameObject dialogueUI;
    public Shop shop;

    public void OpenDialogue()
    {
        dialogueUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogue()
    {
        dialogueUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Buy()
    {
        dialogueUI.SetActive(false);
        shop.OpenShop();
    }

    public void Quit()
    {
        CloseDialogue();
    }
}