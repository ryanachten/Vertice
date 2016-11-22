using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Import_MandatoryFieldVerify : MonoBehaviour {

	//checks whether mandatory data fields have input assigned to them
	//doesn't check validity of data input (at this stage)

	public Import_AddDataToXml AddDataToXml;
	public MandatoryFieldFeedback FieldFeedback;
	public GameObject[] mandatoryAttributes;
	private List<string> remainingFields;
	public bool testXmlWriterMode;

	public Color invalidTextColor;
	public Color validTextColor;
	public Text infoPanelText;
	public Text contextPanelText;
	public Text mediaPanelText;
	public Text meshPanelText;


	//TODO review this verification process - doesn't actually take into account expected data types

	public void VerifyFields() //gauges whether fields have text input assigned to them and sends to feedback if not
	{
		remainingFields = new List<string>();

		for (int i = 0; i < mandatoryAttributes.Length; i++) {

			string attrName = mandatoryAttributes[i].name;
			for (int j = 0; j < mandatoryAttributes[i].transform.childCount; j++) { //originally did this by referencing child index 
																					//~ too unstable as requires specifically ordered editor hierarchy
				GameObject attrChild = mandatoryAttributes[i].transform.GetChild(j).gameObject;
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
									if (fieldAttrChild.GetComponent<Text>().text.Length <= 0) //if user input hasn't been assigned to the input field text
									{
										remainingFields.Add(i.ToString()); //attrName
										FieldFeedback.InvalidFieldFeedback(mandatoryAttributes[i], attrChild); //execute invalid feedback
									}
									else //if user has assigned input 
									{
										FieldFeedback.ResetValidField(mandatoryAttributes[i], attrChild); //Execute field reset
									}
								}
							}
						}
					}
				}
			}
		}
		if (remainingFields.Count > 0)
		{
			Debug.Log("Mandatory Fields remaining");
			ProvidePanelFeedback(remainingFields);
		}
		else if (remainingFields.Count == 0)
		{
			Debug.Log("Fields complete! Add data");
			AddDataToXml.GetArtefactData();
		}
		if (testXmlWriterMode)
		{
			Debug.Log("Debug test");
			AddDataToXml.GetArtefactData();
		}
			
	}


	void ProvidePanelFeedback(List<string> remainingFields)
	{
		bool infoPanel = false;
		bool contextPanel = false;
		bool mediaPanel = false;
		bool meshPanel = false;


		for (int i = 0; i < remainingFields.Count; i++) 
		{
			if (remainingFields[i] == "0" || remainingFields[i] == "1" || remainingFields[i] == "2" || remainingFields[i] == "3" || remainingFields[i] == "4")
			{
				infoPanel = true;
			}
			if (remainingFields[i] == "5" || remainingFields[i] == "6")
			{
				contextPanel = true;
			}
			if (remainingFields[i] == "7")
			{
				mediaPanel = true;
			}
			if (remainingFields[i] == "8")
			{
				meshPanel = true;
			}
		}
		if (infoPanel){
			Debug.Log("Execute infoPanel");
			infoPanelText.color = invalidTextColor;
		}
		else{
			infoPanelText.color = validTextColor;
		}

		if (contextPanel){
			Debug.Log("Execute contextPanel");
			contextPanelText.color = invalidTextColor;
		}
		else{
			contextPanelText.color = validTextColor;
		}

		if (mediaPanel){
			Debug.Log("Execute mediaPanel");	
			mediaPanelText.color = invalidTextColor;
		}
		else{
			mediaPanelText.color = validTextColor;
		}

		if (meshPanel){
			Debug.Log("Execute meshPanel");
			meshPanelText.color = invalidTextColor;
		}
		else{
			meshPanelText.color = validTextColor;
		}
	}

}
