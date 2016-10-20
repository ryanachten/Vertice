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
	{

		#if UNITY_WEBGL
		var wwwDirectory = Paths.Remote + texLocation; 
		#elif UNITY_STANDALONE
		var wwwDirectory = Paths.Local + texLocation; //Doesn't work due to the VerticeArchive folder residing outside of Assets folder
		#endif

		textureLocation = wwwDirectory;

		WWW www = new WWW(wwwDirectory);
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
	public void sendActive()
	{
		GameObject mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
		MediaView_Control mediaActiveScript = mediaViewer.GetComponent<MediaView_Control>();

		string imgTitle = contImgTitle.GetComponent<Text>().text;
//		Debug.Log("activeMediaViewer imgTitle: " + imgTitle + " contextMediaType: " + contextMediaType + " textureLocation: " + textureLocation);
		mediaActiveScript.activeMediaViewer(imgTitle, contextMediaType, textureLocation);
	}
}
