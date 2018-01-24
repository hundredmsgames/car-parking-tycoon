using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingController : MonoBehaviour
{
	static CarParkController carParkController;

	Transform topLeft;
	Transform topRight;
	Transform bottomLeft;
	Transform bottomRight;

	void Start()
	{
		topLeft     = this.transform.Find("TopLeftPoint");
		topRight    = this.transform.Find("TopRightPoint");
		bottomLeft  = this.transform.Find("BottomLeftPoint");
		bottomRight = this.transform.Find("BottomRightPoint");
	}

	public Vector3[] GetParkingInfo()
	{
		Vector3[] parkingInfo = new Vector3[4];

		// Car top left point
		parkingInfo[0] = topLeft.position;

		// Car top right point
		parkingInfo[1] = topRight.position;

		// Car bottom left point
		parkingInfo[2] = bottomLeft.position;

		// Car bottom right point
		parkingInfo[3] = bottomRight.position;

		return parkingInfo;
	}

	void OnTriggerStay(Collider col)
	{
		if(col.tag != "ParkingSpace")
			return;

		if(carParkController == null)
			carParkController = GameObject.FindObjectOfType<CarParkController>();

		ParkingSpace ps = carParkController.GetParkingSpace(col.gameObject);

		if(ps.occupied == true)
			return;

		carParkController.ToggleParkingSpaceIndicator(ps, true);
		WorldController.Instance.world.ParkCar(ps);
	}

	void OnTriggerExit(Collider col)
	{
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
