using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed = 3f;
    Vector3 enterOfCarPark;
	Camera mainCamera;

    //camera drag drop
    public float dragSpeed = 3f;
    private Vector3 dragOrigin;


    //is camera locked on the target?
    bool cameraLocked;

	void Start()
	{
		mainCamera = Camera.main;
		enterOfCarPark = mainCamera.transform.position;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
       
        }

        //if I release button return
        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y* dragSpeed);

        mainCamera.transform.Translate(move, Space.World);

        //dragOrigin = Input.mousePosition;
    }


    void LateUpdate()
	{
		Car carForParking = WorldController.Instance.world.carForParking;
		Vector3 cameraPos = mainCamera.transform.position;

		if(carForParking == null)
		{
            cameraLocked = false;
            //mainCamera.transform.position = enterOfCarPark;
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
