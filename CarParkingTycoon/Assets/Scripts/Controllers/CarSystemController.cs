using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystemController : MonoBehaviour {

    DriveController driveController = null;

    public GameObject CarControlCanvas;
    // Use this for initialization
    void Start () {
        driveController = GetComponent<DriveController>();

        CarControlCanvas = WorldController.Instance.CarControlCanvas;

    }
	
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
                if (driveController != null)
                {

                //activate UI
                if(CarControlCanvas.activeSelf == false)
                    CarControlCanvas.SetActive(true);

                CarControlCanvas.transform.SetParent(transform);
                CarControlCanvas.GetComponent<CarControlCanvasController>().SetCurrentCarsDriverController(driveController);

                }
            }

        }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Obstacle")
        {
            WorldController.Instance.world.carForParking.TakeDamage();
        }
    }
}
