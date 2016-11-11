using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Import_AddDataToXml : MonoBehaviour {

	public GameObject[] descriptiveFieldAttributes;
	public GameObject[] structuralFieldAttributes;
//	private string identifier = "";
	private Dictionary<string, object> data;


	/// <summary>
	/// Compiles artefact metadata returned from text input fields with those stored in the SaveData struct
	/// </summary>
	public void GetArtefactData()
	{
		

		data = new Dictionary<string, object>();
		GenerateInfoDictionaries("descriptive", descriptiveFieldAttributes);
		GenerateInfoDictionaries("structural", structuralFieldAttributes);

		string meshLocation = ArtefactSaveData.MeshLocation;
		string texLocation = ArtefactSaveData.TexLocation;
		string identifier = ArtefactSaveData.ArtefactIdentifier;
		Dictionary<string, string>[] contextualMedia = ArtefactSaveData.ContextualMediaAssets.ToArray();

		DublinCoreWriter.WriteDataForArtefactWithIdentifier(identifier,data,meshLocation, texLocation, contextualMedia);
	}

	/// <summary>
	/// Generates dictionaries of attributes and their text values from input fields
	/// </summary>
	/// <param name="dataType">Data type; either descritpive or structural</param>
	/// <param name="fieldAttributes">Field attributes related to those data types</param>
	private void GenerateInfoDictionaries(string dataType, GameObject[] fieldAttributes) //for all of descriptive or structural attributes
	{
		Debug.Log("Getting data");

		Dictionary<string, object> attrDictionary = new Dictionary<string, object>();

		for (int i = 0; i < fieldAttributes.Length; i++) 
		{
			string attrName = fieldAttributes[i].name;

			List<string> attributeList = new List<string>();
			GenerateFieldList(attrName, fieldAttributes[i], out attributeList); //TODO check this - think it should be fieldAttributes not descriptiveFieldAttributes

			for (int j = 0; j < attributeList.Count; j++) {
				Debug.Log("attributeList:" + attributeList[j]);
			}
				
			if(attributeList.Count > 0) //for ea. attribute that isn't empty: add a string w/ attribute name and attribute field list as string array
			{
				string[] fieldAttrArray = attributeList.ToArray();
				attrDictionary.Add(attrName, fieldAttrArray);
			}
			else //for ea. attribute that is empty: add empty array TODO not sure if this is the best approach to represent an empty attribute
			{
				string[] fieldAttrArray = new string[1]{""}; 
				attrDictionary.Add(attrName, fieldAttrArray);
			}
		}
		data.Add(dataType, attrDictionary);
	}

	/// <summary>
	/// Goes through field attributes and pulls data from their text objects to be returned as a list
	/// </summary>
	/// <param name="attrName">Name of the current attribute</param>
	/// <param name="fieldAttribute">Attribute gameobject assigned via editor</param>
	/// <param name="attributeList">List of attribute value(s) to be returned</param>
	private void GenerateFieldList(string attrName, GameObject fieldAttribute, out List<string> attributeList)
	{
		List<string> attributeFieldContent = new List<string>();

		for (int j = 0; j < fieldAttribute.transform.childCount; j++) //cycle through all of the attribute's fields
		{
			GameObject attrChild = fieldAttribute.transform.GetChild(j).gameObject;
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
								if (fieldAttrChildText.Length > 0) //if user has assigned input add it to the list
								{
									attributeFieldContent.Add(fieldAttrChildText);
								}
							}
						}
					}
				}
			}
		}
		attributeList = attributeFieldContent;
	}
}
