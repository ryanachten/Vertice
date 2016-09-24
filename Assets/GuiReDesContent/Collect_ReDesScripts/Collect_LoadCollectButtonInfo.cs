using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collect_LoadCollectButtonInfo : MonoBehaviour {

	public Text title;
	public Text identifier;
	public Text creator;
	public Text date;
	public Text description;


	public void LoadInfo(string[] collectData)
	{
		title.text = collectData[0];
		identifier.text = collectData[1];
		creator.text = collectData[2];
		date.text = collectData[3];
		description.text = collectData[4];
	}

	public void SendToLoad()
	{
		Collect_CollectControl CollectControl = GameObject.Find("CollectionMenu_Panel").GetComponent<Collect_CollectControl>();
		CollectControl.ImportArtefacts(identifier.text);
	}
}
