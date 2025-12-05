using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class GettingInAndOutVehicle : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject playerModel = null; // modellen att dölja

    private Rigidbody playerRb;
    private Collider playerCollider;

    [Header("Car")]
    [SerializeField] GameObject car = null;
    [SerializeField] Transform playerSeat = null; // Empty GameObject för sits
    private CarUserControl carController;

    [Header("Input")]
    [SerializeField] KeyCode enterExitKey = KeyCode.E;

    [Header("Cameras")]
    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private Camera carCamera = null;

    [Header("UI")]
    [SerializeField] private GameObject drivePromptUI = null;

    [SerializeField] float closeDistance = 3f;
    private bool onVehicle = false;

    private void Start()
    {
        carController = car.GetComponent<CarUserControl>();
        carController.enabled = false;

        playerRb = player.GetComponent<Rigidbody>();
        playerCollider = player.GetComponent<Collider>();

        if (drivePromptUI != null)
            drivePromptUI.SetActive(false);

        SwitchCamera(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(car.transform.position, player.transform.position);

        if (!onVehicle && distance < closeDistance)
        {
            if (drivePromptUI != null)
                drivePromptUI.SetActive(true);
        }
        else
        {
            if (drivePromptUI != null)
                drivePromptUI.SetActive(false);
        }

        if (Input.GetKeyDown(enterExitKey))
        {
            if (onVehicle)
                DismountVehicle();
            else if (distance < closeDistance)
                MountVehicle();
        }
    }

    void MountVehicle()
    {
        onVehicle = true;

        // Placera spelaren i sitsen och gör den till barn till sitsen
        player.transform.position = playerSeat.position;
        player.transform.rotation = playerSeat.rotation;
        player.transform.SetParent(playerSeat);

        // Dölj spelarmodellen
        if (playerModel != null)
            playerModel.SetActive(false);

        // Stäng av fysik och collider
        if (playerRb != null)
            playerRb.isKinematic = true;

        if (playerCollider != null)
            playerCollider.enabled = false;

        carController.enabled = true;

        if (drivePromptUI != null)
            drivePromptUI.SetActive(false);

        SwitchCamera(true);
    }

    void DismountVehicle()
    {
        onVehicle = false;

        player.transform.SetParent(null);

        // Placera spelaren vid sidan av bilen
        player.transform.position = car.transform.position + car.transform.TransformDirection(Vector3.left * 2f);
        player.transform.rotation = Quaternion.identity;

        // Visa spelarmodellen
        if (playerModel != null)
            playerModel.SetActive(true);

        // Slå på fysik och collider igen
        if (playerRb != null)
            playerRb.isKinematic = false;

        if (playerCollider != null)
            playerCollider.enabled = true;

        carController.enabled = false;

        SwitchCamera(false);
    }

    private void SwitchCamera(bool isOnVehicle)
    {
        playerCamera.enabled = !isOnVehicle;
        carCamera.enabled = isOnVehicle;

        playerCamera.tag = isOnVehicle ? "Untagged" : "MainCamera";
        carCamera.tag = isOnVehicle ? "MainCamera" : "Untagged";
    }
}
