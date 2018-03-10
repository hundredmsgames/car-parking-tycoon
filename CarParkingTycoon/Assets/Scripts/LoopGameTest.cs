using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopGameTest : MonoBehaviour
{
	public GameObject carPrefab;
	GameObject car;
	List<GameObject> loopCars;

	List<List<float>[]> loopCarMoves;
	List<List<float>[]> tempMoves;
	List<float>[] keyPresses = new List<float>[4];

	bool[] keyStates;

	bool[] loopKeyStates;

	float startTime;
	float looperStartTime;

	// Use this for initialization
	void Start ()
	{
		car = Instantiate(carPrefab);
		loopCars = new List<GameObject>();

		for(int i = 0; i < keyPresses.Length; i++)
			keyPresses[i] = new List<float>();

		keyStates = new bool[4];
		loopKeyStates = new bool[4];
		loopCarMoves = new List<List<float>[]>();

		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		ControlLoopers();

		if(Input.GetKey(KeyCode.UpArrow) != keyStates[0])
		{
			keyStates[0] = !keyStates[0];
			keyPresses[0].Add(Time.time - startTime);
		}
		if(Input.GetKey(KeyCode.LeftArrow) != keyStates[1])
		{
			keyStates[1] = !keyStates[1];
			keyPresses[1].Add(Time.time - startTime);
		}
		if(Input.GetKey(KeyCode.DownArrow) != keyStates[2])
		{
			keyStates[2] = !keyStates[2];
			keyPresses[2].Add(Time.time - startTime);
		}
		if(Input.GetKey(KeyCode.RightArrow) != keyStates[3])
		{
			keyStates[3] = !keyStates[3];
			keyPresses[3].Add(Time.time - startTime);
		}

		if(Input.GetKeyDown(KeyCode.E) == true)
		{
			Destroy(car);
			foreach(GameObject g in loopCars)
			{
				Destroy(g);
			}

			loopCarMoves.Add(keyPresses);
			tempMoves = new List<List<float>[]>(loopCarMoves);
			loopKeyStates = new bool[4];

			car = Instantiate(carPrefab);

			looperStartTime = Time.time;

			// Instaiate a new car
			// remove camera from this car
			// make setActive(false) this car
		}
	}

	int spawnedCars;
	float spawnTime = 5f;

	void ControlLoopers()
	{
		if(spawnedCars < loopCarMoves.Count)
		{
			if(Time.time - looperStartTime > spawnTime)
			{
				spawnTime += 5f;
				spawnedCars++;
				GameObject lCar = Instantiate(carPrefab);
				lCar.GetComponent<LoopDriveController>().loopCar = true;
				Destroy(lCar.transform.Find("Main Camera").gameObject);
				loopCars.Add(lCar);
			}
		}

		for(int i = 0; i < spawnedCars; i++)
		{
			LoopDriveController ldc = loopCars[i].GetComponent<LoopDriveController>();

			if(tempMoves[i][0].Count > 0 && Time.time - looperStartTime > tempMoves[i][0][0])
			{
				loopKeyStates[0] = !loopKeyStates[0];
				tempMoves[i][0].RemoveAt(0);
			}
			if(tempMoves[i][1].Count > 0 && Time.time - looperStartTime > tempMoves[i][1][0])
			{
				loopKeyStates[1] = !loopKeyStates[1];
				tempMoves[i][1].RemoveAt(0);
			}
			if(tempMoves[i][2].Count > 0 && Time.time - looperStartTime > tempMoves[i][2][0])
			{
				loopKeyStates[2] = !loopKeyStates[2];
				tempMoves[i][2].RemoveAt(0);
			}
			if(tempMoves[i][3].Count > 0 && Time.time - looperStartTime > tempMoves[i][3][0])
			{
				loopKeyStates[3] = !loopKeyStates[3];
				tempMoves[i][3].RemoveAt(0);
			}

			float motorTorque = (loopKeyStates[0] == true ? 1 : 0);
			motorTorque -= (loopKeyStates[2] == true ? 1 : 0);

			float steerAngle = (loopKeyStates[1] == true ? 1 : 0);
			steerAngle -= (loopKeyStates[3] == true ? 1 : 0);

			ldc.SetMotorTorque(motorTorque);
			ldc.SetSteerAngle(steerAngle);
		}
	}
}
