using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Browse_BrowseMode : MonoBehaviour {

	public Browse_BrowseControl BrowseControl;
	public Transform fieldGroup;

	/// <summary>
	/// Detects which toggle has been activated by user
	/// </summary>
	public void UserBrowse() //finds user defined browse group
	{
		for (int i = 0; i < fieldGroup.childCount; i++) {

			Toggle curToggle = fieldGroup.GetChild(i).gameObject.GetComponent<Toggle>();
			if (curToggle.isOn)
			{
				string toggleName = curToggle.name;
				GetBrowseMode(toggleName);
			}
		}
	}

	/// <summary>
	/// Takes the user defined browse mode and executes relevant Browse query
	/// </summary>
	/// <param name="toggleName">Active toggle name</param>
	void GetBrowseMode (string toggleName) //takes the user defined field and executes appropiate function
	{
		switch (toggleName) {

		case "BrowseTitle_FieldToggle" : 
			BrowseControl.FindArtefacts("Title");
			break;

		case "BrowseCreator_FieldToggle" : 
			BrowseControl.FindArtefacts("Creator");
			break;

		case "BrowseContributor_FieldToggle" : 
			BrowseControl.FindArtefacts("Contributor");
			break;

		case "BrowseDate_FieldToggle" : 
			BrowseControl.FindArtefacts("Date");
			break;

		case "BrowseSubject_FieldToggle" : 
			BrowseControl.FindArtefacts("Subject");
			break;

		case "BrowseCoverage_FieldToggle" : 
			BrowseControl.FindArtefacts("Coverage");
			break;

		default:
			break;
		}

	}

}
