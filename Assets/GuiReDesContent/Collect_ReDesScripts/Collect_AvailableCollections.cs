using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collect_AvailableCollections : MonoBehaviour {

	//used to load available collection buttons into collections panel
	//activated on scene load and otherwise when panel opened

	public Object collectButtonPrefab;
	public Transform instantParent;

	void Start () {
		GetCollections();
	}


	public void GetCollections () {
		CollectionReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");

		string[] collectIdentifiers = CollectionReader.GetIdentifiersForCollections();
		for (int i = 0; i < collectIdentifiers.Length; i++) {
			InstantCollectButton(collectIdentifiers[i]);
		}
	}

	private void InstantCollectButton(string collectID)
	{
		Object curCollectButton = Instantiate(collectButtonPrefab, instantParent);

		Dictionary<string, string[]> data = CollectionReader.GetCollectionMetadataWithIdentifier(collectID);
		string collectTitle = data["title"][0]; //grabs first title for collection button
		string collectCreator = data["creator"][0];
		string collectDate = data["date"][0];
		string collectDescription = data["description"][0];

		Debug.Log("");
		Debug.Log("collectTitle: " + collectTitle);
		Debug.Log("collectCreator: " + collectCreator);
		Debug.Log("collectDate: " + collectDate);
		Debug.Log("collectDescription: " + collectDescription);
		Debug.Log("");


		//TODO add other CollectionReader references to get the rest of metadata -> send to new script to update button gui
	}
}
