using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DriveController : MonoBehaviour
{
	private WheelCollider[] wheels;
	private Rigidbody rigBody;
	public GameObject wheelShapePrefab;

	// Curr motor torque
	float motorTorque;

	float maxMotorTorque = 900f;

	// Curr steer angle
	public float steerAngle;

	float maxSteerAngle = 30f;

	float currentSpeed;
	public float desiredSpeed;

	float maxSpeed = 40f;
    float minSpeed = 20f;

	// Drag variables. These are used to slow down the car
	// when there is no torque on it.
	float upperSpeedLimitForDrag = 10f;
	float lowerDragLimit = 0.1f;
	float upperDragLimit = 0.9f;

	// Use this for initialization
	void Start ()
	{
		rigBody = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelCollider>();

		// create wheel shapes only when needed
		if (wheelShapePrefab != null)
		{
			foreach (var wheel in wheels)
			{
				var ws = GameObject.Instantiate(wheelShapePrefab, wheel.transform);
			}
		}	
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (WheelCollider wheel in wheels)
		{
			// update visual wheels if any
			if (wheelShapePrefab)
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose(out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild(0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}	
	}

	void FixedUpdate()
	{
		rigBody.drag = Mathf.Clamp(((upperSpeedLimitForDrag - currentSpeed) / upperSpeedLimitForDrag) *
			(upperDragLimit - lowerDragLimit) + lowerDragLimit, lowerDragLimit, upperDragLimit);
		
		foreach (WheelCollider wheel in wheels)
		{
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = steerAngle;

			if (currentSpeed < desiredSpeed)
			{
				if (wheel.transform.localPosition.z < 0)
					wheel.motorTorque = motorTorque;
			}
			else
			{
				wheel.motorTorque = 0;
			}
		}

		currentSpeed = GetSpeedOfCar();
	}

	public void SetMotorTorque(float motorTorque)
	{
		this.motorTorque = motorTorque * maxMotorTorque;
	}

	public void SetMaxSpeed(float maxSpeed)
	{
		this.desiredSpeed = Mathf.Lerp(minSpeed , this.maxSpeed, (maxSpeed + 1f) / 2f);
	}

	public void SetSteerAngle(float steerAngle)
	{
		this.steerAngle = Mathf.Lerp(-maxSteerAngle, maxSteerAngle, (steerAngle + 1f) / 2f);
	}

	public float GetSpeedOfCar()
	{
		return Mathf.Abs(rigBody.velocity.magnitude * 3.6f);
	}
}
