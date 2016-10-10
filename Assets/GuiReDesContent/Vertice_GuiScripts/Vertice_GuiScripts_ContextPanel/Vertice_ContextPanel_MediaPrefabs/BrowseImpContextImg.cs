using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BrowseImpContextImg : MonoBehaviour {

	public GameObject contextImg;
	public GameObject contImgTitle;

	private string contextMediaType = "Image";
	private string textureLocation;

	/// <summary>
	/// Coroutine for importing and scaling jpg image to contextual media prefab
	/// </summary>
	/// <returns>Image 2D texture</returns>
	/// <param name="texLocation">Texture directory URI.</param>
	public IEnumerator ContextImgImp (string texLocation)
//	public void ContextImgImp(string texLocation)
	{
		textureLocation = texLocation;

		#if UNITY_WEBGL
//		var wwwDirectory = Paths.Remote + texLocation; 
		string wwwDirectory = "file://" + Application.dataPath + "/../.." + texLocation; //TODO test directory - VerticeArchive shipped outside of Assets folder
		#elif UNITY_STANDALONE
		var wwwDirectory = Paths.Local + texLocation; //Doesn't work due to the VerticeArchive folder residing outside of Assets folder
		#endif


		WWW www = new WWW(wwwDirectory);
//		Debug.Log("Downloading contextual image: " + wwwDirectory);
		while(!www.isDone){
			yield return www; //TODO not downloading all of the data before continuing
		}

		float texWidth = www.texture.width;
		float texHeight = www.texture.height;

		//check img sizes
		RectTransform imgRect = contextImg.GetComponent<RectTransform>();
		float mvMaxWidth = imgRect.rect.width;
		float mvMaxHeight = imgRect.rect.height;

		if (texHeight > mvMaxHeight) //if img height is greater than max mv max height
		{
			float scaleHFactor = mvMaxHeight /texHeight;

			texHeight = texHeight * scaleHFactor;
			texWidth = texWidth * scaleHFactor;			
		}
		if (texWidth > mvMaxWidth) //if img width is greater than max mv max width
		{
			var scaleWFactor = mvMaxWidth /texWidth;
			texHeight = texHeight * scaleWFactor;
			texWidth = texWidth * scaleWFactor;
		}

		//Assigning to RawImg comp
		Texture2D newImgTex = new Texture2D(Mathf.RoundToInt(texWidth), Mathf.RoundToInt(texHeight));
		www.LoadImageIntoTexture(newImgTex);
		RawImage imageRender = contextImg.GetComponent<RawImage>();
		imageRender.texture = newImgTex;	

		//Resize RawImg rect
		LayoutElement layoutElement = contextImg.GetComponent<LayoutElement>();
		layoutElement.minHeight = texHeight;	
	}

	/// <summary>
	/// Activates the MediaViewer for viewing cotextual media
	/// </summary>
	void sendActive()
	{
		GameObject mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
		MetaPipe_MediaV_Activate mediaActiveScript = mediaViewer.GetComponent<MetaPipe_MediaV_Activate>(); //TODO need to refactor the MediaViewer script

		string imgTitle = contImgTitle.GetComponent<Text>().text;
		Debug.Log("imgTitle: " + imgTitle);
		mediaActiveScript.activeMediaViewer(imgTitle, contextMediaType, textureLocation);
	}
}
