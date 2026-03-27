using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string itemName;
    public GameObject itemButton; // UI item (för inventory)

    private InventoryController inventory;
    private WeaponSystem weaponSystem;

    private bool canPickup = true;

    void Start()
    {
        inventory = FindObjectOfType<InventoryController>();
        weaponSystem = FindObjectOfType<WeaponSystem>();
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("TRIGGER FUNKAR");

        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V TRYCKT");
            TryPickup();
        }
    }

    void TryPickup()
    {
        if (inventory == null) return;

        // ---------------- ADD TO INVENTORY ----------------
        Debug.Log("Pickup pressed!");

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Debug.Log("Checking slot: " + i);

            Slot slot = inventory.slots[i].GetComponent<Slot>();

            if (!slot.isFull)
            {
                Debug.Log("FOUND EMPTY SLOT!");

                GameObject item = Instantiate(itemButton, inventory.slots[i].transform, false);

                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                item.transform.SetAsLastSibling();

                slot.isFull = true;
                slot.currentItem = item;
                slot.itemName = itemName;

                Destroy(gameObject);
                return;
            }
        
    
        }

        // ---------------- UNLOCK WEAPON ----------------
        if (weaponSystem != null)
        {
            if (itemName.Contains("AK"))
                weaponSystem.hasAK = true;

            else if (itemName.Contains("Shotgun"))
                weaponSystem.hasShotgun = true;

            else if (itemName.Contains("Handgun"))
                weaponSystem.hasHandgun = true;

            else if (itemName.Contains("Sniper"))
                weaponSystem.hasSniper = true;
        }

        // Ta bort object i världen
        gameObject.SetActive(false);
    }
}