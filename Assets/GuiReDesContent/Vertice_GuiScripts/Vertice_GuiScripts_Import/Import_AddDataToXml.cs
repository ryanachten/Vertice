using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Import_AddDataToXml : MonoBehaviour {

	public GameObject[] descriptiveFieldAttributes;
	public GameObject[] structuralFieldAttributes;
	private string identifier;
	private Dictionary<string, object> data;



	public void GetFieldData()
	{
		AddFieldTextToDictionary("descriptive", descriptiveFieldAttributes);
		AddFieldTextToDictionary("structural", structuralFieldAttributes);
		//TODO Media Panel
	}


	/// <summary>
	///Adds data from fields to a dictionary to be passed to DCWriter
	/// </summary>
	/// <param name="infoType">Type of information to be added. Either descriptive or structural</param>
	/// <param name="fieldAttributes">Field attributes to be searched through.</param>
	private void AddFieldTextToDictionary(string infoType, GameObject[] fieldAttributes)
	{
		for (int i = 0; i < fieldAttributes.Length; i++) 
		{
			string attrName = fieldAttributes[i].name;
			for (int j = 0; j < fieldAttributes[i].transform.childCount; j++)  //originally did this by referencing child index 
			{																	//~ too unstable as requires specifically ordered editor hierarchy
				GameObject attrChild = fieldAttributes[i].transform.GetChild(j).gameObject;
				if (attrChild.name == attrName + "_FieldGroup") 
				{
					for (int k = 0; k < attrChild.transform.childCount; k++) 
					{
						GameObject fieldGroupChild = attrChild.transform.GetChild(k).gameObject;
						if (fieldGroupChild.name == attrName + "_FieldAttr")
						{
							for (int l = 0; l < fieldGroupChild.transform.GetChild(0).transform.childCount; l++) {
								GameObject fieldAttrChild = fieldGroupChild.transform.GetChild(0).transform.GetChild(l).gameObject;
								if (fieldAttrChild.name == "Text")
								{
									string fieldAttrChildText = fieldAttrChild.GetComponent<Text>().text;
									if (fieldAttrChildText.Length <= 0) //if user input hasn't been assigned to the input field text
									{
										//TODO add default info assignment or ""
										Debug.Log("attrName: " + attrName) ;
									}
									else //if user has assigned input 
									{
										//TODO add text to dictionary
										Debug.Log("attrName: " + attrName + " | " + "text: " + fieldAttrChildText);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
