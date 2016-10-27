using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Import_ImportContextlMedia : MonoBehaviour {

	public Transform contentParent;
	public Object contextImgPrefab;


	public void OpenDialogue(string openMode)
	{
		string[] fileExtensions = new string[1]; //Filter Files
		if (openMode == "image")
		{
			fileExtensions[0] = "jpg";
			UniFileBrowser.use.SetFileExtensions(fileExtensions);
			UniFileBrowser.use.OpenFileWindow(OpenImage);
		}
		else if (openMode == "audio")
		{
			fileExtensions[0] = "ogg";
			UniFileBrowser.use.SetFileExtensions(fileExtensions);
//			UniFileBrowser.use.OpenFileWindow(OpenTexture);
		}
	}

	private void OpenImage(string pathToImage)
	{
		if (pathToImage.Length > 0)
		{
			StartCoroutine(ImportTexture("file://" + pathToImage));
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
		WWW www = new WWW(texLocation);

		while (!www.isDone){
			yield return null;
		}
		www.LoadImageIntoTexture(imgTexture);
		prefabRawImg.texture = imgTexture;
	}
}
