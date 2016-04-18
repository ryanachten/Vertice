#pragma strict

//held on MediaViewer empty GO

import UnityEngine.UI;

var mediaViewerPanel : GameObject;
var imageView : GameObject;


function activeMediaViewer( mediaType: String, mediaLocation : String)
{
	if (mediaViewerPanel.activeSelf == false)
	{
		mediaViewerPanel.SetActive(true);	
	}
	if (!imageView.activeSelf)
	{
		imageView.SetActive(true);
	}
	Debug.Log("Media Type: " + mediaType + " | Media Location: " + mediaLocation);
	
	if (mediaType == "Image")
	{
		loadContextImage(mediaLocation);
	}
	
	if (mediaType == "Video")
	{
		loadContextVideo(mediaLocation);
	}
	
	if (mediaType == "Audio")
	{
		loadContextAudio(mediaLocation);
	}
}


function deactivateMediaViewer()
{
	mediaViewerPanel.SetActive(false);	
}



function loadContextAudio(audioLocation : String)
{
	var wwwDirectory = "file://" + audioLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	var www : WWW = new WWW(wwwDirectory);
	
	while(!www.isDone){
		
		yield www;
		//Debug.Log("Done Downloading Texture");	
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}	
	
	var audClip = www.audioClip;		
		
	var audSrce : AudioSource = imageView.GetComponent.<AudioSource>(); 
	audSrce.clip = audClip;
	
	if (!audClip == null)
	{
		Debug.Log("!audClip == null");
	}
	//Debug.Log("Ready to play");
	audSrce.Play();

}




function loadContextVideo(texLocation : String)
{
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	var www : WWW = new WWW(wwwDirectory);
	
	var vidTex = www.movie;	
	
	while(!vidTex.isReadyToPlay){
		yield www;
		//Debug.Log("Done Downloading Texture");
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}
	
	//move img and vid resizing stuff into its own function
	var texWidth = vidTex.width;
	var texHeight = vidTex.height;
	Debug.Log("texWidth: " + texWidth + " texHeight: " + texHeight);
	
	//check img sizes
	var imgRect : RectTransform = imageView.GetComponent.<RectTransform>();
	var mvMaxWidth = imgRect.rect.width;
	var mvMaxHeight = imgRect.rect.height;
	//Debug.Log("mvMaxWidth: " + mvMaxWidth + " mvMaxHeight: " + mvMaxHeight);
	
	if (texHeight > mvMaxHeight) //if img height is greater than max mv max height
	{
		//Debug.Log("Img.height too big for MV.height: downscaling");
		
		var scaleHFactor = mvMaxHeight /texHeight;
		//Debug.Log("scaleHFactor: " + scaleHFactor);
		
		texHeight = texHeight * scaleHFactor;
		texWidth = texWidth * scaleHFactor;		
	}
	if (texWidth > mvMaxWidth) //if img width is greater than max mv max width
	{
		//Debug.Log("Img.width too big for MV.width: downscaling");
		
		var scaleWFactor = mvMaxWidth /texWidth;
		//Debug.Log("scaleWFactor: " + scaleWFactor);
		
		texHeight = texHeight * scaleWFactor;
		texWidth = texWidth * scaleWFactor;
	}
	
	imgRect.sizeDelta = new Vector2(texWidth, texHeight);
	
	
	//Video Tex assign
	var imgView : UI.RawImage = imageView.GetComponent.<RawImage>();
	imgView.texture = vidTex;
	
	//Video Audio assign
	var vidAudio : AudioSource = imageView.GetComponent.<AudioSource>(); 
	vidAudio.clip = vidTex.audioClip;

	//Debug.Log("Ready to play");
	vidTex.Play();
	vidAudio.Play();	
	
}


function loadContextImage(texLocation : String)
{	
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**	
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);

		yield www;
		
		var texWidth = www.texture.width;
		var texHeight = www.texture.height;
		
		//Debug.Log("texWidth: " + texWidth + " texHeight: " + texHeight);
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}

	//check img sizes
	var imgRect : RectTransform = imageView.GetComponent.<RectTransform>();
	var mvMaxWidth = imgRect.rect.width;
	var mvMaxHeight = imgRect.rect.height;
	//Debug.Log("mvMaxWidth: " + mvMaxWidth + " mvMaxHeight: " + mvMaxHeight);
	
	if (texHeight > mvMaxHeight) //if img height is greater than max mv max height
	{
		//Debug.Log("Img.height too big for MV.height: downscaling");
		
		var scaleHFactor = mvMaxHeight /texHeight;
		//Debug.Log("scaleHFactor: " + scaleHFactor);
		
		texHeight = texHeight * scaleHFactor;
		texWidth = texWidth * scaleHFactor;		
	}
	if (texWidth > mvMaxWidth) //if img width is greater than max mv max width
	{
		//Debug.Log("Img.width too big for MV.width: downscaling");
		
		var scaleWFactor = mvMaxWidth /texWidth;
		//Debug.Log("scaleWFactor: " + scaleWFactor);
		
		texHeight = texHeight * scaleWFactor;
		texWidth = texWidth * scaleWFactor;
	}
	
	
	//Assigning to RawImg comp
	var newImgTex = new Texture2D(texWidth, texHeight); //HERE - need to change this to be further down - create size using texWidth and texHeight
	www.LoadImageIntoTexture(newImgTex);
	var imageRender : UI.RawImage = imageView.GetComponent.<RawImage>();
	imageRender.texture = newImgTex;
	
	//Resize RawImg rect
	imgRect.sizeDelta = new Vector2(texWidth, texHeight);	
}


