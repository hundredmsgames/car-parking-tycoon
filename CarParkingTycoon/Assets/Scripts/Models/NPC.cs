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
	Func<float, bool> isThereObstacle;
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
			// Ray toward forward 
			if(isThereObstacle(stoppingInterval) == true)
			{
				// If there is something, stop the car.

				if(onSetBrakeTorque != null)
					onSetBrakeTorque(car.maxBrakeTorque);

				if(onSetMotorTorque != null)
					onSetMotorTorque(0f);
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

	public void RegisterIsThereObstacleFunc(Func<float, bool> func)
	{
		this.isThereObstacle += func;
	}

	public void UnRegisterIsThereObstacleFunc(Func<float, bool> func)
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
