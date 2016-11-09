using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Reusable script for toggling between 2 panels w/ 2 toggles

public class Collect_AvailCreatePanelToggle : MonoBehaviour {

	public int panelDefault; //set in inspector, used to define whether panel is on/off at startup
	public Toggle toggle1; //toggle to activae panel 1
	public Toggle toggle2; //toggle to activae panel 2
	public GameObject panel1; //panel to be toggled
	public GameObject panel2; //panel to be toggled
	
	void Start () { //sets default

		if (panelDefault == 1)
		{
			panel1.SetActive(true);
			panel2.SetActive(false);
		}
		else if (panelDefault == 2)
		{
			panel1.SetActive(false);
			panel2.SetActive(true);
		}
	}
	
	public void TogglePanel(int panelNum) //toggles panel activation after toggle change 
	{
		if (panelNum == 1)
		{
			panel1.SetActive(true);
			panel2.SetActive(false);
		}
		else if (panelNum == 2)
		{
			panel1.SetActive(false);
			panel2.SetActive(true);
		}
	}
}
