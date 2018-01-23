using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParkController : MonoBehaviour
{
	Dictionary<ParkingSpace, GameObject> parkingSpaceGoMap;
	Dictionary<GameObject, ParkingSpace> goToParkingSpaceMap;

	// Use this for initialization
	void Start ()
	{
		Transform parkingSpaces = GameObject.Find("Environments/CarPark/ParkingSpaces").transform;
		parkingSpaceGoMap   = new Dictionary<ParkingSpace, GameObject>();
		goToParkingSpaceMap = new Dictionary<GameObject, ParkingSpace>();

		for(int i = 0; i < parkingSpaces.childCount; i++)
		{
			Transform psTrf  = parkingSpaces.GetChild(i);
			Transform area  = psTrf.Find("Area");

			ParkingSpace ps = new ParkingSpace(psTrf.position.x, psTrf.position.z, area.localScale.x, area.localScale.z);
			WorldController.Instance.world.carPark.AddParkingSpace(ps);
			parkingSpaceGoMap.Add(ps, psTrf.gameObject);
			goToParkingSpaceMap.Add(psTrf.gameObject, ps);
		}
	}
	
	public ParkingSpace GetParkingSpace(GameObject psGo)
	{
		return goToParkingSpaceMap[psGo];
	}

	public void ToggleParkingSpaceIndicator(ParkingSpace ps, bool enable)
	{
		Transform psTrf = parkingSpaceGoMap[ps].transform;
		GameObject area  = psTrf.Find("Area").gameObject;

		area.SetActive(enable);
	}
}
