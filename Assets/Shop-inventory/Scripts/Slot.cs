using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isFull = false;

    // (valfri) håller koll på vilket item som ligger här
    public string itemName;

    // (valfri) referens till item UI objektet i sloten
    public GameObject currentItem;
}