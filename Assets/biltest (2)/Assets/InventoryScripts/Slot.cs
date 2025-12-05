using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    private InventoryController inventory;
    public int i;

    public TextMeshProUGUI amountText;
    public TextMeshProUGUI itemNameText;

    public int amount;
    public string itemName;

    [Tooltip("Max antal av samma item i slotten")]
    public int maxStackSize = 5;

    private Transform playerTransform;

    void Start()
    {
        inventory = FindObjectOfType<InventoryController>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (amountText != null)
            amountText.text = amount > 1 ? amount.ToString() : "";

        if (itemNameText != null)
            itemNameText.text = itemName;

        // Säkerställ att namnet uppdateras korrekt vid 0
        if (amount <= 0)
        {
            inventory.isFull[i] = false;
            itemName = "";
            if (itemNameText != null)
                itemNameText.text = "";
        }
    }

    public void DropItem()
    {
        Spawn spawn = GetComponentInChildren<Spawn>();
        if (spawn == null || playerTransform == null) return;

        amount--;

        GameObject dropPrefab = inventory.GetDropPrefab(spawn.itemName);
        if (dropPrefab != null)
        {
            Vector3 dropPosition = playerTransform.position + playerTransform.forward * 2f;
            GameObject droppedObj = Instantiate(dropPrefab, dropPosition, Quaternion.identity);

            var pickup = droppedObj.GetComponent<Pickup>();
            if (pickup != null)
            {
                pickup.enabled = false;
                StartCoroutine(EnablePickupAfterDelay(pickup, 1f));
            }
        }

        if (amount <= 0)
        {
            Destroy(spawn.gameObject);
        }
    }

    private IEnumerator EnablePickupAfterDelay(Pickup pickup, float delay)
    {
        yield return new WaitForSeconds(delay);
        pickup.enabled = true;
    }
}
