#pragma strict

#if UNITY_WEBGL

//held on MediaViewer empty GO

import UnityEngine.UI;

var mediaViewerPanel : GameObject;
var imageView : GameObject;
var mediaTitle : Text;
var imageRender : UI.RawImage;

#if UNITY_WEBGL
var vidTex = null;
#else
var vidTex : MovieTexture;
#endif

var audSrce : AudioSource;
var audImg : Texture2D;

var imgRect : RectTransform;
private var defaultMvWidth : float = 900f; //dunno if this system will work for responsive resizing - might need a duplicate of imgRect that doesn't change
private var defaultMvHeight : float = 500f; //as above

var mediaButtons : GameObject;
var mediaButtonCont : MetaPipe_MediaV_ButonCont;


function activeMediaViewer( mediaName : String, mediaType: String, mediaLocation : String)
//function activeMediaViewer( mediaType: String, mediaLocation : String)
{
	if (mediaViewerPanel.activeSelf == false)
		mediaViewerPanel.SetActive(true);	
		
	if (!imageView.activeSelf)
		imageView.SetActive(true);

	#if UNITY_WEBGL
	Debug.LogError("MetaPipe_MediaV_Activate [vidTex.Stop] not implemented in WebGL in activeMediaViewer");
//	if (vidTex != null && vidTex.isPlaying)
//		vidTex.Stop();
	#endif

		
	if (audSrce.isPlaying)
		audSrce.Stop();

	
	//Reset windows sizes
	var mvMaxWidth = imgRect.rect.width;
	if (mvMaxWidth != defaultMvWidth)
		imgRect.sizeDelta = new Vector2(defaultMvWidth, defaultMvHeight);
		
	var mvMaxHeight = imgRect.rect.height;
	if (mvMaxHeight != defaultMvHeight)
		imgRect.sizeDelta = new Vector2(defaultMvWidth, defaultMvHeight);
	
	mediaTitle.text = mediaName;
	
	//Activate Loads
	if (mediaType == "Image")
		loadContextImage(mediaLocation);
	
	if (mediaType == "Video")
		loadContextVideo(mediaLocation);
	
	if (mediaType == "Audio")
		loadContextAudio(mediaLocation);
	
	//Media Button control
	if (mediaType == "Video" || mediaType == "Audio")
		mediaButtons.SetActive(true);
	
	else if (mediaType == "Image")
		mediaButtons.SetActive(false);
}


function deactivateMediaViewer()
{

	#if UNITY_WEBGL
	Debug.LogError("MetaPipe_MediaV_Activate [vidTex.Stop] not implemented for WebGL in deactivateMediaViewer");
//	if (vidTex!= null && vidTex.isPlaying)
//		vidTex.Stop(); //prevents videos running in the bg consuming CPU
	#endif

	if (audSrce.isPlaying){
		audSrce.Stop();
	}

	mediaViewerPanel.SetActive(false);	
}

#if UNITY_WEBGL
function loadContextVideo(texLocation : String)
{
	Debug.LogError("MetaPipe_MediaV_Activate.loadContextVideo not implemented in WebGL");
	Debug.Break();
}
#else


function loadContextVideo(texLocation : String)
{
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	var www : WWW = new WWW(wwwDirectory);
	
	vidTex = www.movie;

	while(!www.isDone && !vidTex.isReadyToPlay){ //answer coroutine
		yield www;
		if (www.isDone && vidTex.isReadyToPlay)
		{
			break;
		}
	}

	var texWidth = vidTex.width;
	var texHeight = vidTex.height;
//	Debug.Log("texWidth: " + texWidth + " texHeight: " + texHeight);
	
	//check img sizes
	var mvMaxWidth = imgRect.rect.width;
	var mvMaxHeight = imgRect.rect.height;
//	Debug.Log("mvMaxWidth: " + mvMaxWidth + " mvMaxHeight: " + mvMaxHeight);
	
	if (texHeight > mvMaxHeight)
	{
		var scaleHFactor = mvMaxHeight /texHeight;
		texHeight = texHeight * scaleHFactor;
		texWidth = texWidth * scaleHFactor;		
	}
	if (texWidth > mvMaxWidth)
	{
		var scaleWFactor = mvMaxWidth /texWidth;
		texHeight = texHeight * scaleWFactor;
		texWidth = texWidth * scaleWFactor;
	}
	imgRect.sizeDelta = new Vector2(texWidth, texHeight);
	
	//Video Tex assign
	var imgView : UI.RawImage = imageView.GetComponent.<RawImage>(); //method 01 [using local variables]
//	var imgView = imageRender; //method 02 [using public variables]
	imgView.texture = vidTex;
	
	//Video Audio assign
	audSrce.clip = vidTex.audioClip;

	//Activate Media Controls
	mediaButtonCont.setMedia("Video");	
}
#endif


function loadContextAudio(audioLocation : String)
{

	#if UNITY_WEBGL
	var BASE_URL = "https://s3-ap-southeast-2.amazonaws.com/vertice-dev";
	var wwwDirectory = BASE_URL + audioLocation;
	#else
	var wwwDirectory = "file://" + audioLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	#endif
	var www : WWW = new WWW(wwwDirectory);
	
	while(!www.isDone){
		yield www;
	}
	Debug.Log("Loaded audio file from: " + wwwDirectory);
	
	var audClip = www.audioClip;		
	audSrce.clip = audClip;
	Debug.Log("Loaded audio in to audSrce");

	imageRender.texture = audImg;

	mediaButtonCont.setMedia("Audio");
}

function loadContextImage(texLocation : String)
{	

	#if UNITY_WEBGL
	var BASE_URL = "https://s3-ap-southeast-2.amazonaws.com/vertice-dev";
	var wwwDirectory = BASE_URL + texLocation;	
	#else
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**	
	#endif

	while(true){
	
		var www : WWW = new WWW(wwwDirectory);
		yield www;
		
		var texWidth = www.texture.width;
		var texHeight = www.texture.height;

		if (www.isDone)
			break; //if done downloading image break loop
	}
	//check img sizes
	var mvMaxWidth = imgRect.rect.width;
	var mvMaxHeight = imgRect.rect.height;
	
	if (texHeight > mvMaxHeight) //if img height is greater than max mv max height
	{
		//Debug.Log("Img.height too big for MV.height: " + texHeight + " > " + mvMaxHeight);
		var scaleHFactor = mvMaxHeight /texHeight;
		texHeight = texHeight * scaleHFactor;
		texWidth = texWidth * scaleHFactor;		
	}
	if (texWidth > mvMaxWidth) //if img width is greater than max mv max width
	{
		//Debug.Log("Img.width too big for MV.width: " + texWidth + " > " + mvMaxWidth);
		var scaleWFactor = mvMaxWidth /texWidth;
		texHeight = texHeight * scaleWFactor;
		texWidth = texWidth * scaleWFactor;
	}
	
	//Assigning to RawImg comp	
	var newImgTex = new Texture2D(texWidth, texHeight); //HERE - need to change this to be further down - create size using texWidth and texHeight
	www.LoadImageIntoTexture(newImgTex);
	imageRender.texture = newImgTex;
	
	//Resize RawImg rect
	imgRect.sizeDelta = new Vector2(texWidth, texHeight);	
}