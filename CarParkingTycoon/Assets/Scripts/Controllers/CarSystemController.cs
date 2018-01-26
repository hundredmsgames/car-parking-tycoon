using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystemController : MonoBehaviour {

    DriveController driveController = null;

	public CarControlCanvasController carControlCanvas;

    // Use this for initialization
    void Start ()
	{
        driveController = GetComponent<DriveController>();
		carControlCanvas = WorldController.Instance.CarControlCanvas.GetComponent<CarControlCanvasController>();
    }
	
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (driveController != null)
			{
                //activate UI
				if(carControlCanvas.gameObject.activeSelf == false)
					carControlCanvas.gameObject.SetActive(true);

                carControlCanvas.transform.SetParent(transform);
                carControlCanvas.SetCurrentCarsDriverController(driveController.car);
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
