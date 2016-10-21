using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Import_InfoPanelToggle : MonoBehaviour {

	public GameObject artefactPanel;
	public GameObject contextPanel;
	public GameObject originalPanel;
	public GameObject mediaPanel;
	public GameObject meshPanel;

	void Start()
	{
		TogglePanels("artefact");
	}


	public void TogglePanels( string infoMode)
	{
		switch (infoMode) {

		case "artefact" :
			artefactPanel.SetActive(true);
			contextPanel.SetActive(false);
			originalPanel.SetActive(false);
			mediaPanel.SetActive(false);
			meshPanel.SetActive(false);
			break;

		case "context" :
			artefactPanel.SetActive(false);
			contextPanel.SetActive(true);
			originalPanel.SetActive(false);
			mediaPanel.SetActive(false);
			meshPanel.SetActive(false);
			break;

		case "original" :
			artefactPanel.SetActive(false);
			contextPanel.SetActive(false);
			originalPanel.SetActive(true);
			mediaPanel.SetActive(false);
			meshPanel.SetActive(false);
			break;

		case "media" :
			artefactPanel.SetActive(false);
			contextPanel.SetActive(false);
			originalPanel.SetActive(false);
			mediaPanel.SetActive(true);
			meshPanel.SetActive(false);
			break;

		case "mesh" :
			artefactPanel.SetActive(false);
			contextPanel.SetActive(false);
			originalPanel.SetActive(false);
			mediaPanel.SetActive(false);
			meshPanel.SetActive(true);
			break;

		default:
			break;
		}
	}
}
