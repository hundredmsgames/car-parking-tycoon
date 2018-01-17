using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
	public Rigidbody car;
	public TextMeshProUGUI speedText;

	// Update is called once per frame
	void Update ()
	{
		speedText.text = string.Format("Speed: {0:F0} KPH", car.velocity.magnitude * 3.6f);
	}
}
