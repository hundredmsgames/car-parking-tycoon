using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public static InputController Instance;

	[HideInInspector]
	public Vector3 mousePosition;

	[HideInInspector]
	public float horAxis;

	[HideInInspector]
	public float verAxis;

	[HideInInspector]
	public float spaceKeyAxis;

	[HideInInspector]
	public bool leftClick;

	[HideInInspector]
	public bool rightClick;

	// Use this for initialization
	void Start ()
	{
		if(Instance != null)
			return;
		Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
        InputsForCarParking();
		InputsForMouse();
	}

    void InputsForCarParking()
    {
        if (Input.GetKeyDown(KeyCode.N) == true)
            WorldController.Instance.world.NextCar();

        if (WorldController.Instance.world.carForParking != null)
        {
            horAxis = Input.GetAxis("Horizontal");
            verAxis = Input.GetAxis("Vertical");
            spaceKeyAxis = Input.GetAxis("Jump");
        }
    }

	void InputsForMouse()
	{
		leftClick     = Input.GetMouseButton(0);
		rightClick    = Input.GetMouseButton(1);
		mousePosition = Input.mousePosition;
	}
}
