using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Browse_AddToCollectButtonInfo : MonoBehaviour {

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

	public void AddToCollection()
	{
		string artefactIdentifier = GameObject.Find("ArtefactInfoIdenifier_Text").GetComponent<Text>().text;

		VerticeTransform defaultPosition = new VerticeTransform();

		if (artefactIdentifier.Length > 0)
		{ 
			Debug.Log("Collection identifier: " + identifier.text);
			CollectionWriter.AddArtefactToCollectionWithIdentifier(identifier.text, artefactIdentifier, defaultPosition);
		}
	}
}
