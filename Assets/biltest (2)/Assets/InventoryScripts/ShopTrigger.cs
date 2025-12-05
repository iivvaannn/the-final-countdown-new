using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;       // Själva shoppen (Canvas/Panel)
    public GameObject pressEUI;      // "Press E to open shop"-UI

    private bool isPlayerInZone = false;

    private void Start()
    {
        shopUI.SetActive(false);
        pressEUI.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenShop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            pressEUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            pressEUI.SetActive(false);
            shopUI.SetActive(false);
        }
    }

    void OpenShop()
    {
        shopUI.SetActive(true);
        pressEUI.SetActive(false);
    }
}
