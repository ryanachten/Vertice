using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//toggles between viewing artefact contextual media and viewing artefact contextual information

public class ContextInfoModeToggle : MonoBehaviour {

	public GameObject mediaControlToggles;
	public GameObject mediaPanel;
	public Toggle mediaToggle;
	public GameObject imageToggle;
	public GameObject audioToggle;
	public GameObject videoToggle;

	public GameObject infoControlToggles;
	public GameObject infoPanel;
	public Toggle infoToggle;

	void Start()
	{
		#if UNITY_WEBGL
		imageToggle.SetActive(false);
		audioToggle.SetActive(false);
		videoToggle.SetActive(false);
		#endif

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
