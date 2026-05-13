using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAmmo : MonoBehaviour
{
    public Image weaponIcon;
    public TMP_Text ammoText;
    public GameObject ammoPanel;

    private WeaponSystem weaponSystem;

    void Start()
    {
        weaponSystem = FindObjectOfType<WeaponSystem>();
    }

    void Update()
    {
        if (weaponSystem == null)
        {
            ammoPanel.SetActive(false);
            return;
        }

        Gun gun = weaponSystem.GetCurrentGun();

        if (gun == null)
        {
            ammoPanel.SetActive(false);
            return;
        }

        ammoPanel.SetActive(true);

        weaponIcon.sprite = gun.weaponIcon;
        ammoText.text = gun.currentAmmo + " / " + gun.reserveAmmo;
    }
}
