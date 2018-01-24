using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSystemController : MonoBehaviour {

    DriveController driveController = null;
    
    // Use this for initialization
    void Start () {
        driveController = GetComponent<DriveController>();
    }
	
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
                if (driveController != null)
                {
                    driveController.car.controller = Controller.Player;

                    if (WorldController.Instance.world.carForParking != null)
                        WorldController.Instance.world.carForParking.controller = Controller.None;

                    WorldController.Instance.world.carForParking = driveController.car;

                }
            }

        }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Obstacle")
        {
            WorldController.Instance.world.carForParking.TakeDamage();
        }
    }
}
