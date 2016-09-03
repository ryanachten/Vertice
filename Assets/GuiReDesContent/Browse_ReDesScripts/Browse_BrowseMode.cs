using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Browse_BrowseMode : MonoBehaviour {

	public Browse_SelectAttributes SelectAttr;
	public Transform fieldGroup;
	public GameObject attributePanel;

	/// <summary>
	/// Detects which toggle has been activated by user
	/// </summary>
	public void UserBrowse()
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
		attributePanel.SetActive (true);

		switch (toggleName) {

		case "BrowseTitle_FieldToggle" : 
			SelectAttr.GetAttributes("Title");
			break;

		case "BrowseCreator_FieldToggle" : 
			SelectAttr.GetAttributes("Creator");
			break;

		case "BrowseContributor_FieldToggle" : 
			SelectAttr.GetAttributes("Contributor");
			break;

		case "BrowseDate_FieldToggle" : 
			SelectAttr.GetAttributes("Date");
			break;

		case "BrowseSubject_FieldToggle" : 
			SelectAttr.GetAttributes("Subject");
			break;

		case "BrowseCoverage_FieldToggle" : 
			SelectAttr.GetAttributes("Coverage");
			break;

		default:
			break;
		}
	}

}
