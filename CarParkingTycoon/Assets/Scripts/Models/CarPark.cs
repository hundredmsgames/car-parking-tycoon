using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPark
{
	int capacity;
	int parkedCarCount;
    
	List<Car> carsInPark;
	List<ParkingSpace> parkingSpaces;

	Action<Car, ParkingSpace> onCarParked;

	// TODO: List of workers we own

	public CarPark()
	{
		carsInPark    = new List<Car>();
		parkingSpaces = new List<ParkingSpace>();
	}

	public void AddParkingSpace(ParkingSpace ps)
	{
		parkingSpaces.Add(ps);
		capacity++;
	}

	public void ParkCar(Car car, ParkingSpace ps)
	{
		carsInPark.Add(car);
		parkedCarCount++;

		if(onCarParked != null)
			onCarParked(car, ps);
	}

	public bool IsCarParked(Car car, ParkingSpace ps)
	{
		Vector3[] carInfo = null;

		if(car.getParkingInfo == null)
		{
			Debug.LogError("IsCarParked() -- Could not get parking info.");
			return false;
		}

		carInfo = car.getParkingInfo();

		float carLowerBoundX = carInfo[0].x - (carInfo[1].x / 2f);
		float carUpperBoundX = carInfo[0].x + (carInfo[1].x / 2f);
		float carLowerBoundZ = carInfo[0].z - (carInfo[1].z / 2f);
		float carUpperBoundZ = carInfo[0].z + (carInfo[1].z / 2f);

		return AreAllCornersInThePs(carInfo, ps);
	}

	// Checks if car's all corner's in the parking space.
	private bool AreAllCornersInThePs(Vector3[] carInfo, ParkingSpace ps)
	{
		float psHalfWidth  = ps.width / 2f;
		float psHalfHeight = ps.height / 2f;

		float psLowerX = ps.posX - psHalfWidth;
		float psUpperX = ps.posX + psHalfWidth;
		float psLowerZ = ps.posZ - psHalfHeight;
		float psUpperZ = ps.posZ + psHalfHeight;

		foreach(var info in carInfo)
		{
			if(IsPointInTheBox(info, psLowerX, psUpperX, psLowerZ, psUpperZ) == false)
				return false;
		}

		return true;
	}

	private bool IsPointInTheBox(Vector3 point, float lowerX, float upperX, float lowerZ, float upperZ)
	{
		return (lowerX < point.x && point.x < upperX &&	lowerZ < point.z && point.z < upperZ);
	}

	#region Register or unregister actions and funcs

	public void RegisterOnCarParked(Action<Car, ParkingSpace>  cb)
	{
		this.onCarParked += cb;
	}

	public void UnRegisterOnCarParked(Action<Car, ParkingSpace>  cb)
	{
		this.onCarParked -= cb;
	}

	#endregion
}
