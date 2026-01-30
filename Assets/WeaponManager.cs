using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Armsaks")]
    public GameObject armsakRifle;       // Armsak för rifles (AK, shotgun etc.)
    public GameObject armsakPistol;      // Armsak för pistoler

    [Header("Weapon Points")]
    public Transform rifleWeaponPoint;   // WeaponPoint i armsakRifle
    public Transform pistolWeaponPoint;  // WeaponPoint i armsakPistol

    private GameObject currentWeapon;    // Vapnet som är aktivt i handen

    // Equip weapon från prefab
    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning("Weapon prefab is null!");
            return;
        }

        // Ta bort tidigare vapen
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }

        // Hämta Gun-script från prefab
        Gun gun = weaponPrefab.GetComponent<Gun>();
        if (gun == null)
        {
            Debug.LogError("The prefab does not have a Gun script!");
            return;
        }

        // Dölj båda armsaks först
        armsakRifle.SetActive(false);
        armsakPistol.SetActive(false);

        // Bestäm vilken armsak och weaponPoint som ska användas
        Transform targetPoint;
        if (gun.weaponType == WeaponType.Rifle)
        {
            armsakRifle.SetActive(true);
            targetPoint = rifleWeaponPoint;
        }
        else if (gun.weaponType == WeaponType.Pistol)
        {
            armsakPistol.SetActive(true);
            targetPoint = pistolWeaponPoint;
        }
        else
        {
            Debug.LogWarning("Unknown weapon type!");
            return;
        }

        // Instansiera vapnet som child till WeaponPoint
        currentWeapon = Instantiate(weaponPrefab);
        currentWeapon.transform.SetParent(targetPoint, false);
        // 'false' gör att prefabens lokala position/rotation/scale behålls exakt
    }

    // Ta bort nuvarande vapen (t.ex. drop)
    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }

        armsakRifle.SetActive(false);
        armsakPistol.SetActive(false);
    }
}
