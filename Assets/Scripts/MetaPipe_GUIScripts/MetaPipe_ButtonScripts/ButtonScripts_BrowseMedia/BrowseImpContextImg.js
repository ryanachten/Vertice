#pragma strict

//script for importing img to Browse GUI prefab

import UnityEngine.UI;

public var contextMediaType : String = "Image"; //new - used to send to media viewer
public var textureLocation : String; //new



public var contextImg : GameObject;
private var contImg : RawImage;
public var contImgTitle : GameObject;
private var imgTex : Texture2D;
private var imgRectTrans : RectTransform;
public var newImgTex : Texture2D;
private var newImgSprite : Sprite;

private var assetFromFile : boolean;



function Awake(){

	contImg = contextImg.GetComponent(RawImage);
	imgRectTrans = contextImg.GetComponent(RectTransform);	
}


public function ContextImgImp(texLocation){

	textureLocation = texLocation;

	#if UNITY_WEBGL
	var wwwDirectory = Paths.Remote + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	#elif UNITY_STANDALONE
	var wwwDirectory = Paths.Local + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	#endif

	var www : WWW = new WWW(wwwDirectory);
	Debug.Log("Downloading contextual image: " + wwwDirectory);
	while(!www.isDone){
		yield www;
	}

	var texWidth = www.texture.width;
	var texHeight = www.texture.height;

	//check img sizes
	var imgRect : RectTransform = contextImg.GetComponent.<RectTransform>();
	var mvMaxWidth = imgRect.rect.width;
	var mvMaxHeight = imgRect.rect.height;
//	Debug.Log("mvMaxWidth: " + mvMaxWidth + " mvMaxHeight: " + mvMaxHeight);
	
	if (texHeight > mvMaxHeight) //if img height is greater than max mv max height
	{
//		Debug.Log("Img.height too big for MV.height: downscaling");
		
		var scaleHFactor = mvMaxHeight /texHeight;
		//Debug.Log("scaleHFactor: " + scaleHFactor);
		
		texHeight = texHeight * scaleHFactor;
		texWidth = texWidth * scaleHFactor;			
	}
	if (texWidth > mvMaxWidth) //if img width is greater than max mv max width
	{
//		Debug.Log("Img.width too big for MV.width: downscaling");
		
		var scaleWFactor = mvMaxWidth /texWidth;
		//Debug.Log("scaleWFactor: " + scaleWFactor);
		
		texHeight = texHeight * scaleWFactor;
		texWidth = texWidth * scaleWFactor;
	}
	
	
	//Assigning to RawImg comp
	var newImgTex = new Texture2D(texWidth, texHeight); //HERE - need to change this to be further down - create size using texWidth and texHeight
	www.LoadImageIntoTexture(newImgTex);
	var imageRender : UI.RawImage = contextImg.GetComponent.<RawImage>();
	imageRender.texture = newImgTex;	
	
	//Resize RawImg rect
	var layoutElement = contextImg.GetComponent.<LayoutElement>();
	layoutElement.minHeight = texHeight;	
}



function sendActive() //used to activate media viewer
{
	var mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
	var mediaActiveScript = mediaViewer.GetComponent.<MetaPipe_MediaV_Activate>();

	var imgTitle = contImgTitle.GetComponent.<Text>().text;
	Debug.Log("imgTitle: " + imgTitle);
	mediaActiveScript.activeMediaViewer(imgTitle, contextMediaType, textureLocation);
}