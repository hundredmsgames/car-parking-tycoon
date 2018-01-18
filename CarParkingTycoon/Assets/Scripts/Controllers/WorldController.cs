using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
	public Dictionary<string, GameObject> carPrefabs;

	public Dictionary<Car, GameObject> carGoDic;

	Transform[] spawnPositions;

	public World world;

	// We may need to limit car count in the world.
	int maxCarCount;

	// Use this for initialization
	void Start ()
	{
		carGoDic = new Dictionary<Car, GameObject>();
		InitalizeWorld();
        InitializeCarPrefabs();
		InitializeSpawnPositions();
	}

	void InitalizeWorld()
	{
		world = new World();
		world.RegisterOnCarSpawnedCallback(OnCarSpawned);
	}

	void InitializeCarPrefabs()
	{
		carPrefabs = new Dictionary<string, GameObject>();
		GameObject[] protos = Resources.LoadAll<GameObject>("Prefabs");

		foreach(GameObject proto in protos)
		{
			carPrefabs.Add(proto.name, proto);
		}
	}

	void InitializeSpawnPositions()
	{
		GameObject[] spawnPosGos = GameObject.FindGameObjectsWithTag("SpawnPoint");

		spawnPositions = new Transform[spawnPosGos.Length];
		for(int i = 0; i < spawnPositions.Length; i++)
			spawnPositions[i] = spawnPosGos[i].transform;
	}

	// Update is called once per frame
	void Update()
	{
		world.Update(Time.deltaTime);
	}

	void OnCarSpawned(Car car)
	{
		GameObject carGo = Instantiate(
			carPrefabs[car.name],
			spawnPositions[Random.Range(0, spawnPositions.Length)].position,
			spawnPositions[Random.Range(0, spawnPositions.Length)].rotation
		);

		carGo.transform.SetParent(gameObject.transform.GetChild(0));
		carGoDic.Add(car, carGo);

		var wheelDriveController = carGo.GetComponent<WheelDriveController>();
		wheelDriveController.car = car;

        if (car.controlledByNPC == true)
        {
            // Register NPC callbacks.
            car.npc.RegisterIsCarGroundedFunc(wheelDriveController.IsCarGrounded);
            car.npc.RegisterIsThereObstacleFunc(wheelDriveController.IsThereObstacle);
            car.npc.RegisterOnSetMotorTorque(wheelDriveController.SetMotorTorque);
            car.npc.RegisterOnSetBrakeTorque(wheelDriveController.SetBrakeTorque);
            car.npc.RegisterOnSetSteerAngle(wheelDriveController.SetSteerAngle);
        }
	}
}
