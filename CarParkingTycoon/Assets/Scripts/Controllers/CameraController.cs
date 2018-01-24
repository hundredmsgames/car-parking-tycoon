using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed = 3f;
    Vector3 enterOfCarPark;
	Camera mainCamera;

    bool cameraLocked;

	void Start()
	{
		mainCamera = Camera.main;
		enterOfCarPark = mainCamera.transform.position;
	}

	void LateUpdate()
	{
		Car carForParking = WorldController.Instance.world.carForParking;
		Vector3 cameraPos = mainCamera.transform.position;

		if(carForParking == null)
		{
            cameraLocked = false;
            mainCamera.transform.position = enterOfCarPark;
			return;
		}

		GameObject carForParkingGo = WorldController.Instance.carGoDic[carForParking];
        Vector3 carPos = enterOfCarPark;

        if (cameraLocked == false && Vector2.Distance(new Vector2(cameraPos.x, cameraPos.z),
				new Vector2(carForParkingGo.transform.position.x, carForParkingGo.transform.position.z)) > .1
		){
			carPos = Vector3.Lerp(mainCamera.transform.position, carForParkingGo.transform.position, Time.deltaTime * speed);
		}
		else
        {
            cameraLocked = true;
            carPos = WorldController.Instance.carGoDic[carForParking].transform.position;
        }

        carPos.y = enterOfCarPark.y;
		mainCamera.transform.position = carPos;

	}
}
