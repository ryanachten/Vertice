using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Collect_CollectGuiInfo : MonoBehaviour {

	private Dictionary<string, string[]> data;
	public Object fieldText;
	public Object fieldException;
	public GameObject guiInfoPanel; //panel containing info fields


	//Descriptive elements
	public Transform titleGroup;
	public Text identifierText;
	public Transform creatorGroup;
	public Transform contributorGroup;
	public Transform dateGroup;

	public Transform coverageGroup;
	public Transform subjectGroup;
	public Text descriptionText;



	public void LoadCollectInfo(string collectId)
	{
		guiInfoPanel.SetActive(true);

		data = new Dictionary<string, string[]>();
		data = CollectionReader.GetCollectionMetadataWithIdentifier(collectId);

		InstantFieldData ( "title", titleGroup);
		ImportTextData ( "identifier", identifierText); //TODO text function
		InstantFieldData ( "creator", creatorGroup);
		InstantFieldData ( "contributor", contributorGroup);
		InstantFieldData ( "date", dateGroup);
		InstantFieldData ( "coverage", coverageGroup);
		InstantFieldData ( "subject", subjectGroup);
		ImportTextData ( "description", descriptionText); //TODO text function

	}

	/// <summary>
	/// Instantiates prefabs for multi-field attributes
	/// </summary>
	/// <param name="data">Artefac dictionary data.</param>
	/// <param name="elementType">Dublin Core type to be searched (i.e. Descriptive or Structural).</param>
	/// <param name="elementName">Attribute to be found in artefact data (i.e. Title / Date etc)</param>
	/// <param name="fieldGroup">Parent for the prefab to be instanted under</param>
	public void InstantFieldData (string elementName, Transform fieldGroup) 
	{
//		ResetField(fieldGroup); //TODO reset field function

		try {
			string[] curData = data[elementName];
			for (int i = 0; i < curData.Length; i++) 
			{
				GameObject field = Object.Instantiate (fieldText, fieldGroup) as GameObject;
				field.GetComponent<Text> ().text = curData [i];
//				Debug.Log (field.name + " " + i + " : " + curData [i]);
			}
		}
		catch(System.Exception ex)
		{
			GameObject field = Object.Instantiate (fieldException) as GameObject;
			field.transform.SetParent (fieldGroup, false);
//			Debug.Log ("No data in field");
		}
		
	}

	/// <summary>
	/// Assigns XML data to single field attributes
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="elementType">Dublin Core type to be searched (i.e. Descriptive or Structural).</param>
	/// <param name="elementName">Attribute to be found in artefact data (i.e. Title / Date etc)</param>
	/// <param name="elementText">Text element for data to be assigned to</param>
	private void ImportTextData(string elementName, Text elementText)
	{
		try
		{
			elementText.text = data[elementName][0]; //TODO passing this via index reference prob not ideal
		}
		catch(System.Exception ex) {
			elementText.text = "No data in field";
		}

	}



}
