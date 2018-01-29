using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 3f;
    Vector3 enterOfCarPark;
	Camera mainCamera;

    // camera drag drop
	Vector3 currFramePosition;
	Vector3 lastFramePosition;
	Vector3 mousePosition;

	// This is a factor for dragging.
	// We multiply this factor by camera's y-axis.
	// It changes when camera zooms in-out.
	float dragSpeed = 0.75f;

    //is camera locked on the target?
    bool cameraLocked;

	void Start()
	{
		mainCamera = Camera.main;
		enterOfCarPark = mainCamera.transform.position;
    }

	void LateUpdate()
	{
		mousePosition = InputController.Instance.mousePosition;

		// Distance from ground
		mousePosition.z = mainCamera.transform.position.y * dragSpeed;

		currFramePosition = mainCamera.ScreenToWorldPoint(mousePosition);
		currFramePosition.y = 0f;

		Car carForParking = WorldController.Instance.world.carForParking;

		if(carForParking == null)
		{
			cameraLocked = false;
			UpdateCameraMovement();
		}
		else
		{
			FocusCar(WorldController.Instance.carGoDic[carForParking]);
		}

		lastFramePosition = mainCamera.ScreenToWorldPoint(mousePosition);
		lastFramePosition.y = 0f;
	}

	void UpdateCameraMovement()
	{
		// Handle screen panning
		if(InputController.Instance.rightClick == true)
		{
			Vector3 diff = lastFramePosition - currFramePosition;
			mainCamera.transform.Translate(diff, Space.World);
		}

		float cameraPosY = mainCamera.transform.position.y;

		cameraPosY -= cameraPosY * Input.GetAxis("Mouse ScrollWheel");
		cameraPosY = Mathf.Clamp(cameraPosY, 15f, 25f);

		mainCamera.transform.position = new Vector3(
			mainCamera.transform.position.x,
			cameraPosY,
			mainCamera.transform.position.z
		);
	}

	void FocusCar(GameObject carForParkingGo)
	{
		Vector3 carPos    = carForParkingGo.transform.position;
		Vector3 cameraPos = mainCamera.transform.position;
		carPos.y    = 0f;
		cameraPos.y = 0f;

		if (cameraLocked == false && (carPos - cameraPos).magnitude > 0.1f)
			carPos = Vector3.Lerp(cameraPos, carPos, Time.deltaTime * cameraSpeed);
		else
			cameraLocked = true;

		carPos.y = enterOfCarPark.y;
		mainCamera.transform.position = carPos;
	}
}
