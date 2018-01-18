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

	Func<bool> isCarGrounded;
	Func<float, string> isThereObstacle;
	Action<float> onSetMotorTorque;
	Action<float> onSetBrakeTorque;
	Action<float> onSetSteerAngle;

	public NPC(Car car, float stoppingInterval)
	{
		this.car = car;
		this.stoppingInterval = stoppingInterval;
	}

	public void Update()
	{
		// If the car is not grounded, just return.
		if(isCarGrounded == null || isCarGrounded() == false)
			return;
		
		if(isThereObstacle != null)
		{
            string obstacle = isThereObstacle(stoppingInterval);
            // Ray toward forward 
            if (obstacle!= null)
			{
                // If there is something, check what is this object's tag.
                // and act up to it

                if (onSetBrakeTorque != null)
                    onSetBrakeTorque(car.maxBrakeTorque);

                if (onSetMotorTorque != null)
                    onSetMotorTorque(0f);

                if (obstacle == "StopPoint")
                {
                    WorldController.Instance.AddCarToParkingQueue(car);
                    
                }

				
			}
			else
			{
				// Otherwise move forward
				if(onSetBrakeTorque != null)
					onSetBrakeTorque(0f);

				if(onSetMotorTorque != null)
					onSetMotorTorque(car.maxMotorTorque / 2f);
			}
		}
	}

	#region Register or unregister actions and funcs

	public void RegisterIsCarGroundedFunc(Func<bool> func)
	{
		this.isCarGrounded += func;
	}

	public void UnRegisterIsCarGroundedFunc(Func<bool> func)
	{
		this.isCarGrounded -= func;
	}

	public void RegisterIsThereObstacleFunc(Func<float, string> func)
	{
		this.isThereObstacle += func;
	}

	public void UnRegisterIsThereObstacleFunc(Func<float, string> func)
	{
		this.isThereObstacle -= func;
	}

	public void RegisterOnSetMotorTorque(Action<float> cb)
	{
		this.onSetMotorTorque += cb;
	}

	public void UnRegisterOnSetMotorTorque(Action<float> cb)
	{
		this.onSetMotorTorque -= cb;
	}

	public void RegisterOnSetBrakeTorque(Action<float> cb)
	{
		this.onSetBrakeTorque += cb;
	}

	public void UnRegisterOnSetBrakeTorque(Action<float> cb)
	{
		this.onSetBrakeTorque -= cb;
	}

	public void RegisterOnSetSteerAngle(Action<float> cb)
	{
		this.onSetSteerAngle += cb;
	}

	public void UnRegisterOnSetSteerAngle(Action<float> cb)
	{
		this.onSetSteerAngle -= cb;
	}

	#endregion
}
