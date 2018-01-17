using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform car;

	void Update ()
	{
		Camera.main.transform.position = new Vector3(car.position.x, Camera.main.transform.position.y, car.position.z);
	}
}
