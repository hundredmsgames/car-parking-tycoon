using UnityEngine;
using System.Collections;

public class WheelDriveController : MonoBehaviour
{
	private WheelCollider[] wheels;

	private Rigidbody rigBody;
	public float maxAngle = 30f;
	public float maxTorque = 600f;
	public float brakeTorque = 2000f;
	public GameObject wheelShape;

	float angle;
	float torque;
	float brake;

	// Drag variables. These are used to slow down the car
	// when there is no torque on it.
	float lowerSpeedLimitForDrag = 0f;
	float upperSpeedLimitForDrag = 15f;
	float lowerDragLimit = 0.1f;
	float upperDragLimit = 0.9f;

	// here we find all the WheelColliders down in the hierarchy
	public void Start()
	{
		rigBody = GetComponent<Rigidbody>();
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	public void Update()
	{
		angle  = maxAngle * Input.GetAxis("Horizontal");
		torque = maxTorque * Input.GetAxis("Vertical");
		brake  = brakeTorque * Input.GetAxis("Jump");

		foreach (WheelCollider wheel in wheels)
		{
			// update visual wheels if any
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}
	}

	void FixedUpdate()
	{
		float speed = rigBody.velocity.magnitude * 3.6f;
		rigBody.drag = Mathf.Clamp(((upperSpeedLimitForDrag - speed) / upperSpeedLimitForDrag) *
			(upperDragLimit - lowerDragLimit) + lowerDragLimit, lowerDragLimit, upperDragLimit);

		foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer while rear ones drive
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			if(wheel.transform.localPosition.z < 0)
				wheel.motorTorque = torque;

			wheel.brakeTorque = brake;
		}
	}
}
