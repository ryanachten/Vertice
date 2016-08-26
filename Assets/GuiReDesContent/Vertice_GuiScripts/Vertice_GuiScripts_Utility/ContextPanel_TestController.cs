using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ContextPanel_TestController : MonoBehaviour {

	public string testIdentifier;
//	public Text titleText;

	void Start()
	{
		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, string[]>> data = DublinCoreReader.GetArtefactWithIdentifier(testIdentifier);

		for (int i = 0; i < data["descriptive"]["title"].Length; i++){
			string theTitle = data["descriptive"]["title"][i];
			Debug.Log(theTitle);
		}
	}



}
