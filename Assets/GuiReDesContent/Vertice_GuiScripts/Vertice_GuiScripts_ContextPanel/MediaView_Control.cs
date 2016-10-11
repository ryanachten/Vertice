using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MediaView_Control : MonoBehaviour {

	public GameObject mediaViewerPanel;
	public GameObject imageView;
	public Text mediaTitle;
	public RawImage imageRender;
	public RectTransform imgRect;
	private float defaultMvWidth = 900f; //TODO Should use screen.size probs
	private float defaultMvHeight = 500f; //TODO as above
	public GameObject mediaButtons;


	//TODO currently only contextual images supported; consider audio and video formats 

	public void activeMediaViewer(string mediaName, string mediaType, string mediaLocation)
	{
		if (mediaViewerPanel.activeSelf == false)
			mediaViewerPanel.SetActive(true);	

		if (!imageView.activeSelf)
			imageView.SetActive(true);

		/*//Reset windows sizes
		float mvMaxWidth = imgRect.rect.width;
		if (mvMaxWidth != defaultMvWidth)
			imgRect.sizeDelta = new Vector2(defaultMvWidth, defaultMvHeight);

		float mvMaxHeight = imgRect.rect.height;
		if (mvMaxHeight != defaultMvHeight)
			imgRect.sizeDelta = new Vector2(defaultMvWidth, defaultMvHeight);*/

		mediaTitle.text = mediaName;

		//Activate Loads
		if (mediaType == "Image")
		{
			StartCoroutine(loadContextImage(mediaLocation));
			mediaButtons.SetActive(false);
		}			
	}


	public void deactivateMediaViewer()
	{
		mediaViewerPanel.SetActive(false);	
	}
		


	IEnumerator loadContextImage(string texLocation)
	{
		#if UNITY_WEBGL
//		string wwwDirectory = Paths.Remote + texLocation;	//handled in BrowseImpContextImg
		#endif

		string wwwDirectory = texLocation;
		WWW www = new WWW(wwwDirectory);
		while(!www.isDone){
			yield return www;
			assignImgToTex(www);
		}
	}


	void assignImgToTex(WWW www)
	{
		float texWidth = www.texture.width;
		float texHeight = www.texture.height;
		Debug.Log("texDimen: " + texWidth + " : " + texHeight);

		Texture2D newImgTex = new Texture2D(Mathf.RoundToInt(texWidth), Mathf.RoundToInt(texHeight)); 
		www.LoadImageIntoTexture(newImgTex);
		imageRender.texture = newImgTex;

		//Resize RawImg rect
//		imgRect.sizeDelta = new Vector2(texWidth, texHeight);
		downScaleRect(newImgTex);
	}

	void downScaleRect(Texture2D imgTex) //FIXME scaling fucntion still not working as expected
	{
		Debug.Log("ImgRect: " + imgRect.rect.width + " / " + imgRect.rect.height);

		float texHeight = imgTex.height;
		float texWidth = imgTex.width;

		float viewHeight = mediaViewerPanel.GetComponent<RectTransform>().rect.height;
		float viewWidth = mediaViewerPanel.GetComponent<RectTransform>().rect.width;

		float heightAspect = 100;
		float widthAspect = 100;

		if (texWidth > viewHeight && texWidth > texHeight)
		{
			Debug.Log("Image is portrait and bigger than viewer");

			Debug.Log("texWidth: " + texWidth + " viewWidth: " + viewWidth);
			widthAspect = texWidth - viewWidth;
			Debug.Log("widthAspect: " + widthAspect);
			widthAspect += 100;
		}
		imgRect.sizeDelta = new Vector2(-widthAspect, -heightAspect);
	}
}