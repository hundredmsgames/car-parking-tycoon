using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	public Queue<Car> carQueue;

	public World()
	{
		carQueue = new Queue<Car>();
	}
}
