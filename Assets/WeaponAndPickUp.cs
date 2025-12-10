using UnityEngine;

public class WeaponPickupAndDrop : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject armsakPrefab;      // Armsak med armar + WeaponPoint
    public Transform weaponPoint;        // WeaponPoint i Armsak
    public GameObject worldWeapon;       // Vapnet som ligger på mappen
    private bool hasWeapon = false;

    void Start()
    {
        armsakPrefab.SetActive(false); // Dölj Armsak från början
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!hasWeapon)
                PickUpWeapon();
            else
                DropWeapon();
        }
    }

    void PickUpWeapon()
    {
        if (worldWeapon == null) return;

        // Dölj vapnet i världen
        worldWeapon.SetActive(false);

        // Visa Armsak
        armsakPrefab.SetActive(true);

        // Flytta vapnet i Armsak
        Transform gunInArmsak = armsakPrefab.transform.Find("WeaponPoint/Handgun");
        if (gunInArmsak != null)
        {
            gunInArmsak.localPosition = Vector3.zero;
            gunInArmsak.localRotation = Quaternion.identity;
        }

        hasWeapon = true;
    }

    void DropWeapon()
    {
        if (!hasWeapon) return;

        // Dölj Armsak
        armsakPrefab.SetActive(false);

        // Visa vapnet i världen igen
        worldWeapon.SetActive(true);

        hasWeapon = false;
    }
}
