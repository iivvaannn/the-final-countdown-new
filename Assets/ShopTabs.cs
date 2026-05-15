using UnityEngine;

public class ShopTabs : MonoBehaviour
{
    public GameObject weaponsPanel;
    public GameObject consumablesPanel;

    public void OpenWeapons()
    {
        weaponsPanel.SetActive(true);
        consumablesPanel.SetActive(false);
    }

    public void OpenConsumables()
    {
        weaponsPanel.SetActive(false);
        consumablesPanel.SetActive(true);
    }
}