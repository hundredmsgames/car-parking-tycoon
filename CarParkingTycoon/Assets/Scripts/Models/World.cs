using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	public static World Instance;

	public List<Car> spawnedCars;

	public Dictionary<string, Car> carProtos;

	float carSpawnInterval = 5f;
	float carSpawnTime     = 0f;

	Action<Car> onCarSpawned;

	public World()
	{
		if(Instance != null)
			return;
		
		Instance = this;
		
		spawnedCars = new List<Car>();

		InitializeCarProtos();
	}

	void InitializeCarProtos()
	{
		carProtos = new Dictionary<string, Car>();

		carProtos.Add(
			"TempCar",
			new Car(
				"TempCar",
				600f,
				2000f,
				30f,
				100f,
				10f,
				100,
				false
			)
		);
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
		
		Car spawnedCar = carProtos[carName].Clone();
		spawnedCar.npc = new NPC(spawnedCar, 7f);
		spawnedCars.Add(spawnedCar);

		if(onCarSpawned != null)
			onCarSpawned(spawnedCar);
	}

	public void RegisterOnCarSpawnedCallback(Action<Car> cb)
	{
		this.onCarSpawned += cb;
	}

	public void UnRegisterOnCarSpawnedCallback(Action<Car> cb)
	{
		this.onCarSpawned -= cb;
	}
}
