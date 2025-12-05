using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Vehicles.Car; 
public class GettingInAndOutOfCars : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] AutoCam mCamera = null; 

    [Header ("Human")]
    [SerializeField] GameObject human = null;

    [SerializeField] float closeDistance = 15f; 

    [Space, Header("Car Stuff")]
    [SerializeField] GameObject car = null;
    [SerializeField] CarUserControl carController = null;
    [SerializeField] CarController carEngine = null;

    [Header ("Input")]
    [SerializeField] KeyCode enterExitKey = KeyCode.E; 

    bool inCar = false;

    private void Start()
    {
        inCar = car.activeSelf; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(enterExitKey))
        {
            if (inCar)
                GetOutOfCar();
            else if (Vector3.Distance (car.transform.position, human.transform.position) < closeDistance) //if out of car 
                GetIntoCar(); 
        }
    }

    void GetOutOfCar ()
    {
        inCar = false;

        human.SetActive(true);

        human.transform.position = car.transform.position + car.transform.TransformDirection(Vector3.left);

        mCamera.SetTarget(human.transform);

        carController.enabled = false;

        carEngine.Move(0, 0, 1, 1); 
    }

    void GetIntoCar ()
    {
        inCar = true;

        human.SetActive(false);

        mCamera.SetTarget(car.transform);

        carController.enabled = true;

        // Starta bilen (om det finns en metod för att starta motorn)
        carEngine.Move(1, 1, 1, 1); // Se till att bilen är i ett körbart tillstånd


    }
}
