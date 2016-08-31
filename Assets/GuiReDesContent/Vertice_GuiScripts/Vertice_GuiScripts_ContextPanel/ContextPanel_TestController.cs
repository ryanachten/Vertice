using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContextPanel_TestController : MonoBehaviour {

	//Controller for testing contextual media -> DublinCoreReader communication

	public Text userInput;
	public ContextPanel_InfoController InfoCont;
	public ContextPanel_MediaController MediaCont;

	public void LoadTestData()
	{
		string testIdentifier = userInput.text;
		InfoCont.LoadData(testIdentifier);
		MediaCont.artefactId = testIdentifier;
//		MediaCont.LoadMedia();

	}
}
