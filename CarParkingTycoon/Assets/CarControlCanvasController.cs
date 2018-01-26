using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlCanvasController : MonoBehaviour {

    public static CarControlCanvasController Instance;

    Camera mainCamera;
	Car currCar;

	// Use this for initialization
	void Start ()
	{
        if (Instance != null)
            return;
    
		Instance = this;
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(0, 1.5f, 0);
        transform.rotation = mainCamera.transform.rotation;
    }

    public void Player()
    {
        GiveControl(Controller.Player); 
    }

    public void NPC()
    {
        GiveControl(Controller.NPC);
    }

    public void GiveControl(Controller controller)
    {
		Car carForParking = WorldController.Instance.world.carForParking;


        // We selected car is controlled car by player. We may not show take control button.
        // This is a temporary solution.
        if (carForParking !=null && currCar == carForParking && currCar.controller == controller)
        {
            Debug.Log("exit");
            ExitButton();
            return;
        }

        if ((carForParking != null && carForParking.getSpeedOfCar != null) &&
			carForParking.getSpeedOfCar() > 0.1f)
		{
            return;
		}
        else if((carForParking != null && carForParking.getSpeedOfCar != null) &&
            carForParking.getSpeedOfCar() <=0.1f)
        {
            if (currCar != carForParking)
            {
                currCar.controller = controller;
                carForParking.controller = Controller.None;
                WorldController.Instance.world.carForParking = currCar;
            }
            else
            {
                WorldController.Instance.world.carForParking.controller = controller;
            }
            Debug.Log(controller);
        }
        else
        {
            currCar.controller = controller;
            WorldController.Instance.world.carForParking = currCar;
        }

		

		
        
		ExitButton();
    }

	public void SetCurrentCarsDriverController(Car car)
    {
        currCar = car;
    }

    public void ExitButton()
    {
        transform.SetParent(null);
        if (gameObject.activeSelf == true )
            gameObject.SetActive(false);
     
    }
}
