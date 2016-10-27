using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MandatoryFieldVerify : MonoBehaviour {

	//checks whether mandatory data fields have input assigned to them
	//doesn't check validity of data input (at this stage)

	public Import_AddDataToXml AddDataToXml;
	public MandatoryFieldFeedback FieldFeedback;
	public GameObject[] mandatoryAttributes;
	private List<string> remainingMandatoryFields;


	//TODO review this verification process - doesn't actually take into account expected data types

	public void VerifyFields() //gauges whether fields have text input assigned to them and sends to feedback if not
	{
		remainingMandatoryFields = new List<string>();

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
										remainingMandatoryFields.Add(attrName);
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
//		if (remainingMandatoryFields.Count > 0)
//		{
//			Debug.Log("Mandatory Fields remaining:");
//			for (int i = 0; i < remainingMandatoryFields.Count; i++) 
//			{
//				Debug.Log("Field: " + remainingMandatoryFields[i]);
//			}
//		}
//		else if (remainingMandatoryFields.Count == 0)
//		{
//			Debug.Log("ADD DATA");
//			AddDataToXml.GetFieldData();
//		}
		AddDataToXml.GetArtefactData();//TODO comment out
	}
}
