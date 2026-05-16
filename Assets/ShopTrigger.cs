using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool playerNear = false;
    private ShopUIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<ShopUIManager>(true); // ?? hittar även inactive

        if (uiManager == null)
            Debug.LogError("ShopUIManager hittades inte i scenen!");
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            uiManager.OpenDialogue();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNear = true;

        uiManager.ShowPrompt();

        Debug.Log("FORCE SHOW UI");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerNear = false;

        uiManager.HidePrompt();
    }
}