using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ContextPanel_MediaController : MonoBehaviour {

	public ContextPanel_InfoController ContextInfoCont;
	public Toggle imagesToggle;
	public Toggle audioToggle;
	public Toggle videoToggle;

	public Transform contentParent;
	public Object imagePrefab; //TODO something is wrong with this prefab setup - revise rectTransforms
	public Object audioPrefab;
	public Object videoPrefab;
	public Object noMediaPrefab;


//	TODO this current process isn't ideal the ID is referenced from the TestController script
	//this could later be a 'Panel Controller', however not sure if this is the best approach
	//(i.e. relying on a public var too unstable?)


	/// <summary>
	/// Detects which user defined toggles are active for viewing media
	/// </summary>
	public void LoadMedia() //executed on pressing Media button
	{
		StartCoroutine (LoadMediaAsync);

	}

	/// <summary>
	/// Provides a backing for LoadMedia() that can load data in to the DublinCoreReader asynchronously in the case 
	/// where the DCReader has yet to be populated with data
	/// </summary>
	IEnumerator LoadMediaAsync(){

		// If the DublinCoreReader has not been 
		if (!DublinCoreReader.HasXml ()) {
			UnityWebRequest www = UnityWebRequest.Get (Paths.Remote + "/Metadata/Vertice_ArtefactInformation.xml");
			yield return www.Send ();

			if (www.isError) {
				// TODO: Echo the error condition to the user
				Debug.Log ("Couldn't download XML file" + www.error);
			} else {
				DublinCoreReader.LoadXmlFromText (www.downloadHandler.text);
				Debug.Log("Downloaded some XML");
			}
		}

		string artefactId = ContextInfoCont.artefactId;

		ResetPanel();

		if (imagesToggle.isOn)
		{
			InstantMedia(artefactId, "Image");
		}
		else if (audioToggle.isOn)
		{
			InstantMedia(artefactId, "Audio");
		}
		else if (videoToggle.isOn)
		{
			InstantMedia(artefactId, "Video");
		}
		
	}

	/// <summary>
	/// Resets the media panel between artefacts
	/// </summary>
	private void ResetPanel()
	{
		for (int i = 0; i < contentParent.childCount; i++) 
		{
			GameObject contentChild = contentParent.GetChild(i).gameObject;	
			Destroy(contentChild);
		}
	}

	/// <summary>
	/// Instantiates media prefabs depending on media type
	/// </summary>
	/// <param name="identifier">Identifier of artefact.</param>
	/// <param name="mediaType">Media type of media to be instantiated.</param>
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
//			Debug.Log("identifier: " + identifier + " mediaType: " + mediaType);
			Dictionary<string, string>[] media = DublinCoreReader.GetContextualMediaArtefactWithIdentifierAndType(identifier, mediaType); 

			for (int i = 0; i < media.Length; i++) 
			{
				string mediaName = media[i]["MediaName"];
				string mediaLocation = media[i]["MediaLocation"];
//				Debug.Log(mediaName);
//				Debug.Log(mediaLocation);

				GameObject mediaInstant = Object.Instantiate(mediaPrefab, contentParent) as GameObject;

				Text mediaText = mediaInstant.transform.GetChild(1).gameObject.GetComponent<Text>(); //updates the prefab title
				mediaText.text = mediaName;

				if (mediaType == "Image")
				{
					BrowseImpContextImg imgImpScript = mediaInstant.GetComponentInChildren<BrowseImpContextImg>();
					
//					Debug.Log("mediaLocation: " + mediaLocation);
					StartCoroutine(imgImpScript.ContextImgImp(mediaLocation)); //TODO this coroutine is not working properly
				} 
//				else if (mediaType == "Audio")
//				{
//					mediaPrefab = audioPrefab;
//				}
//				else if (mediaType == "Video")
//				{
//					mediaPrefab = videoPrefab;
//				}
			}
		}
		catch(System.Exception ex)
		{
			Debug.Log("No Contextual Media for this artefact");
			GameObject mediaInstant = Object.Instantiate(noMediaPrefab, contentParent) as GameObject;
		}
	}
}
