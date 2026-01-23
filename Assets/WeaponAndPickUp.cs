using UnityEngine;

public class WeaponAndPickUp: MonoBehaviour
{
    public Camera playerCamera;
    public GameObject armsak;

    //public GameObject handgunInHand;        
    public GameObject akInHand;
    public GameObject shotgunInHand;

    //private bool hasHandgun;
    private bool hasAK;
    private bool hasShotgun;

    private GameObject currentWeapon;

    public float pickupDistance = 3f;

    void Start()
    {
        armsak.SetActive(false);

       // handgunInHand.SetActive(false);
        akInHand.SetActive(false);
        shotgunInHand.SetActive(false);
    }

    void Update()
    {
        HandlePickup();
        HandleSwitch();
    }

    void HandlePickup()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupDistance))
        {
            if (!hit.transform.CompareTag("Weapon")) return;

           // if (hit.transform.name.Contains("Handgun"))
            //{
              //  hasHandgun = true;
                //hit.transform.gameObject.SetActive(false);
               // Equip(handgunInHand);
           // }
            else if (hit.transform.name.Contains("AK"))
            {
                hasAK = true;
                hit.transform.gameObject.SetActive(false);
                Equip(akInHand);
            }
            else if (hit.transform.name.Contains("Shotgun"))
            {
                hasShotgun = true;
                hit.transform.gameObject.SetActive(false);
                Equip(shotgunInHand);
            }
        }
    }

    void HandleSwitch()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1) && hasHandgun)
        //    Equip(handgunInHand);

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasAK)
            Equip(akInHand);

        if (Input.GetKeyDown(KeyCode.Alpha3) && hasShotgun)
            Equip(shotgunInHand);
    }

    void Equip(GameObject weapon)
    {
        armsak.SetActive(true);

       // handgunInHand.SetActive(false);
        akInHand.SetActive(false);
        shotgunInHand.SetActive(false);

        weapon.SetActive(true);
        currentWeapon = weapon;
    }
}
