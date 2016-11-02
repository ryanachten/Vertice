using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Import_IndentifierGenerator : MonoBehaviour {

	public string idPrefix = "VERT-";
	public InputField identifierField;
	public Text identifierFieldText;
//	public Text identifierFieldPlaceholder;

	public void GenerateIdentifier()
	{
		string dateTime = DateTime.Now.ToString("yyMMddhhmmss");
		string vertId = idPrefix + dateTime;

		ArtefactSaveData.ArtefactIdentifier = vertId;
		identifierField.text = vertId;
//		Debug.Log("vertId: " + ArtefactSaveData.ArtefactIdentifier);
	}

	public void UserDefinedIdentifier()
	{
		if (identifierFieldText.text.Length > 1)
		{
			ArtefactSaveData.ArtefactIdentifier = identifierFieldText.text;
			Debug.Log("customId: " + ArtefactSaveData.ArtefactIdentifier);
		}
	}
}
