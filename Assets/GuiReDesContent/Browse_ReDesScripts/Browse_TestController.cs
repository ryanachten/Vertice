using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Browse_TestController : MonoBehaviour {

	public Transform fieldGroup;

	public void UserBrowse() //finds user defined browse group
	{
		for (int i = 0; i < fieldGroup.childCount; i++) {

			Toggle curToggle = fieldGroup.GetChild(i).gameObject.GetComponent<Toggle>();
			if (curToggle.isOn)
			{
				string toggleName = curToggle.name;
				FindArtefacts(toggleName);
			}
		}
	}
		
	void FindArtefacts (string toggleName) //takes the user defined field and executes appropiate function
	{
		switch (toggleName) {

		case "BrowseTitle_FieldToggle" : 
			Debug.Log("Title");
			break;

		case "BrowseCreator_FieldToggle" : 
			Debug.Log("Creator");
			break;

		case "BrowseContributor_FieldToggle" : 
			Debug.Log("Contributor");
			break;

		case "BrowseDate_FieldToggle" : 
			Debug.Log("Date");
			break;

		case "BrowseSubject_FieldToggle" : 
			Debug.Log("Subject");
			break;

		case "BrowseCoverage_FieldToggle" : 
			Debug.Log("Coverage");
			break;

		default:
			break;
		}

	}

}
