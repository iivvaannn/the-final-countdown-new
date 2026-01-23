using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public Camera cam;
    public WeaponManager weaponManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, pickupRange))
            {
                WeaponPickup pickup = hit.collider.GetComponent<WeaponPickup>();
                if (pickup != null)
                {
                    weaponManager.PickupWeapon(pickup.weaponType);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
