using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Browse_SearchMode : MonoBehaviour {

	public Browse_BrowseControl BrowseControl;
	public Transform toggleParent;
	private List<string> activeModes;

	public GameObject searchPreviewText;


	public void GetSearchModes()
	{
		activeModes = new List<string> ();
		for (int i = 0; i < toggleParent.childCount; i++) 
		{
			Toggle curToggle = toggleParent.GetChild (i).GetComponent<Toggle>();	
			if (curToggle.isOn) 
			{
				string toggleName = toggleParent.GetChild (i).GetComponentInChildren<Text>().text;	
				activeModes.Add (toggleName);
			}
		}
//		for (int i = 0; i < activeModes.Count; i++) {
//
//			Debug.Log (activeModes[i]);
//		}
	}

	public void SearchPreview()
	{
		if (!searchPreviewText.activeSelf) {
			searchPreviewText.SetActive (true);
		}
		//TODO relevantArtefacts = DCReader.*GetArtefactTypes*(activeModes);
//		searchPreviewText.GetComponent<Text>().text= relevantArtefacts.Count + " related artefacts";
	}

	public void SearchCommit()
	{
		//TODO relevantArtefacts = DCReader.*GetArtefactTypes*(activeModes);
		//BrowseControl.ImportArtefacts(relevantArtefacts);
	}
}
