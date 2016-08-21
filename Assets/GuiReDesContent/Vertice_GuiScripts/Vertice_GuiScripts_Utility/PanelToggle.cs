using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Reusable script for toggling panels on and off

public class PanelToggle : MonoBehaviour {

	public Toggle toggle; //toggle controlling whether panel is active
	public bool panelDefault; //set in inspector, used to define whether panel is on/off at startup
	public GameObject panel; //panel to be toggled
	
	void Start () { //sets default

		if (panel.activeSelf != panelDefault)
		{
			panel.SetActive(panelDefault);
		}

	}
	
	public void TogglePanel() //toggles panel activation after toggle change
	{
		if (toggle.isOn)
			panel.SetActive(true);

		else
			panel.SetActive(false);
	}
}
