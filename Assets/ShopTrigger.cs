using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool playerNear = false;
    private ShopUIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<ShopUIManager>();

        if (uiManager == null)
            Debug.LogError("ShopUIManager hittades inte i scenen!");
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (uiManager != null)
                uiManager.OpenDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNear = true;

        if (uiManager != null)
            uiManager.ShowPrompt();

        Debug.Log("FORCE SHOW UI");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNear = false;

        if (uiManager != null)
            uiManager.CloseAll();
    }
}