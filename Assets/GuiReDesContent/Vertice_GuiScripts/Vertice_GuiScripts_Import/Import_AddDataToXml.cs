using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Import_AddDataToXml : MonoBehaviour {

	public GameObject[] descriptiveFieldAttributes;
	public GameObject[] structuralFieldAttributes;
	public string meshLocation = "/Users/ryanachten/Documents/UnityTests/VerticeArchive_old/Buddha/Buddha_Model.obj"; //TODO assign during import
	public string texLocation = "/Users/ryanachten/Documents/UnityTests/VerticeArchive_old/Buddha/Buddha_Model.jpg";//TODO assign during import
	private string identifier = "buddha"; //TODO placeholder atm
	private Dictionary<string, object> data;



	//string identifier, Dictionary<string, object> metadata, string meshLocation, string texLocation, Dictionary<string, string>[] contextualMedia

	public void GetArtefactData()
	{
		Dictionary<string, string>[] contextualMedia = new Dictionary<string, string>[1]; //TODO write GetContextualMedia method

		data = new Dictionary<string, object>();
		GenerateInfoDictionaries("descriptive", descriptiveFieldAttributes);
		GenerateInfoDictionaries("structural", structuralFieldAttributes);

		DublinCoreWriter.WriteDataForArtefactWithIdentifier(identifier,data,meshLocation, texLocation, contextualMedia);


	//Dictionary Test
		/*	foreach (KeyValuePair<string, object> dataTypeDict in data) 
		{
			Debug.Log("dataType: " + dataTypeDict.Key);
			if (dataTypeDict.Value == null)
			{
				Debug.Log("dataTypeDict == null");
			}

			Dictionary<string, string[]> attrDict = dataTypeDict.Value as Dictionary<string, string[]>;

			foreach (KeyValuePair<string, string[]> attribute in attrDict) 
			{
				if (attrDict == null)
				{
					Debug.Log("dataTypeDict == null");
				}
					
				Debug.Log("\tAttribute: " + attribute.Key);
				for (int i = 0; i < attribute.Value.Length; i++) {
					Debug.Log("\t\tValue: " + attribute.Value[i]);
					if(attribute.Value == null)
					{
						Debug.Log("Attribute == null");
					}
				}
			}
		}*/
	}


	private void GenerateInfoDictionaries(string dataType, GameObject[] fieldAttributes) //for all of descriptive or structural attributes
	{
//		Dictionary<string, string[]> attrDictionary = new Dictionary<string, string[]>();
		Dictionary<string, object> attrDictionary = new Dictionary<string, object>();

		for (int i = 0; i < fieldAttributes.Length; i++) 
		{
			string attrName = fieldAttributes[i].name;

			List<string> attributeList = new List<string>();
			GenerateFieldList(attrName, descriptiveFieldAttributes[i], out attributeList);

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
//								else //if user input hasn't been assigned to the input field text add default info assignment or "" TODO
//								{									
//								}
							}
						}
					}
				}
			}
		}
		attributeList = attributeFieldContent;
	}
}
