using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AI_UI : MonoBehaviour
{
	public TextMeshProUGUI generationNumberText;
	public TextMeshProUGUI currIndividualText;
	public TextMeshProUGUI speedText;
	public TextMeshProUGUI steerAngleText;
	public TextMeshProUGUI totalDistanceText;
    public Button changeSpeedButton;

    private void Start()
    {
        generationNumberText.color = Color.blue;
        currIndividualText.color = Color.blue;
        speedText.color = Color.blue;
        steerAngleText.color = Color.blue;
        totalDistanceText.color = Color.blue;
    }

    // Update is called once per frame
    void Update ()
	{
		generationNumberText.text = "Generation: " + GetComponent<AI_Trainer>().population.generationNumber;
		currIndividualText.text = "Population: " + GetComponent<AI_Trainer>().population.populationSize +"  Genome: "+(GetComponent<AI_Trainer>().population.currIndiv + 1) ;

		speedText.text = string.Format("Speed: {0:F2}", GetComponent<AI_DriveController>().GetSpeedOfCar());
		steerAngleText.text = string.Format("Steer Angle: {0:F2}", GetComponent<AI_DriveController>().steerAngle);
		totalDistanceText.text = string.Format("Distance: {0:F0}", GetComponent<AI_Trainer>().totalDist);
        changeSpeedButton.GetComponentInChildren<Text>().text =
            string.Format("{0:F0}x", GetComponent<AI_Trainer>().timeScale);
    }
}
