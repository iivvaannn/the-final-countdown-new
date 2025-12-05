using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private InventoryController inventory;
    public GameObject itemButton;
    public string itemName;
    private bool pause = false;

    void Start()
    {
        inventory = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (!other.CompareTag("Player")) return;

        if (inventory == null || inventory.slots == null) return;

        if (!Input.GetKeyDown(KeyCode.V) || pause) return;

        pause = true;
        StartCoroutine(PauseCoroutine());

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            GameObject slotGO = inventory.slots[i];

            if (slotGO == null) continue;

            Slot slotComp = slotGO.GetComponent<Slot>();
            if (slotComp == null) continue;

            // Stacka om samma item finns
            if (inventory.isFull[i] && slotComp.amount < slotComp.maxStackSize)
            {
                var existing = slotGO.GetComponentInChildren<Spawn>();
                if (existing != null && existing.itemName == itemName)
                {
                    slotComp.amount++;
                    //Destroy(gameObject);
                    return;
                }
            }

            // Annars placera i tom slot
            if (!inventory.isFull[i])
            {
                inventory.isFull[i] = true;
                GameObject button = Instantiate(itemButton, slotGO.transform, false);

                var spawnComp = button.GetComponent<Spawn>();
                if (spawnComp != null)
                    spawnComp.itemName = itemName;

                slotComp.amount = 1;
                //Destroy(gameObject);
                return;
            }
        }
    }

    IEnumerator PauseCoroutine()
    {
        yield return new WaitForSeconds(1);
        pause = false;
    }
}
