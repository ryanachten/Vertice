﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Collect_AvailableCollections : MonoBehaviour {

	//used to load available collection buttons into collections panel
	//activated on scene load and otherwise when panel opened

	public Object collectButtonPrefab;
	public Transform instantParent;

	void Start () {
		// Upon creating this object, download the relevant XML data and use it to prepare the CollectionReader
		StartCoroutine (DownloadXml (Paths.Remote + "/Metadata/Vertice_CollectionInformation.xml"));
	}

	/// <summary>
	/// Download an XML file from a specified URL and populate a CollectionReader with the contents of the
	/// XML file
	/// </summary>
	/// <param name="url">The absolute path to the XML file, with the scheme (e.g. file://, http://, etc.) </param>
	IEnumerator DownloadXml(string url) {
		UnityWebRequest www = UnityWebRequest.Get (url);
		yield return www.Send ();

		if (www.isError) {
			// TODO: Echo the error condition to the user
			Debug.Log ("Couldn't download XML file at " + url + "\n" + www.error);
		} else {
			CollectionReader.LoadXmlFromText (www.downloadHandler.text);
			Debug.Log("Downloaded some XML from " + url);
			GetCollections ();
		}
	}


	public void GetCollections () {
		//CollectionReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Vertice_CollectionInformation.xml");

		string[] collectIdentifiers = CollectionReader.GetIdentifiersForCollections();
		for (int i = 0; i < collectIdentifiers.Length; i++) {
			InstantCollectButton(collectIdentifiers[i]);
		}
	}

	private void InstantCollectButton(string collectID)
	{
		GameObject curCollectButton = Instantiate(collectButtonPrefab, instantParent) as GameObject;
		Dictionary<string, string[]> data = CollectionReader.GetCollectionMetadataWithIdentifier(collectID);

		string[] collectData = new string[5];

		try {
			collectData[0] = data["title"][0]; //grabs first title for collection button	
		} catch (System.Exception ex) {
			collectData[0] = "";
		}
		try {
			collectData[1] = data["identifier"][0]; //grabs first title for collection button	
		} catch (System.Exception ex) {
			collectData[1] = "";
		}
		try {
			collectData[2] = data["creator"][0]; //grabs first title for collection button	
		} catch (System.Exception ex) {
			collectData[2] = "";
		}
		try {
			collectData[3] = data["date"][0]; //grabs first title for collection button	
		} catch (System.Exception ex) {
			collectData[3] = "";
		}
		try {
			collectData[4] = data["description"][0]; //grabs first title for collection button	
		} catch (System.Exception ex) {
			collectData[4] = "";
		}

		curCollectButton.GetComponent<Collect_LoadCollectButtonInfo>().LoadInfo(collectData);
	}
}
