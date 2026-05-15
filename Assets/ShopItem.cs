using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int price = 50;
    public string weaponType;

    public GameObject notEnoughText;

    public enum ItemType
    {
        Weapon,
        Ammo,
        Medkit
    }

    public ItemType itemType;
    public int amount = 10;

    private PlayerGold player;
    private WeaponSystem weaponSystem;

    void Start()
    {
        player = FindObjectOfType<PlayerGold>();
        weaponSystem = FindObjectOfType<WeaponSystem>();
    }

    public void Buy()
    {
        if (!player.SpendGold(price))
        {
            ShowNotEnough();
            return;
        }

        switch (itemType)
        {
            case ItemType.Weapon:
                BuyWeapon();
                break;

            case ItemType.Ammo:
                GiveAmmo();
                break;

            case ItemType.Medkit:
                GiveMedkit();
                break;
        }
    }

    void BuyWeapon()
    {
        if (IsOwned())
        {
            Debug.Log("Already owned: " + weaponType);
            return;
        }

        Debug.Log("Bought: " + weaponType);

        switch (weaponType)
        {
            case "AK":
                weaponSystem.hasAK = true;
                weaponSystem.SendMessage("EquipAK");
                break;

            case "Shotgun":
                weaponSystem.hasShotgun = true;
                weaponSystem.SendMessage("EquipShotgun");
                break;

            case "Revolver":
                weaponSystem.hasRevolver = true;
                weaponSystem.SendMessage("EquipRevolver");
                break;

            case "Sniper":
                weaponSystem.hasSniper = true;
                weaponSystem.SendMessage("EquipSniper");
                break;

            case "Handgun":
                weaponSystem.hasHandgun = true;
                weaponSystem.SendMessage("EquipHandgun");
                break;
        }
    }

    void GiveAmmo()
    {
        Gun[] guns = FindObjectsOfType<Gun>(true);

        foreach (Gun gun in guns)
        {
            switch (weaponType)
            {
                // AK ammo
                case "AK":

                    if (gun.gameObject.name.Contains("AK"))
                    {
                        gun.reserveAmmo += amount;
                        Debug.Log("AK ammo added");
                    }

                    break;

                // Shotgun ammo
                case "Shotgun":

                    if (gun.gameObject.name.Contains("Shotgun"))
                    {
                        gun.reserveAmmo += amount;
                        Debug.Log("Shotgun ammo added");
                    }

                    break;

                // Sniper ammo
                case "Sniper":

                    if (gun.gameObject.name.Contains("Sniper"))
                    {
                        gun.reserveAmmo += amount;
                        Debug.Log("Sniper ammo added");
                    }

                    break;

                // Handgun + Revolver share ammo
                case "Handgun":

                    if (gun.gameObject.name.Contains("Handgun") ||
                        gun.gameObject.name.Contains("Revolver"))
                    {
                        gun.reserveAmmo += amount;
                        Debug.Log("Handgun ammo added");
                    }

                    break;
            }
        }
    }

    void GiveMedkit()
    {
        Medkit medkit = FindObjectOfType<Medkit>();

        if (medkit != null)
        {
            medkit.AddMedkits(amount);

            Debug.Log("Medkit added: " + amount);
        }
    }

    bool IsOwned()
    {
        switch (weaponType)
        {
            case "AK": return weaponSystem.hasAK;
            case "Shotgun": return weaponSystem.hasShotgun;
            case "Revolver": return weaponSystem.hasRevolver;
            case "Sniper": return weaponSystem.hasSniper;
            case "Handgun": return weaponSystem.hasHandgun;
        }

        return false;
    }

    void ShowNotEnough()
    {
        Debug.Log("Not enough gold");

        if (notEnoughText != null)
        {
            notEnoughText.SetActive(true);
            Invoke(nameof(HideText), 2f);
        }
    }

    void HideText()
    {
        if (notEnoughText != null)
            notEnoughText.SetActive(false);
    }
}