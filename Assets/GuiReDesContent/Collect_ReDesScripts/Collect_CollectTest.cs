using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collect_CollectTest : MonoBehaviour {

	public string collectId = "TestCollect";

	// Use this for initialization
	void Start () {
		GetCollections();
	}
	
	// Update is called once per frame
	public void GetCollections () {
		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Vertice_CollectionInformation.xml");

		Dictionary<string, Dictionary<string, string[]>> data = DublinCoreReader.GetArtefactWithIdentifier(collectId);
		Debug.Log("Collect data: " + data["descriptive"]["title"][0]);

	}
}
