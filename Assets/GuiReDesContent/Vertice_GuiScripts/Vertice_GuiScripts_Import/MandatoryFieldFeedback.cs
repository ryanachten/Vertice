using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MandatoryFieldFeedback : MonoBehaviour {

	public Color validPanelColour; //invalid field colour
	public Color invalidPanelColour; //invalid field colour
	public Object feedbackText; //invalid field text feedback

	public void InvalidFieldFeedback(GameObject fieldAttr, GameObject fieldGroup) //changes panel colour and provides text feedback
	{
//		Debug.Log(fieldAttr.name + " ~ Mandatory field invalid\n");
		bool feedbackProvided = false;

		for (int i = 0; i < fieldGroup.transform.childCount; i++) {
			if (fieldGroup.transform.GetChild(i).name == "MandatoryFieldFeedback(Clone)")
			{
				feedbackProvided = true; //feedback already exists
			}
		}

		if (!feedbackProvided) //if feedback hasn't ben provided, provide it
		{
			fieldAttr.GetComponent<Image>().color = invalidPanelColour;
			GameObject invalidFeedback = Object.Instantiate(feedbackText) as GameObject;
			invalidFeedback.transform.SetParent(fieldGroup.transform, false);
		}
	}

	public void ResetValidField(GameObject fieldAttr, GameObject fieldGroup) //changes panel colour back and removes text feedback
	{

		for (int i = 0; i < fieldGroup.transform.childCount; i++) //check to see if has invalid feedback
		{
			if (fieldGroup.transform.GetChild(i).name == "MandatoryFieldFeedback(Clone)") //if so, reset	
			{
				fieldAttr.GetComponent<Image>().color = validPanelColour;
				Destroy(fieldGroup.transform.GetChild(i).gameObject);
			}
		}
	}
}
