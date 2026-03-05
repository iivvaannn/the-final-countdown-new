using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // UIAmmo reference REMOVED

    [Header("Armsaks")]
    public GameObject armsakRifle;
    public GameObject armsakHandgun;

    [Header("Weapon Points")]
    public Transform riflePoint;
    public Transform handgunPoint;

    [Header("Weapons under WeaponPoint (already placed)")]
    public GameObject ak;
    public GameObject shotgun;
    public GameObject handgun;
    public GameObject sniper;

    [Header("Camera")]
    public Camera cam;
    public float pickupDistance = 3f;

    bool hasAK;
    bool hasShotgun;
    bool hasHandgun;
    bool hasSniper;

    GameObject currentWeapon;

    void Start()
    {
        armsakRifle.SetActive(false);
        armsakHandgun.SetActive(false);

        ak.SetActive(false);
        shotgun.SetActive(false);
        handgun.SetActive(false);
        sniper.SetActive(false);
    }

    void Update()
    {
        HandlePickup();
        HandleSwitch();
    }

    // ---------------- PICKUP ----------------
    void HandlePickup()
    {
        if (!Input.GetKeyDown(KeyCode.V)) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupDistance))
        {
            if (!hit.transform.CompareTag("Weapon")) return;

            string name = hit.transform.name;

            if (name.Contains("AK"))
            {
                hasAK = true;
                EquipAK();
            }
            else if (name.Contains("Shotgun"))
            {
                hasShotgun = true;
                EquipShotgun();
            }
            else if (name.Contains("Handgun"))
            {
                hasHandgun = true;
                EquipHandgun();
            }
            else if (name.Contains("Sniper"))
            {
                hasSniper = true;
                EquipSniper();
            }

            hit.transform.gameObject.SetActive(false);
        }
    }

    // ---------------- SWITCH ----------------
    void HandleSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasAK) EquipAK();
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasShotgun) EquipShotgun();
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasHandgun) EquipHandgun();
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasSniper) EquipSniper();
    }

    // ---------------- EQUIP ----------------
    void ClearWeapons()
    {
        ak.SetActive(false);
        shotgun.SetActive(false);
        handgun.SetActive(false);
        sniper.SetActive(false);

        armsakRifle.SetActive(false);
        armsakHandgun.SetActive(false);
    }

    void EquipAK()
    {
        ClearWeapons();
        armsakRifle.SetActive(true);
        ak.SetActive(true);
        currentWeapon = ak;
    }

    void EquipShotgun()
    {
        ClearWeapons();
        armsakRifle.SetActive(true);
        shotgun.SetActive(true);
        currentWeapon = shotgun;
    }

    void EquipHandgun()
    {
        ClearWeapons();
        armsakHandgun.SetActive(true);
        handgun.SetActive(true);
        currentWeapon = handgun;
    }

    void EquipSniper()
    {
        ClearWeapons();
        armsakRifle.SetActive(true);
        sniper.SetActive(true);
        currentWeapon = sniper;
    }

    // THIS is what UIAmmo will read
    public Gun GetCurrentGun()
    {
        if (currentWeapon == null) return null;
        return currentWeapon.GetComponentInChildren<Gun>();
    }
}
