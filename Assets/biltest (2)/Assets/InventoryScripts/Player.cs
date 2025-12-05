using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float holdTime = 5f;  // Hur länge man måste hålla E
    private float holdTimer = 5f;     // Nedräkning för tiden
    private Pickup currentPickup;
    internal int Gold;

    void Update()
    {
        // Kontrollera om det finns ett pickup-objekt att interagera med
        if (currentPickup != null)
        {
            // Om vi håller nere E
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("E trycks ned!");  // Kontrollera om E registreras
                holdTimer -= Time.deltaTime;  // Nedräkning
                Debug.Log("Håller E: " + holdTimer.ToString("F2") + " sek kvar");

                if (holdTimer <= 0f)
                {
                    Debug.Log("Objekt plockat upp!");
                    
                    holdTimer = holdTime;   // Återställ timer
                    currentPickup = null;   // Rensa den aktuella pickupen
                }
            }
            else
            {
                // Om vi inte håller E, se till att tiden återställs
                if (holdTimer != holdTime)
                {
                    Debug.Log("Släppte E för tidigt. Timer återställd.");
                    holdTimer = holdTime;    // Återställ timer
                }
            }
        }
        else
        {
            Debug.Log("Ingen pickup i närheten.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Gick in i triggern med: " + other.name);  // Debug för OnTriggerEnter
        if (other.CompareTag("PickUp"))
        {
            currentPickup = other.GetComponent<Pickup>();  // Hämta pickupen
            holdTimer = holdTime;   // När vi går nära, sätt timer till max
            Debug.Log("Nära pickup: " + other.name);  // Logga pickupen
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Lämnade triggern med: " + other.name);  // Debug för OnTriggerExit
        if (other.CompareTag("PickUp"))
        {
            currentPickup = null;
        }
    }
}
