using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Collect_CollectMenuGuiControl : MonoBehaviour {

	private Dictionary<string, string[]> data;
	public Object fieldText;
	public Object fieldException;
	public GameObject guiInfoPanel; //panel containing info fields
	public GameObject editCollectButton;

	//Main Menu Panel
	public Transform titleGroup;
	public Text identifierText;
	public Transform creatorGroup;
	public Transform contributorGroup;
	public Transform dateGroup;
	public Transform coverageGroup;
	public Transform subjectGroup;
	public Text descriptionText;


	void Start()
	{
		#if UNITY_WEBGL
		editCollectButton.SetActive(false);
		#endif
	}

	public void LoadCollectInfo(string collectId)
	{
		CollectDataHost.LoadXmlData(collectId); //FIXME not sure if this should be called from this script
//		CollectDataHost.DebugCollectionStruct();

		Debug.Log("Should be loading");

		guiInfoPanel.SetActive(true);

		InstantFieldData (CollectDataHost.CollectionTitle, titleGroup);
		identifierText.text = CollectDataHost.CollectionIdentifier;	
		InstantFieldData ( CollectDataHost.CollectionCreator, creatorGroup);
		InstantFieldData ( CollectDataHost.CollectionContributor, contributorGroup);
		InstantFieldData ( CollectDataHost.CollectionDate, dateGroup);
		InstantFieldData ( CollectDataHost.CollectionCoverage, coverageGroup);
		InstantFieldData ( CollectDataHost.CollectionSubject, subjectGroup);
		descriptionText.text = CollectDataHost.CollectionDescription;

	}

	/// <summary>
	/// Instantiates prefabs for multi-field attributes
	/// </summary>
	/// <param name="data">Artefac dictionary data.</param>
	/// <param name="elementType">Dublin Core type to be searched (i.e. Descriptive or Structural).</param>
	/// <param name="elementName">Attribute to be found in artefact data (i.e. Title / Date etc)</param>
	/// <param name="fieldGroup">Parent for the prefab to be instanted under</param>
	public void InstantFieldData (List<string> elementList, Transform fieldGroup) 
	{
		ResetField(fieldGroup);

		try {
			for (int i = 0; i < elementList.Count; i++) 
			{
				GameObject field = Object.Instantiate (fieldText, fieldGroup) as GameObject;
				field.GetComponent<Text> ().text = elementList[i];
				field.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
//				Debug.Log (field.name + " " + i + " : " + elementList[i]);
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
	/// Resets attribute
	/// </summary>
	private void ResetField(Transform fieldGroup)
	{
		if (fieldGroup.childCount > 0)
		{
			for (int i = 0; i < fieldGroup.childCount; i++) {
				Destroy(fieldGroup.GetChild(i).transform.gameObject);
			}
		}
		descriptionText.text = ""; //TODO test
	}
}
