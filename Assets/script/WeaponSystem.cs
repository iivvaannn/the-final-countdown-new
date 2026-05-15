using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // UIAmmo reference REMOVED

    public GameObject knife;
    public GameObject armsakKnife;
   // public GameObject knifeIcon;

    public bool hasKnife;

    [Header("Idle Arms")]
    public GameObject fpsArms;

    [Header("Armsaks")]
    public GameObject armsakRifle;
    public GameObject armsakHandgun;

    [Header("Weapon Points")]
    public Transform riflePoint;
    public Transform handgunPoint;

    [Header("Weapons under WeaponPoint (already placed)")]
    public GameObject ak;
    public GameObject shotgun;
    public GameObject revolver;
    public GameObject sniper;
    public GameObject handgun;

    [Header("Camera")]
    public Camera cam;
    public float pickupDistance = 3f;

    [Header("Inventory UI")]
    public GameObject akIcon;
    public GameObject shotgunIcon;
    public GameObject revolverIcon;
    public GameObject sniperIcon;
    public GameObject handgunIcon;

    public bool hasAK;
    public bool hasShotgun;
    public bool hasRevolver;
    public bool hasSniper;
    public bool hasHandgun;

    GameObject currentWeapon;

    void Start()
    {
        fpsArms.SetActive(true);

        armsakRifle.SetActive(false);
        armsakHandgun.SetActive(false);

        knife.SetActive(false);
        armsakKnife.SetActive(false);

     //   if (knifeIcon != null)
       //     knifeIcon.SetActive(false);

        ak.SetActive(false);
        shotgun.SetActive(false);
        revolver.SetActive(false);
        sniper.SetActive(false);
        handgun.SetActive(false);
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
            else if (name.Contains("Revolver"))
            {
                hasRevolver = true;
                EquipRevolver();
            }
            else if (name.Contains("Sniper"))
            {
                hasSniper = true;
                EquipSniper();
            }
            else if (name.Contains("Handgun"))
            {
                hasHandgun = true;
                EquipHandgun();
            }
            else if (name.Contains("Knife"))
            {
                hasKnife = true;
                EquipKnife();
            }

            hit.transform.gameObject.SetActive(false);
        }
    }

    // ---------------- SWITCH ----------------
    void HandleSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasKnife) EquipKnife();
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasAK) EquipAK();
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasShotgun) EquipShotgun();
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasRevolver) EquipRevolver();
        if (Input.GetKeyDown(KeyCode.Alpha5) && hasSniper) EquipSniper();
        if (Input.GetKeyDown(KeyCode.Alpha6) && hasHandgun) EquipHandgun();

        // UNEQUIP
        if (Input.GetKeyDown(KeyCode.X))
        {
            UnequipAll();
        }
    }

    // ---------------- EQUIP ----------------
    void ClearWeapons()
    {
        ak.SetActive(false);
        shotgun.SetActive(false);
        revolver.SetActive(false);
        sniper.SetActive(false);
        handgun.SetActive(false);

        armsakRifle.SetActive(false);
        armsakHandgun.SetActive(false);

        fpsArms.SetActive(true);

        knife.SetActive(false);
        armsakKnife.SetActive(false);


        if (akIcon != null) akIcon.SetActive(false);
        if (shotgunIcon != null) shotgunIcon.SetActive(false);
        if (revolverIcon != null) revolverIcon.SetActive(false);
        if (sniperIcon != null) sniperIcon.SetActive(false);
        if (handgunIcon != null) handgunIcon.SetActive(false);
      //  if (knifeIcon != null) knifeIcon.SetActive(false);
    }

    void EquipAK()
    {
        ClearWeapons();
        fpsArms.SetActive(false);

        armsakRifle.SetActive(true);
        ak.SetActive(true);
        currentWeapon = ak;
        akIcon.SetActive(true);
    }

    void EquipShotgun()
    {
        ClearWeapons();
        fpsArms.SetActive(false);

        armsakRifle.SetActive(true);
        shotgun.SetActive(true);
        currentWeapon = shotgun;
        shotgunIcon.SetActive(true);
    }

    void EquipRevolver()
    {
        ClearWeapons();
        fpsArms.SetActive(false);

        armsakHandgun.SetActive(true);
        revolver.SetActive(true);
        currentWeapon = revolver;
        revolverIcon.SetActive(true);
    }

    void EquipSniper()
    {
        ClearWeapons();
        fpsArms.SetActive(false);

        armsakRifle.SetActive(true);
        sniper.SetActive(true);
        currentWeapon = sniper;
        sniperIcon.SetActive(true);
    }

    void EquipHandgun()
    {
        ClearWeapons();
        fpsArms.SetActive(false);

        armsakHandgun.SetActive(true);
        handgun.SetActive(true);
        currentWeapon = handgun;
        handgunIcon.SetActive(true);
    }

    void EquipKnife()
    {
        ClearWeapons();
        fpsArms.SetActive(false);

        armsakKnife.SetActive(true);
        knife.SetActive(true);

        currentWeapon = knife;

      //  if (knifeIcon != null)
        //    knifeIcon.SetActive(true);
    }

    // ---------------- UNEQUIP ----------------
    void UnequipAll()
    {
        ClearWeapons();
        currentWeapon = null;
    }

    // THIS is what UIAmmo will read
    public Gun GetCurrentGun()
    {
        if (currentWeapon == null) return null;
        return currentWeapon.GetComponentInChildren<Gun>();
    }
}