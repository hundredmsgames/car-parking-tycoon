using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	Vector3 enterOfCarPark;
	Camera mainCamera;

	void Start()
	{
		mainCamera = Camera.main;
		enterOfCarPark = mainCamera.transform.position;
	}

	void Update ()
	{
		Car carForParking = WorldController.Instance.world.carForParking;

		if(carForParking == null)
		{
			mainCamera.transform.position = enterOfCarPark;
			return;
		}

		Vector3 carPos = WorldController.Instance.carGoDic[carForParking].transform.position;
		carPos.y = enterOfCarPark.y;
		mainCamera.transform.position = carPos;
	}
}
