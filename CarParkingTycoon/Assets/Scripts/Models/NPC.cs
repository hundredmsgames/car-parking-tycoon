using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC
{
	// The car that we control.
	public Car car;

	// This variable would be specific to car.
	float stoppingInterval;



	public NPC(Car car, float stoppingInterval)
	{
		this.car = car;
		this.stoppingInterval = stoppingInterval;
	}

	public void Update()
	{
		// If the car is not grounded, just return.
		if(car.isCarGrounded == null || car.isCarGrounded() == false)
			return;
		
		if(car.isThereObstacle != null)
		{
			// Ray toward forward 

			string obstacle = car.isThereObstacle(stoppingInterval);
            
            if (obstacle != null)
			{
                // If there is something, check what is this object's tag.
                // and act up to it

				if (car.onSetBrakeTorque != null)
					car.onSetBrakeTorque(car.maxBrakeTorque);

				if (car.onSetMotorTorque != null)
					car.onSetMotorTorque(0f);

                if (obstacle == "StopPoint")
                {
                    WorldController.Instance.world.AddCarToParkingQueue(car);
                }
			}
			else
			{
				// Otherwise move forward
				if(car.onSetBrakeTorque != null)
					car.onSetBrakeTorque(0f);

				if(car.onSetMotorTorque != null)
					car.onSetMotorTorque(car.maxMotorTorque / 2f);
			}
		}
	}
}
