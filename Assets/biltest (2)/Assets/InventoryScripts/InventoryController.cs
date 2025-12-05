using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DroppableItem
{
    public string itemName;
    public GameObject worldPrefab;
}

public class InventoryController : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;

    [Header("Droppbara föremål")]
    public List<DroppableItem> droppableItems;

    //public List<WeaponData> weapons = new List<WeaponData>();

    public GameObject GetDropPrefab(string itemName)
    {
        /*for (int i = 0; i < weapons.Count; i++)
        {
            WeaponData currentWeapon = weapons[i];
        }*/

        foreach (var item in droppableItems)
        {
            if (item.itemName == itemName)
                return item.worldPrefab;
        }
        Debug.LogWarning("Hittar inte prefab för: " + itemName);
        return null;
    }
}
