using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingController : MonoBehaviour
{
	CarParkController carParkController;



	void OnTriggerStay(Collider col)
	{

        
		Debug.Log("Stay");

		if(col.tag != "ParkingSpace")
			return;

		if(carParkController == null)
			carParkController = GameObject.FindObjectOfType<CarParkController>();

		ParkingSpace ps = carParkController.GetParkingSpace(col.gameObject);

		if(ps.occupied == true)
			return;
		
		WorldController.Instance.world.ParkCar(ps);
		carParkController.ToggleParkingSpaceIndicator(ps, true);
	}

	void OnTriggerExit(Collider col)
	{
		Debug.Log("Exit");

		if(col.tag != "ParkingSpace")
			return;

		if(carParkController == null)
			carParkController = GameObject.FindObjectOfType<CarParkController>();

		ParkingSpace ps = carParkController.GetParkingSpace(col.gameObject);

		if(ps.occupied == true)
			return;

		carParkController.ToggleParkingSpaceIndicator(ps, false);
	}
}
