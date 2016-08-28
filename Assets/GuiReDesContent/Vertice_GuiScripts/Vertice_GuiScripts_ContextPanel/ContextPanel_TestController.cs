using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContextPanel_TestController : MonoBehaviour {

	public Text userInput;
	public ContextPanel_InfoController InfoCont;
	public ContextPanel_MediaController MediaCont;

	public void LoadTestData()
	{
		string testIdentifier = userInput.text;
		InfoCont.LoadData(testIdentifier);
		MediaCont.LoadMedia(testIdentifier);

	}
}
