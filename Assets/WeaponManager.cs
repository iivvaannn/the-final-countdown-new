using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject armsak;

    public GameObject ak47;
    public GameObject shotgun;
  //  public GameObject handgun;

    private WeaponType? currentWeapon = null;

    void Start()
    {
        armsak.SetActive(false);
        DisableAllWeapons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(WeaponType.AK47);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(WeaponType.Shotgun);
    }

    public void PickupWeapon(WeaponType type)
    {
        armsak.SetActive(true);
        Equip(type);
    }

    void Equip(WeaponType type)
    {
        DisableAllWeapons();
        currentWeapon = type;

        if (type == WeaponType.AK47) ak47.SetActive(true);
        if (type == WeaponType.Shotgun) shotgun.SetActive(true);
      //  if (type == WeaponType.Handgun) handgun.SetActive(true);
    }

    void DisableAllWeapons()
    {
        ak47.SetActive(false);
        shotgun.SetActive(false);
       // handgun.SetActive(false);
    }
}
