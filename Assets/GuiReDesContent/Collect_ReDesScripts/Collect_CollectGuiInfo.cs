using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Collect_CollectGuiInfo : MonoBehaviour {

	private Dictionary<string, string[]> data;
	public Object fieldText;
	public Object fieldException;

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
		data = new Dictionary<string, string[]>();
		data = CollectionReader.GetCollectionMetadataWithIdentifier(collectId);

		InstantFieldData ( "title", titleGroup);
//		InstantFieldData ( "identifier", "descriptive", identifierText); //TODO text function
		InstantFieldData ( "creator", creatorGroup);
		InstantFieldData ( "contributor", contributorGroup);
		InstantFieldData ( "date", dateGroup);
		InstantFieldData ( "coverage", coverageGroup);
		InstantFieldData ( "subject", subjectGroup);
//		InstantFieldData ( "description", "descriptive", descriptionText); //TODO text function

	}


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



}
