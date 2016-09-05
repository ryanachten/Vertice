using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Browse_BrowseSelectAttributes : MonoBehaviour {

	//enables select attribute panel for user to refine their
	//browse query, then sends to browse control

	public Transform attributeParent; //transform for prefabs to be instantiated under
	public Object attributePrefab;
	public Browse_BrowseControl BrowseCont;

	private string browseMode; //user defined browse mode



	/// <summary>
	/// Gets all attributes related to a user browse query type. Executes DCReader function.
	/// </summary>
	/// <param name="browseType">Type of browse user wants to view</param>
	public void GetAttributes(string browseType)
	{
		//TODO DCReader function for returning attributes based on user type query
		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Debug.Log("GetAttr: " + browseType);

		string[] browseAttributes;

		switch (browseType) {

		case "Creator" : 
			browseAttributes = DublinCoreReader.GetValuesForCreator();	
			break;

		case "Contributor" : 
			browseAttributes = DublinCoreReader.GetValuesForContributor();	
			break;

		case "Date" : 
			browseAttributes = null; //TODO this will need to be apporoached differently
			break;

		case "Subject" : 
			browseAttributes = DublinCoreReader.GetValuesForSubject(); //TODO this will need to be apporoached differently
			break;

		case "Coverage" : 
			browseAttributes = null; //TODO no method
			break;

		default:
			browseAttributes = null;
			break;
		}

		browseMode = browseType;
		InstantAttributes(browseAttributes);
	}


	/// <summary>
	/// Executed once user has finished their selection of relevant attributes
	/// </summary>
	public void DoneAttributeSelect()
	{
		List<string> activeAttributes = new List<string> ();

		for (int i = 0; i < attributeParent.childCount; i++) {

			Toggle curToggle = attributeParent.GetChild (i).GetComponent<Toggle>();
			if (curToggle.isOn) 
			{
				activeAttributes.Add (attributeParent.GetChild (i).GetComponentInChildren<Text>().text);
			}
		}

		string[] activeAttrArray = activeAttributes.ToArray(); //need to convert to array to account for DCReader, need list cause don't know how many attrs will be active
		string[] attributeIdentifiers;

		if (browseMode == "Creator"){
			attributeIdentifiers = DublinCoreReader.GetIdentifiersForCreators(activeAttrArray);
		}
		else if (browseMode == "Contributor"){
			attributeIdentifiers = DublinCoreReader.GetIdentifiersForContributors(activeAttrArray);
		}
		else if (browseMode == "Date"){
			attributeIdentifiers = null; //TODO this will need to be approached differently
		}
		else if (browseMode == "Subject"){
			attributeIdentifiers = DublinCoreReader.GetIdentifiersForSubjects(activeAttrArray); //TODO this will need to be approached differently
		}
		else if (browseMode == "Coverage"){
			attributeIdentifiers = null; //TODO no method
		}
		else 
		{
			attributeIdentifiers = null;
		}
			
		for (int i = 0; i < attributeIdentifiers.Length; i++) {
			Debug.Log("activeAttributes: " + attributeIdentifiers[i]);
		}

		BrowseCont.ImportArtefacts (attributeIdentifiers);
		gameObject.SetActive (false);

	}



	/// <summary>
	/// Instantiates browse attribute prefabs for user to select
	/// </summary>
	/// <param name="InstantAttributes">attributes in XML related to user query</param>
	private void InstantAttributes( string[] browseAttributes)
	{
		ResetAttributes ();
		for (int i = 0; i < browseAttributes.Length; i++) {
			
			GameObject curBrowseAtt = Object.Instantiate (attributePrefab, attributeParent) as GameObject;
			curBrowseAtt.GetComponentInChildren<Text>().text = browseAttributes[i];
		}
	}
		

	/// <summary>
	/// Resets the attribute panel
	/// </summary>
	private void ResetAttributes()
	{
		for (int i = 0; i < attributeParent.childCount; i++) 
		{
			GameObject curAttr = attributeParent.GetChild (i).gameObject;
			Destroy(curAttr);
		}
	}

}
