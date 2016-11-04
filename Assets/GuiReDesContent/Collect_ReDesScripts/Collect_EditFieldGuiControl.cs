using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Collect_EditFieldGuiControl : MonoBehaviour {
	
	public Transform titleGroup;
	public Object titleFieldPrefab;

	public InputField identifierField;

	public Transform creatorGroup;
	public Object creatorFieldPrefab;

	public Transform contributorGroup;
	public Object contributorFieldPrefab;

	public Transform dateGroup;
	public Object dateFieldPrefab;

	public Transform coverageGroup;
	public Object coverageFieldPrefab;

	public Transform subjectGroup;
	public Object subjectFieldPrefab;

	public InputField descriptionField;



	void Start()
	{
		LoadCollectInfo();
	}


	//Called on opening the panel
	public void LoadCollectInfo()
	{
		Debug.Log("Should be loading");

//		guiInfoPanel.SetActive(true);

		InstantFieldData (CollectDataHost.CollectionTitle, titleGroup, titleFieldPrefab);
		identifierField.text = CollectDataHost.CollectionIdentifier;	
		InstantFieldData ( CollectDataHost.CollectionCreator, creatorGroup, creatorFieldPrefab);
		InstantFieldData ( CollectDataHost.CollectionContributor, contributorGroup, contributorFieldPrefab);
		InstantFieldData ( CollectDataHost.CollectionDate, dateGroup, dateFieldPrefab);
		InstantFieldData ( CollectDataHost.CollectionCoverage, coverageGroup, coverageFieldPrefab);
		InstantFieldData ( CollectDataHost.CollectionSubject, subjectGroup, subjectFieldPrefab);
		descriptionField.text = CollectDataHost.CollectionDescription;

	}

	/// <summary>
	/// Instantiates prefabs for multi-field attributes
	/// </summary>
	/// <param name="data">Artefac dictionary data.</param>
	/// <param name="elementType">Dublin Core type to be searched (i.e. Descriptive or Structural).</param>
	/// <param name="elementName">Attribute to be found in artefact data (i.e. Title / Date etc)</param>
	/// <param name="fieldGroup">Parent for the prefab to be instanted under</param>
	public void InstantFieldData (List<string> elementList, Transform fieldGroup, Object fieldPrefab) 
	{
		ResetField(fieldGroup);

		if (elementList != null && elementList.Count > 0){
			for (int i = 0; i < elementList.Count; i++) 
			{
				if (i == 0)
				{
//					Debug.Log("fieldGroup.name: " + fieldGroup.name + " child count: " + fieldGroup.childCount);
					fieldGroup.GetChild(0).GetComponentInChildren<InputField>().text = elementList[i];
				}
				else if (i > 0)
				{
					GameObject field = Object.Instantiate (fieldPrefab, fieldGroup) as GameObject;
					field.GetComponentInChildren<InputField> ().text = elementList[i];
					field.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
//					Debug.Log (field.name + " " + i + " : " + elementList[i]);
				}
			}
		}
		else
		{
//			Debug.Log("Nothing to instantiate, I'm gonna do nothing?");
		}
	}


	/// <summary>
	/// Resets attribute
	/// </summary>
	private void ResetField(Transform fieldGroup)
	{
		for (int i = 0; i < fieldGroup.childCount; i++) 
		{
			if (i == 1)
			{
				fieldGroup.GetChild(i).GetComponent<InputField>().text = "";
			}
			else if (i > 1)
			{
				Destroy(fieldGroup.GetChild(i).transform.gameObject);
			}
		}
		descriptionField.text = ""; //TODO test
	}
}
