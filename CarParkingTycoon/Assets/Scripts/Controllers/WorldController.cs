using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance;

	public Dictionary<string, GameObject> carPrefabs;
	public Dictionary<Car, GameObject> carGoDic;

	Transform[] spawnPoints;

   

    public LayerMask layerMask;
	public World world;

	// Use this for initialization
	void Awake()
	{
        if (Instance != null)
            return;

        Instance = this;
		carGoDic = new Dictionary<Car, GameObject>();

		InitalizeWorld();
        InitializeCarPrefabs();
		InitializeSpawnPositions();
	}

	void InitalizeWorld()
	{
		world = new World();
		world.RegisterOnCarSpawnedCallback(OnCarSpawned);
		world.RegisterGetNextSpawnPoint(GetNextSpawnPoint);
	}

	void InitializeCarPrefabs()
	{
		carPrefabs = new Dictionary<string, GameObject>();
		GameObject[] protos = Resources.LoadAll<GameObject>("Prefabs");

		foreach(GameObject proto in protos)
		{
			carPrefabs.Add(proto.name, proto);
          //  Debug.Log(proto.name);
		}
	}

	void InitializeSpawnPositions()
	{
		GameObject[] spawnPosGos = GameObject.FindGameObjectsWithTag("SpawnPoint");

		spawnPoints = new Transform[spawnPosGos.Length];
		for(int i = 0; i < spawnPoints.Length; i++)
			spawnPoints[i] = spawnPosGos[i].transform;
	}

	// Update is called once per frame
	void Update()
	{
		world.Update(Time.deltaTime);
    }

	void FixedUpdate()
	{
		world.PhysicsUpdate();
	}

	void OnCarSpawned(Car car, int spawnIndex)
	{
        if (carPrefabs.ContainsKey(car.name) == false)
            return;
		
		GameObject carGo = Instantiate<GameObject>(
			carPrefabs[car.name],
			spawnPoints[spawnIndex].position,
			spawnPoints[spawnIndex].rotation
		);

		carGo.transform.SetParent(gameObject.transform.GetChild(0));
		carGoDic.Add(car, carGo);

		var wheelDriveController = carGo.GetComponent<DriveController>();
		var parkingController    = carGo.GetComponent<ParkingController>();
		wheelDriveController.car = car;

		if (car.controller == Controller.NPC)
        {
            // Register NPC callbacks.
            car.RegisterIsCarGroundedFunc(wheelDriveController.IsCarGrounded);
            car.RegisterIsThereObstacleFunc(wheelDriveController.IsThereObstacle);
			car.RegisterGetSpeedOfCarFunc(wheelDriveController.GetSpeedOfCar);
            car.RegisterOnSetMotorTorque(wheelDriveController.SetMotorTorque);
            car.RegisterOnSetBrakeTorque(wheelDriveController.SetBrakeTorque);
            car.RegisterOnSetSteerAngle(wheelDriveController.SetSteerAngle);
			car.RegisterGetParkingInfo(parkingController.GetParkingInfo);
        }        
	}

	private int GetNextSpawnPoint()
	{
		List<int> spawnablePoints = new List<int>();

		for(int i = 0; i < spawnPoints.Length; i++)
		{
            // Vector3(1.5f, 1f, 1.5f) is enough to collide with cars at spawn point
            // TODO: We can make it a const.

           

            //this sphere will collide with only cars
			if(Physics.OverlapSphere(spawnPoints[i].position,3f,layerMask).Length == 0)
			{
				spawnablePoints.Add(i);			
			}
		}

		if(spawnablePoints.Count == 0)
			return -1;

		return Random.Range(0, spawnablePoints.Count);
	}
}
