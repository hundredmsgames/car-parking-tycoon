using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car
{
	// Name is the brand
	public string name;

	public NPC npc;

	// Max motor torque
	public float maxMotorTorque;

	// Max brake torque
	public float maxBrakeTorque;

	// Max steer angle
	public float maxSteerAngle;

	// %60 damaged
	float damagePercent;

	// sensivity of car
    float damageTakePerHit;

    // if we want to buy or cell cars this will be usefull
    int price;

	bool isParked;

	public bool controlledByNPC;

	// if you want to see specific car info you can add as much as you want
    // we can think about adding smoke particule over decent amount of damage

    // maybe fuel

    // TODO : think about adding events or callbacks to appropriate variables that we might need(explain with a good reason)


	public Car(string name, float maxMotorTorque, float maxBrakeTorque, float maxSteerAngle,
		float damagePercent, float damageTakePerHit, int price, bool isParked, bool controlledByNPC = true)
	{
		this.name             = name;
		this.maxMotorTorque   = maxMotorTorque;
		this.maxBrakeTorque   = maxBrakeTorque;
		this.maxSteerAngle    = maxSteerAngle;
		this.damagePercent    = damagePercent;
		this.damageTakePerHit = damageTakePerHit;
		this.price            = price;
		this.isParked         = isParked;
		this.controlledByNPC  = controlledByNPC;
	}

	public Car Clone()
	{
		Car car = new Car(
			this.name,
			this.maxMotorTorque,
			this.maxBrakeTorque,
			this.maxSteerAngle,
			this.damagePercent,
			this.damageTakePerHit,
			this.price,
			this.isParked,
			this.controlledByNPC
		);

		return car;
	}

	public void Update()
	{
		// If the car is not controlled by npc, just return.
		if(controlledByNPC == false)
			return;
		
		npc.Update();
	}
}
