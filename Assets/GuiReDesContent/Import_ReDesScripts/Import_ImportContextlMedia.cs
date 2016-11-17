using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Import_ImportContextlMedia : MonoBehaviour {

	public Transform contentParent;
	public Object contextImgPrefab;
	public GameObject imageErrorFeedback;


	public void OpenDialogue(string openMode)
	{
		if (ArtefactSaveData.MeshLocation != null)
		{
			string[] fileExtensions = new string[1]; //Filter Files
			if (openMode == "image")
			{
				fileExtensions[0] = "jpg";
				UniFileBrowser.use.SetFileExtensions(fileExtensions);
				UniFileBrowser.use.OpenFileWindow(OpenImage);
			}
		}
		else 
		{
			StartCoroutine(ErrorFeedback("modelFirst"));
		}
	}

	private void OpenImage(string pathToImage)
	{
		if (pathToImage.Length > 0)
		{
			try 
			{
				int verticeArchiveIndex = pathToImage.IndexOf("/VerticeArchive");
				string verticeArchiveSubstring = pathToImage.Substring(verticeArchiveIndex);

				StartCoroutine(ImportTexture(verticeArchiveSubstring));
			} 
			catch (System.Exception ex) 
			{
				Debug.Log("Image not in VerticeArchive folder");
				StartCoroutine(ErrorFeedback("imgLocation"));
			}

		}
	}


	IEnumerator ImportTexture( string texLocation)
	{
		GameObject imgPrefab = Instantiate(contextImgPrefab, contentParent) as GameObject;
		imgPrefab.transform.SetAsFirstSibling();
		imgPrefab.transform.localScale = new Vector3(1,1,1);
		Texture2D imgTexture = new Texture2D (512, 512);
		RawImage prefabRawImg = imgPrefab.GetComponentInChildren<RawImage>();

		// Download texture
		WWW www = new WWW(Paths.PathToFile(texLocation));

		while (!www.isDone){
			yield return null;
		}
		www.LoadImageIntoTexture(imgTexture);
		prefabRawImg.texture = imgTexture;

		string mediaName;
		DefaultNameSplit(texLocation, out mediaName);
		imgPrefab.GetComponentInChildren<Text>().text = mediaName;

		Dictionary<string, string> contextMediaDictionary = new Dictionary<string, string>();
		contextMediaDictionary.Add("MediaName", mediaName);
		contextMediaDictionary.Add("MediaType", "Image");
		contextMediaDictionary.Add("MediaLocation", texLocation);

		ArtefactSaveData.ContextualMediaAssets.Add(contextMediaDictionary);
	}


	private void DefaultNameSplit(string texLocation, out string mediaName)
	{
		int splitIndex = texLocation.LastIndexOf("/") + 1;
		int endIndex = texLocation.LastIndexOf(".");
		int subStrLength = endIndex - splitIndex;

		string nameSubstring = texLocation.Substring(splitIndex, subStrLength);

		mediaName = nameSubstring;
	}

	IEnumerator ErrorFeedback(string errorType)
	{
		if (errorType == "imgLocation")
		{
			imageErrorFeedback.GetComponent<Text>().text = "Image not in your VerticeArchive folder";
		}
		else 
		{
			//Import mesh first
			imageErrorFeedback.GetComponent<Text>().text = "Import Model First";
		}

		imageErrorFeedback.SetActive(true);

		yield return new WaitForSeconds(3);

		imageErrorFeedback.SetActive(false);
	}
}