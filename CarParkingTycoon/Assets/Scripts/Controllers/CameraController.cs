using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed = 3f;
    Vector3 enterOfCarPark;
	Camera mainCamera;

    bool cameraLocked=false;

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
            cameraLocked = false;
            mainCamera.transform.position = enterOfCarPark;
			return;
		}
        Vector3 carPos=enterOfCarPark;

      //  Debug.Log();

        if (cameraLocked ==false && 
            Vector2.Distance(new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z),
            new Vector2(WorldController.Instance.carGoDic[carForParking].transform.position.x, 
            WorldController.Instance.carGoDic[carForParking].transform.position.z)) > .1)
            carPos = Vector3.Lerp(mainCamera.transform.position, WorldController.Instance.carGoDic[carForParking].transform.position, 
                Time.deltaTime * speed);
        else
        {
            cameraLocked = true;
            carPos = WorldController.Instance.carGoDic[carForParking].transform.position;
        }
        carPos.y = enterOfCarPark.y;
		mainCamera.transform.position = carPos;

	}
}
