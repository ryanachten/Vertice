using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ContextPanel_TestController : MonoBehaviour {

	public string testIdentifier;
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


	void Start()
	{
		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, string[]>> data = DublinCoreReader.GetArtefactWithIdentifier(testIdentifier);

		ArtefactInfoLoad(data); //TODO these need to be removed from Start method
		ContextInfoLoad(data); //TODO and activated via context info toggles
		ObjectInfoLoad(data); //TODO should prob be provided their own script maybe?
		MediaInfoLoad(data); //TODO as above
		MeshInfoLoad(data); //TODO as above

	}

	public void ArtefactInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		DublinCoreReader.Refresh (); //TODO this is currently wrong I think. Needs to be refresh() and then
									// loads the data (regardless of whether it is up to date or not)

		instantFieldData (data, "descriptive", "title", titleGroup);
		identifierText.text = testIdentifier;
		instantFieldData (data, "descriptive", "creator", creatorGroup);
		instantFieldData (data, "descriptive", "contributor", contributorGroup);
		instantFieldData (data, "descriptive", "date", dateGroup);
		//rightsText =  TODO no rights data to test against in the XML atm
	}

	public void ContextInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		DublinCoreReader.Refresh ();

		instantFieldData (data, "descriptive", "coverage", coverageGroup);
		instantFieldData (data, "descriptive", "subject", subjectGroup);
		descriptionText.text = data ["descriptive"]["description"][0]; //TODO passing this via index reference prob not ideal
		instantFieldData (data, "descriptive", "relation", relationGroup);
	}

	public void ObjectInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		DublinCoreReader.Refresh ();

		instantFieldData (data, "descriptive", "format", formatGroup);
		instantFieldData (data, "descriptive", "medium", mediumGroup);
		instantFieldData (data, "descriptive", "extent", extentGroup);
	}

	public void MediaInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		DublinCoreReader.Refresh ();

		instantFieldData (data, "structural", "creator", mCreatorGroup);
		instantFieldData (data, "structural", "created", createdGroup);
		mDescriptionText.text = data ["structural"]["description"][0]; //TODO passing this via index reference prob not ideal
	}

	public void MeshInfoLoad(Dictionary<string, Dictionary<string, string[]>> data)
	{
		instantFieldData (data, "structural", "format", mFormatGroup);
		instantFieldData (data, "structural", "extent", mExtentGroup);

//		importTextData (data, "structural", "identifier", mIdentifierText); //HERE
		mIdentifierText.text = data ["structural"]["identifier"][0]; //TODO passing this via index reference prob not ideal


		importTextData (data, "structural", "rights", mRightsText);
	}
		

	//*****

	private void instantFieldData(Dictionary<string, Dictionary<string, string[]>> data, 
									string elementType, string elementName, Transform fieldGroup)
	{
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
		
	private void importTextData(Dictionary<string, Dictionary<string, string[]>> data, 
		string elementType, string elementName, Text elementText)
	{
		try
		{
			elementText.text = data[elementName][elementName][0]; //TODO passing this via index reference prob not ideal
		}
		catch(System.Exception ex) {
			elementText.text = "No data in field";
		}

	}
}
