using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPark
{
	int capacity;
	int parkedCarCount;
    
	List<Car> carsInPark;
	List<ParkingSpace> parkingSpaces;

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

	public bool IsCarParked(Car car, ParkingSpace ps)
	{
		// TODO: We need to evaluate that whether the car is parked or not.

		// TODO: If car is parked. We need to disable indicator of park space.
		// So we need a Action here.

		ps.occupied = true;
		return true;
	}

}
