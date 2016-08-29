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
	public Object imagePrefab; //TODO something is wrong with this prefab setup - revise rectTransforms
	public Object audioPrefab;
	public Object videoPrefab;


	void Start()
	{
		LoadMedia(TestIdent);
	}

//	TODO this current process isn't ideal as the toggles are required to
//	change what media is required, however they can't pass the identifier
//	on the fly. Need to store the identifier somewhere to allow this
//	perhaps have a dedicated public var in this script - too unstable?

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
			Dictionary<string, string>[] media = DublinCoreReader.GetContextualMediaArtefactWithIdentifierAndType(identifier, mediaType); 

			for (int i = 0; i < media.Length; i++) 
			{
				string mediaName = media[i]["MediaName"];
				string mediaLocation = media[i]["MediaLocation"];
				Debug.Log(mediaName);
				Debug.Log(mediaLocation);

				GameObject mediaInstant = Object.Instantiate(mediaPrefab, contentParent) as GameObject;

				Text mediaText = mediaInstant.transform.GetChild(1).gameObject.GetComponent<Text>(); //updates the prefab title
				mediaText.text = mediaName;

				//TODO write link between XML mediaLocation and prefab import script
			}
		}
		catch(System.Exception ex)
		{
			Debug.Log("No Contextual Media for this artefact");
			//TODO create feedback prefab for when not media to view
		}
	}
		




}
