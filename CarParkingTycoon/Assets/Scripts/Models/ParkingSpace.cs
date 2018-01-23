using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSpace
{
	Car parkedCar;

	float posX;
	float posZ;
	float width;
	float height;

	public bool occupied;

	public ParkingSpace(float posX, float posZ, float width, float height)
	{
		this.posX = posX;
		this.posZ = posZ;
		this.width = width;
		this.height = height;
	}
}
