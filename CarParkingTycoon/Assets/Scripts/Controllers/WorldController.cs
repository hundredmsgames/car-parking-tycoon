using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
	public Dictionary<string, GameObject> carProtos;

	// Use this for initialization
	void Start ()
	{
        InitializeCarProtos();
	}

	void InitializeCarProtos()
	{
		carProtos = new Dictionary<string, GameObject>();
		GameObject[] protos = Resources.LoadAll<GameObject>("Prefabs");

		foreach(GameObject proto in protos)
		{
			carProtos.Add(proto.name, proto);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
