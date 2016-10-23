using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//toggles between viewing artefact contextual media and viewing artefact contextual information

public class Import_InfoModeToggle : MonoBehaviour {

	public GameObject mediaControlToggles;
	public GameObject mediaPanel;
	public GameObject infoControlToggles;
	public GameObject infoPanel;

	void Start()
	{
		SwitchMode("info"); //default - should be 'info'
	}


	public void SwitchMode(string contextMode)
	{
		if (contextMode == "info")
		{
			infoControlToggles.SetActive(true);
			infoPanel.SetActive(true);

			mediaControlToggles.SetActive(false);
			mediaPanel.SetActive(false);
		}
		else if (contextMode == "media")
		{
			infoControlToggles.SetActive(false);
			infoPanel.SetActive(false);

			mediaControlToggles.SetActive(true);
			mediaPanel.SetActive(true);
		}	
	}
}
