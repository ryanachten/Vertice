using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Collect_SaveCollectionInformation : MonoBehaviour {

	public string collectionId = "#TESTID";
	public GameObject[] collectionFields;
	public Transform collectionArtefactsParent;
	private Dictionary<string, object> data;
	public Collect_CollectMenuGuiControl CollectMenuGuiControl;



	/// <summary>
	/// Goes through field attributes and pulls data from their text objects to be returned as a list
	/// Only to be used from the Attribute Field panel save button (NOT main menu save)
	/// </summary>
	/// <param name="attrName">Name of the current attribute</param>
	/// <param name="fieldAttribute">Attribute gameobject assigned via editor</param>
	/// <param name="attributeList">List of attribute value(s) to be returned</param>
	public void GenerateFieldLists() //string attrName, GameObject fieldAttribute, out List<string> attributeList
	{
		for (int i = 0; i < collectionFields.Length; i++) {

			List<string> attributeFieldContent = new List<string>();
			
			string attrName = collectionFields[i].name;

			for (int j = 0; j < collectionFields[i].transform.childCount; j++) //cycle through all of the attribute's fields
			{
				GameObject attrChild = collectionFields[i].transform.GetChild(j).gameObject;
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
			if (attrName == "Title")
			{
				CollectDataHost.CollectionTitle = attributeFieldContent;
			}
			else if (attrName == "Identifier")
			{
				collectionId = CollectDataHost.CollectionIdentifier;
				CollectDataHost.CollectionIdentifier = attributeFieldContent[0];
			}
			else if (attrName == "Creator")
			{
				CollectDataHost.CollectionCreator = attributeFieldContent;
			}
			else if (attrName == "Contributor")
			{
				CollectDataHost.CollectionContributor = attributeFieldContent;
			}
			else if (attrName == "Date")
			{
				CollectDataHost.CollectionDate = attributeFieldContent;
			}
			else if (attrName == "Coverage")
			{
				CollectDataHost.CollectionCoverage = attributeFieldContent;
			}
			else if (attrName == "Subject")
			{
				CollectDataHost.CollectionSubject = attributeFieldContent;
			}
			else if (attrName == "Description")
			{
				CollectDataHost.CollectionDescription = attributeFieldContent[0];
			}
		}
		CollectDataHost.DebugCollectionStruct();
		SaveCollectionData();
	}






	public void SaveCollectionData()
	{

		//Pull metadata from struct
		Dictionary<string, string[]> collectionMetadata = new Dictionary<string, string[]>();

		string[] collectTitles = DataListToArray(CollectDataHost.CollectionTitle);
		collectionMetadata.Add("title", collectTitles);

//		string[] identArray = new string[1]{CollectDataHost.CollectionIdentifier}; //FIXME can't have two 'identifier' keys in the dictionary
//		collectionMetadata.Add("identifier", identArray);

		string[] collectCreators = DataListToArray(CollectDataHost.CollectionCreator);
		collectionMetadata.Add("creator", collectCreators);

		string[] collectContributors = DataListToArray(CollectDataHost.CollectionContributor);
		collectionMetadata.Add("contributor", collectContributors);

		string[] collectDates = DataListToArray(CollectDataHost.CollectionDate);
		collectionMetadata.Add("date", collectDates);

		string[] collectCoverage = DataListToArray(CollectDataHost.CollectionCoverage);
		collectionMetadata.Add("coverage", collectCoverage);

		string[] collectSubject = DataListToArray(CollectDataHost.CollectionSubject);
		collectionMetadata.Add("subject", collectSubject);

		string[] descriptArray = new string[1]{CollectDataHost.CollectionDescription};
			collectionMetadata.Add("description", descriptArray);


		//Create transforms
		Dictionary<string, VerticeTransform> artefactTransforms = new Dictionary<string, VerticeTransform>();
		if (collectionArtefactsParent.childCount > 0)
		{
			for (int i = 0; i < collectionArtefactsParent.childCount; i++) 
			{
				Transform curObjTrans = collectionArtefactsParent.GetChild(i);
				VerticeTransform curArtefactVerticeTrans = new VerticeTransform(curObjTrans.position, curObjTrans.rotation, curObjTrans.localScale);
				string curArtefactName = curObjTrans.name;

				artefactTransforms.Add(curArtefactName, curArtefactVerticeTrans);
			}
		}

		CollectionWriter.WriteCollectionWithIdentifer(CollectDataHost.CollectionIdentifier, collectionMetadata, artefactTransforms); //
		CollectMenuGuiControl.LoadCollectInfo(CollectDataHost.CollectionIdentifier);
	}


	private string[] DataListToArray(List<string> dataList)
	{
		if (dataList == null)
		{
			Debug.Log("Datalist == null");
			string[] dataArray = new string[1]{""};
			return dataArray;
		}
		else if (dataList.Count == 0)
		{
			Debug.Log("Datalist == 0");
			string[] dataArray = new string[1]{""};
			return dataArray;
		}
		else
		{
			string[] dataArray = dataList.ToArray();
			return dataArray;
		}
	}
}
