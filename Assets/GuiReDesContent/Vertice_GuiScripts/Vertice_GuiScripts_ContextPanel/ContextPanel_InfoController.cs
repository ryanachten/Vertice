using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ContextPanel_InfoController : MonoBehaviour {

	public Object fieldText;
	public Object fieldException;

	//Artefact Information Panel
	public Transform titleGroup;
	public Text identifierText;
	public Transform creatorGroup;
	public Transform contributorGroup;
	public Transform dateGroup;
	public Text rightsText;

	//Contextual Information Panel
	public Transform coverageGroup;
	public Transform subjectGroup;
	public Text descriptionText;
	public Transform relationGroup;

	//Object Information Panel
	public Transform formatGroup;
	public Transform mediumGroup;
	public Transform extentGroup;

	//Media Information Panel
	public Transform mCreatorGroup;
	public Transform createdGroup;
	public Text mDescriptionText;
	public Transform isVersionGroup;

	//Mesh Information Panel
	public Transform mFormatGroup;
	public Transform mExtentGroup;
	public Text mIdentifierText;
	public Text mRightsText;


	public void LoadData(string artefactIdentifier)
	{
//		Debug.Log("Aretfact Identifier: " +  artefactIdentifier);

		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, string[]>> data = DublinCoreReader.GetArtefactWithIdentifier(artefactIdentifier);

		ArtefactInfoLoad(data); //TODO these need to be removed from Start method
		ContextInfoLoad(data); //and activated via context info toggles
		ObjectInfoLoad(data); //should prob be provided their own script maybe?
		MediaInfoLoad(data);
		MeshInfoLoad(data); 
	}

	public void ArtefactInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		InstantFieldData (data, "descriptive", "title", titleGroup);
		//identifierText.text = testIdentifier; //TODO need to find a way to access the identifier (could just pass it as an argument I guess)
		InstantFieldData (data, "descriptive", "creator", creatorGroup);
		InstantFieldData (data, "descriptive", "contributor", contributorGroup);
		InstantFieldData (data, "descriptive", "date", dateGroup);
		ImportTextData (data, "descriptive", "rights", rightsText);
	}
		
	public void ContextInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		InstantFieldData (data, "descriptive", "coverage", coverageGroup);
		InstantFieldData (data, "descriptive", "subject", subjectGroup);
		ImportTextData (data, "descriptive", "description", descriptionText);
		InstantFieldData (data, "descriptive", "relation", relationGroup);
	}

	public void ObjectInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		InstantFieldData (data, "descriptive", "format", formatGroup);
		InstantFieldData (data, "descriptive", "medium", mediumGroup);
		InstantFieldData (data, "descriptive", "extent", extentGroup);
	}

	public void MediaInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		InstantFieldData (data, "structural", "creator", mCreatorGroup);
		InstantFieldData (data, "structural", "created", createdGroup);
		ImportTextData (data, "structural", "description", mDescriptionText);
	}

	public void MeshInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		InstantFieldData (data, "structural", "format", mFormatGroup);
		InstantFieldData (data, "structural", "extent", mExtentGroup);
		ImportTextData (data, "structural", "identifier", mIdentifierText);
		ImportTextData (data, "structural", "rights", mRightsText);
	}
		

	//Loading methods
	private void InstantFieldData(Dictionary<string, Dictionary<string, string[]>> data, 
									string elementType, string elementName, Transform fieldGroup)
	{
		ResetField(fieldGroup);
		try {
			string[] curData = data[elementType][elementName];
			for (int i = 0; i < curData.Length; i++) 
			{
				GameObject field = Object.Instantiate (fieldText) as GameObject;
				field.transform.SetParent (fieldGroup, false);
				field.GetComponent<Text> ().text = curData [i];
				Debug.Log (field.name + " " + i + " : " + curData [i]);
			}
		}
		catch(System.Exception ex)
		{
			GameObject field = Object.Instantiate (fieldException) as GameObject;
			field.transform.SetParent (fieldGroup, false);
			Debug.Log ("No data in field");
		}
	}
		
	private void ImportTextData(Dictionary<string, Dictionary<string, string[]>> data, 
									string elementType, string elementName, Text elementText)
	{
		try
		{
			elementText.text = data[elementType][elementName][0]; //TODO passing this via index reference prob not ideal
		}
		catch(System.Exception ex) {
			elementText.text = "No data in field";
		}

	}

	private void ResetField(Transform fieldGroup)
	{
		for (int i = 0; i < fieldGroup.childCount; i++) {
			GameObject groupChild = fieldGroup.GetChild(i).gameObject;
			Debug.Log("Deleting Field: " + groupChild.name);
			Destroy(groupChild);
		}
	}
}
