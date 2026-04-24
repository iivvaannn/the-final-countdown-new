using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public ShopDialogue dialogue;

    private bool playerNear = false;

    public GameObject pressEText;
    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (pressEText != null)
                pressEText.SetActive(false);

            dialogue.OpenDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            if (pressEText != null)
                pressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }
}