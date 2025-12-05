using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class NPCSlot : MonoBehaviour
{
    public Player2 player;

    private InventoryController inventory;

    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemAmount;

    public GameObject itemToBuy;
    public int _ItemAmount;
    public TextMeshProUGUI buyPriceText;

    void Start()
    {
        player = FindObjectOfType<Player2>();
        inventory = FindObjectOfType<InventoryController>();

        if (player == null || inventory == null || itemToBuy == null || itemName == null || itemPrice == null)
        {
            Debug.LogError("Missing references in NPCSlot");
            return;
        }

        int price = itemToBuy.GetComponentInChildren<Spawn>().itemPrice;
        itemName.text = itemToBuy.GetComponent<Spawn>().itemName;
        itemImage.sprite = itemToBuy.GetComponent<Image>().sprite;
        buyPriceText.text = price + " Gold";
    }

    void Update()
    {
        if (itemAmount != null)
            itemAmount.text = "Amount: " + _ItemAmount.ToString();
    }

    public void Buy()
    {
        Debug.Log("Attempting to buy item: " + itemName.text);

        int price = itemToBuy.GetComponentInChildren<Spawn>().itemPrice;
        buyPriceText.text = price + " Gold";

        if (player.Gold < price)
        {
            Debug.Log("Not enough gold to buy the item!");
            return;
        }

        if (_ItemAmount <= 0)
        {
            Debug.Log("No items left to buy!");
            return;
        }

        string itemToBuyName = itemToBuy.GetComponent<Spawn>().itemName;

        // 1. Försök stacka i befintlig slot
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Slot slot = inventory.slots[i].GetComponent<Slot>();
            Spawn existingSpawn = inventory.slots[i].GetComponentInChildren<Spawn>();

            if (inventory.isFull[i] &&
                existingSpawn != null &&
                existingSpawn.itemName == itemToBuyName &&
                slot.amount < slot.maxStackSize)
            {
                slot.amount += 1;
                player.Gold -= price;
                _ItemAmount -= 1;
                Debug.Log("Item stacked! New gold: " + player.Gold);
                return;
            }
        }

        // 2. Leta efter tom slot i inventory
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (!inventory.isFull[i])
            {
                GameObject newItem = Instantiate(itemToBuy, inventory.slots[i].transform, false);
                Slot slot = inventory.slots[i].GetComponent<Slot>();

                inventory.isFull[i] = true;
                slot.amount = 1;
                slot.itemName = itemToBuyName;

                player.Gold -= price;
                _ItemAmount -= 1;

                Debug.Log("Item added to new slot. Gold left: " + player.Gold);

                // ✅ Låt Shooting.cs hantera vapnet själv
                //Shooting shooting = FindObjectOfType<Shooting>();
                //if (shooting != null)
                //{
                //    for (int j = 0; j < shooting.weapons.Count; j++)
                //    {
                //        if (itemToBuy.tag == shooting.weapons[j].tagName)
                //        {
                //            shooting.BuyWeapon(j);
                //            Debug.Log($"Köpt vapen '{itemToBuy.tag}' via Shooting.BuyWeapon()");
                //            return;
                //        }
                //    }
                //}

                return;
            }
        }

        Debug.Log("Inventory full. Can't buy item.");
    }

    public void Sell()
    {
        Debug.Log("Attempting to sell");

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Slot slot = inventory.slots[i].GetComponent<Slot>();
            Spawn spawn = inventory.slots[i].GetComponentInChildren<Spawn>();

            if (spawn != null && spawn.itemName == itemToBuy.GetComponent<Spawn>().itemName)
            {
                if (slot.amount > 0)
                {
                    int sellPrice = spawn.itemPrice;

                    slot.amount -= 1;
                    player.Gold += sellPrice;
                    _ItemAmount += 1;

                    Debug.Log("Item sold! Gold: " + player.Gold);

                    if (slot.amount <= 0)
                    {
                        Destroy(spawn.gameObject);
                        inventory.isFull[i] = false;
                        slot.itemName = "";
                    }

                    break;
                }
            }
        }
    }
}
