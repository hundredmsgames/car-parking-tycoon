using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	public Dictionary<string, Car> carProtos;

	public CarPark carPark;

	public List<Car> spawnedCars;
	Queue<Car> carsWaitingForParking;

	public Car carForParking;

	float carSpawnInterval = 10;
	float carSpawnTime     = 0f;

	// We may need to limit car count in the world.
	int maxCarCount = 20;

	Action<Car, int> onCarSpawned;
	Func<int> getNextSpawnPoint;

	public World()
	{
		spawnedCars = new List<Car>();
		carsWaitingForParking = new Queue<Car>();

		carPark = new CarPark();

		InitializeCarProtos();
	}

	void InitializeCarProtos()
	{
		carProtos = new Dictionary<string, Car>();

        carProtos.Add(
            "TempCar",
            new Car(
                "TempCar",  		// car name
                40f,                // car max speed
				600f,       		// maxTorque
				2000f,      		// maxBreakeTorque
				30f,        		// steering angle
				100f,       		// damage percent
				10f,        		// damage per hit ( DPH )
				100,        		// price
				false,      		// is parked
				Controller.NPC		// controlled by NPC 
			)
		);
	}

	public void AddCarToParkingQueue(Car car)
	{
		// FIXME: This control will be done every frame
		// We can use Dictionary or we can reduce raycast
		// interval. Maybe 100ms interval.
		if (carsWaitingForParking.Contains(car) == true)
			return;

		carsWaitingForParking.Enqueue(car);
	}

	public void Update(float deltaTime)
	{
		if(carSpawnTime <= 0f)
		{
			SpawnCar("TempCar");
			carSpawnTime = carSpawnInterval;
			return;
		}

		carSpawnTime -= deltaTime;
	}

	public void PhysicsUpdate()
	{
		UpdateSpawnedCars();
	}

	void UpdateSpawnedCars()
	{
		foreach(Car car in spawnedCars)
			car.Update();
	}

	void SpawnCar(string carName)
	{
		// TODO: If there is a car at the spawn point we cannot
		// spawn new cars. So we need a Func here...
		// Find a new spawn position and return.
		// If there is none, do not spawn new car.

		// We have reached the max car count in the world.
		// So do not spawn any car.
		if(spawnedCars.Count >= maxCarCount)
			return;

		if(getNextSpawnPoint == null)
			return;

		int spawnIndex = getNextSpawnPoint();

		// There is no appropriate spawn point in the world.
		if(spawnIndex == -1)
			return;

		Car spawnedCar = carProtos[carName].Clone();

		if (spawnedCar.controller == Controller.NPC)
		{
			spawnedCar.npc = new NPC(spawnedCar, 7f);
		}

		spawnedCars.Add(spawnedCar);

		if(onCarSpawned != null)
			onCarSpawned(spawnedCar, spawnIndex);
	}

	public void NextCar()
	{
		if(carForParking != null || carsWaitingForParking == null || carsWaitingForParking.Count == 0)
			return;

		carForParking = carsWaitingForParking.Dequeue();
		carForParking.controller = Controller.Player;
	}

	public void ParkCar(ParkingSpace ps)
	{
		// If position of the car is appropriate for park
		// For now, anywhere is appropriate for park but the car
		// should be stopped.

		if(carForParking == null || carForParking.getSpeedOfCar == null ||
			carForParking.getSpeedOfCar() > 0.1f)
		{
			return;
		}

		if(carPark.IsCarParked(carForParking, ps) == true)
		{
			carForParking.ParkCar(ps);
			carForParking = null;
		}
	}

	public void RegisterOnCarSpawnedCallback(Action<Car, int> cb)
	{
		this.onCarSpawned += cb;
	}

	public void UnRegisterOnCarSpawnedCallback(Action<Car, int> cb)
	{
		this.onCarSpawned -= cb;
	}

	public void RegisterGetNextSpawnPoint(Func<int> func)
	{
		this.getNextSpawnPoint += func;
	}

	public void UnRegisterGetNextSpawnPoint(Func<int> func)
	{
		this.getNextSpawnPoint -= func;
	}
}
