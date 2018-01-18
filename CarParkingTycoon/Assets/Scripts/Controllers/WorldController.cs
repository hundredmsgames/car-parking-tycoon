using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance;

	public Dictionary<string, GameObject> carPrefabs;

	public Dictionary<Car, GameObject> carGoDic;

     Queue<Car> carsWaitingForParking;

	Transform[] spawnPositions;

	public World world;

    Car carForParking;
	// Use this for initialization
	void Start ()
	{
        if (Instance != null)
            return;

        Instance = this;
		carGoDic = new Dictionary<Car, GameObject>();
        carsWaitingForParking = new Queue<Car>();

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
          //  Debug.Log(proto.name);
		}
	}

	void InitializeSpawnPositions()
	{
		GameObject[] spawnPosGos = GameObject.FindGameObjectsWithTag("SpawnPoint");

		spawnPositions = new Transform[spawnPosGos.Length];
		for(int i = 0; i < spawnPositions.Length; i++)
			spawnPositions[i] = spawnPosGos[i].transform;
	}

    public void AddCarToParkingQueue(Car car)
    {
        if (carsWaitingForParking.Contains(car) == true)
            return;
        carsWaitingForParking.Enqueue(car);
    }
	// Update is called once per frame
	void Update()
	{
		world.Update(Time.deltaTime);

        if(carForParking == null && carsWaitingForParking!= null && carsWaitingForParking.Count > 0 )
        { 
            carForParking = carsWaitingForParking.Dequeue();
            
        }
        if(carForParking != null && carForParking.isParked == false)
        {
            carForParking.controlledByNPC = false;
        }
        if (carForParking != null && carForParking.isParked == true)
        {
            carForParking.controlledByNPC = true;
            carForParking = null;
        }

        if (Input.GetKeyDown(KeyCode.P) && carForParking != null)
            carForParking.isParked = true;

    }

	void OnCarSpawned(Car car)
	{
        if (carPrefabs.ContainsKey(car.name) == false)
            return;
		GameObject carGo = Instantiate<GameObject>(
			carPrefabs[car.name],
			spawnPositions[Random.Range(0, spawnPositions.Length)].position,
			spawnPositions[Random.Range(0, spawnPositions.Length)].rotation
		);

		carGo.transform.SetParent(gameObject.transform.GetChild(0));
		carGoDic.Add(car, carGo);

		var wheelDriveController = carGo.GetComponent<DriveController>();
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
