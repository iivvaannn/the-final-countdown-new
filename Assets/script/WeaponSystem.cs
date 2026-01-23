using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public GameObject armsak;

    public GameObject akInHand;
    public GameObject shotgunInHand;

    private bool hasAK = false;
    private bool hasShotgun = false;

    private GameObject currentWeapon;

    public float pickupDistance = 3f;
    public Camera cam;

    void Start()
    {
        armsak.SetActive(false);
        akInHand.SetActive(false);
        shotgunInHand.SetActive(false);
    }

    void Update()
    {
        HandlePickup();
        HandleWeaponSwitch();
    }

    void HandlePickup()
    {
        if (!Input.GetKeyDown(KeyCode.V)) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupDistance))
        {
            if (!hit.transform.CompareTag("Weapon")) return;

            if (hit.transform.name.Contains("AK"))
            {
                hasAK = true;
                hit.transform.gameObject.SetActive(false);
                EquipAK();
            }
            else if (hit.transform.name.Contains("Shotgun"))
            {
                hasShotgun = true;
                hit.transform.gameObject.SetActive(false);
                EquipShotgun();
            }
        }
    }

    void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAK)
        {
            EquipAK();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasShotgun)
        {
            EquipShotgun();
        }
    }

    void EquipAK()
    {
        armsak.SetActive(true);

        akInHand.SetActive(true);
        shotgunInHand.SetActive(false);

        currentWeapon = akInHand;
    }

    void EquipShotgun()
    {
        armsak.SetActive(true);

        shotgunInHand.SetActive(true);
        akInHand.SetActive(false);

        currentWeapon = shotgunInHand;
    }
}
