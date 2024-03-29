﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Browse_SearchMode : MonoBehaviour {

	public Browse_BrowseControl BrowseControl;
	public Transform toggleParent;
//	private List<string> activeModes;

	public GameObject searchPreviewText;


	public void SearchPreview()
	{
		if (!searchPreviewText.activeSelf) {
			searchPreviewText.SetActive (true);
		}
		List<string> activeModes = new List<string>();
		GetSearchModes(out activeModes);

		for (int i = 0; i < activeModes.Count; i++) {
			Debug.Log("activeModes: " + activeModes[i]);



		}
	}

//	private void GetPreviewCount(string searchQuery, string searchMode, out int previewCount)
//	{
//		string[] previewValues;
//
//		switch (activeModes[i]) {
//
//		//			case "Title" : //TODO no title DCReader functions
//
//		case "Creator" :
//			previewValues = DublinCoreReader.GetValuesForCreator();
//			for (int i = 0; i < previewValues.Length; i++) {
//
//			}
//
//
//		default:
//			break;
//		}
//
//	}



	public void SearchCommit()
	{
		//TODO relevantArtefacts = DCReader.*GetArtefactTypes*(activeModes);
		//BrowseControl.ImportArtefacts(relevantArtefacts);
	}


	private void GetSearchModes(out List<string> activeToggles)
	{
		activeToggles = new List<string> ();
		for (int i = 0; i < toggleParent.childCount; i++) 
		{
			Toggle curToggle = toggleParent.GetChild (i).GetComponent<Toggle>();	
			if (curToggle.isOn) 
			{
				string toggleName = toggleParent.GetChild (i).GetComponentInChildren<Text>().text;	
				activeToggles.Add (toggleName);
			}
		}
	}


}
