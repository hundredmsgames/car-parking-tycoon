using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public float maxTorque = 1000f;
	public float maxSteerAngle = 45f;

	public Transform centerOfMass;
	public WheelCollider[] wheelColliders;
	public Transform[] tireMeshes;

	Rigidbody m_rigidBody;

	void Start()
	{
		m_rigidBody = GetComponent<Rigidbody>();
		m_rigidBody.centerOfMass = centerOfMass.localPosition;
	}

	void FixedUpdate()
	{
		float horAxis = Input.GetAxis("Horizontal");
		float verAxis = Input.GetAxis("Vertical");

		float steerAngle = horAxis * maxSteerAngle;
		float accelerate = verAxis * maxTorque; 

		wheelColliders[0].steerAngle = steerAngle;
		wheelColliders[1].steerAngle = steerAngle;

		for(int i = 0; i < wheelColliders.Length; i++)
		{
			wheelColliders[i].motorTorque = accelerate;
		}
	}


	void Update ()
	{
		UpdateMeshesPositions();	
	}

	void UpdateMeshesPositions()
	{
		for(int i = 0; i < tireMeshes.Length; i++)
		{
			Quaternion quat;
			Vector3    pos;
			wheelColliders[i].GetWorldPose(out pos, out quat);

			tireMeshes[i].position = pos;
			tireMeshes[i].rotation = quat;
		}
	}
}
