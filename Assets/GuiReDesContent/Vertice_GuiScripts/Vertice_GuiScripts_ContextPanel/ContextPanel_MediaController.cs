using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ContextPanel_MediaController : MonoBehaviour {

	public string TestIdent = "TestMonk";
	public Toggle imagesToggle;
	public Toggle audioToggle;
	public Toggle videoToggle;

	public Transform contentParent;
	public Object imagePrefab;
	public Object audioPrefab;
	public Object videoPrefab;


	void Start()
	{
		LoadMedia(TestIdent);
	}

	
	public void LoadMedia(string artefactIdentifier)
	{
		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");

		ResetPanel();

		if (imagesToggle.isOn)
		{
			InstantMedia(artefactIdentifier, "Image");
		}
		else if (audioToggle.isOn)
		{
			InstantMedia(artefactIdentifier, "Audio");
		}
		else if (videoToggle.isOn)
		{
			InstantMedia(artefactIdentifier, "Video");
		}
	}


	private void ResetPanel()
	{
		for (int i = 0; i < contentParent.childCount; i++) 
		{
			GameObject contentChild = contentParent.GetChild(i).gameObject;	
			Destroy(contentChild);
		}
	}


	private void InstantMedia(string identifier, string mediaType)
	{
		Dictionary<string, string>[] media = DublinCoreReader.GetContextualMediaArtefactWithIdentifierAndType(identifier, mediaType); 

		Object mediaPrefab = new Object();
		if (mediaType == "Image")
		{
			mediaPrefab = imagePrefab;
		} 
		else if (mediaType == "Audio")
		{
			mediaPrefab = audioPrefab;
		}
		else if (mediaType == "Video")
		{
			mediaPrefab = videoPrefab;
		}

		try {
			for (int i = 0; i < media.Length; i++) 
			{
				string mediaName = media[i]["MediaName"];
				string mediaLocation = media[i]["MediaLocation"];  //Access first image and its media location
				Debug.Log(mediaName);
				Debug.Log(mediaLocation);

				Object.Instantiate(mediaPrefab, contentParent);
			}
		}
		catch(System.Exception ex)
		{
			Debug.Log("No Contextual Media for this artefact");
		}
	}
		




}
