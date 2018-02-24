using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;

public class AI_Trainer : MonoBehaviour
{
	public Population population;
	NeuralNetwork.NeuralNetwork currNN;

	AI_DriveController driveController;
	Transform raycastPoint;
	RaycastHit[] sensors;
	GameObject environments;
	GameObject[] lines;

	Vector3 startPosition;
	Quaternion startRotation;

	Vector3 currCarPos;
	Vector3 lastCarPos;
	public float totalDist;
	public float timePassed;

	public float timeScale = 1f;

	float rayDist = 7;

	// Use this for initialization
	void Start ()
	{
		population = new Population(10, new int[]{3, 100, 2}, 1f);

		raycastPoint = transform.Find("RaycastPoint");	
		environments = GameObject.Find("Environments");
		driveController = GetComponent<AI_DriveController>();

		startPosition = transform.position;
		startRotation = transform.rotation;

		currCarPos = lastCarPos = startPosition;

		currNN = population.Next();
	}

    public void NewGenome()
    {
        OnCollisionEnter();
    }
	// Update is called once per frame
	void Update ()
	{
		Time.timeScale = timeScale;

		if(driveController.GetSpeedOfCar() < 0.01)
			OnCollisionEnter();

		sensors = new RaycastHit[3];

		Physics.Raycast(raycastPoint.position, raycastPoint.forward, out sensors[0], rayDist);
		Physics.Raycast(raycastPoint.position, raycastPoint.forward - raycastPoint.right, out sensors[1], rayDist);
		Physics.Raycast(raycastPoint.position, raycastPoint.forward + raycastPoint.right, out sensors[2], rayDist);

		DrawSensorLines();

		float forward, left, right;
		forward = left = right = rayDist;

		if(sensors[0].collider != null)
			forward = sensors[0].distance;

		if(sensors[1].collider != null)
			left = sensors[1].distance;

		if(sensors[2].collider != null)
			right = sensors[2].distance;


		double[] inputs = new double[3];
		inputs[0] = (2f / rayDist) * forward - 1f;
		inputs[1] = (2f / rayDist) * left - 1f;
		inputs[2] = (2f / rayDist) * right - 1f;

		double[] outputs;
		outputs = currNN.FeedForward(inputs);

		driveController.SetMaxSpeed((float) outputs[0]);
		driveController.SetSteerAngle((float) outputs[1]);
		driveController.SetMotorTorque(1f);

		currCarPos = transform.position;
		totalDist += Vector3.Distance(currCarPos, lastCarPos);
		lastCarPos = currCarPos;

		timePassed += Time.deltaTime;
	}

	void OnCollisionEnter()
	{
		population.SetFitnessOfCurrIndividual(totalDist, timePassed);
		currNN = population.Next();
		ResetCarPosition();
	}

	void ResetCarPosition()
	{
		transform.position = startPosition;
		transform.rotation = startRotation;
		currCarPos = startPosition;
		lastCarPos = startPosition;

		driveController.SetMotorTorque(0f);
		totalDist = 0f;
		timePassed = 0f;
	}

	void DrawSensorLines()
	{
		Color middleSensorColor, leftSensorColor, rightSensorColor;
		middleSensorColor = (sensors[0].collider == null) ? Color.green : Color.red;
		leftSensorColor   = (sensors[1].collider == null) ? Color.green : Color.red;
		rightSensorColor  = (sensors[2].collider == null) ? Color.green : Color.red;

		if(lines == null)
		{
			lines = new GameObject[3];
			DrawLine(
				raycastPoint.position,
				(raycastPoint.position + raycastPoint.forward * rayDist), 
				middleSensorColor,
				0
			);

			DrawLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward - raycastPoint.right) * rayDist),
				leftSensorColor,
				1
			);

			DrawLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward + raycastPoint.right) * rayDist),
				rightSensorColor,
				2
			);
		}
		else
		{
			UpdateLine(
				raycastPoint.position,
				(raycastPoint.position + raycastPoint.forward * rayDist), 
				middleSensorColor,
				0
			);

			UpdateLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward - raycastPoint.right).normalized * rayDist),
				leftSensorColor,
				1
			);

			UpdateLine(
				raycastPoint.position,
				(raycastPoint.position + (raycastPoint.forward + raycastPoint.right).normalized * rayDist),
				rightSensorColor,
				2
			);
		}

	}
		
	void DrawLine(Vector3 start, Vector3 end, Color color, int lineIndex)
	{
		GameObject line = new GameObject();
		line.name = "Line " + lineIndex;
		line.transform.SetParent(environments.transform);

		line.transform.position = start;
		line.AddComponent<LineRenderer>();
		LineRenderer lr = line.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.startColor = color;
		lr.endColor = color;
		lr.startWidth = 0.05f;
		lr.endWidth = 0.05f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);

		lines[lineIndex] = line;
	}

	void UpdateLine(Vector3 start, Vector3 end, Color color, int lineIndex)
	{
		LineRenderer lr = lines[lineIndex].GetComponent<LineRenderer>();
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
	}
}
