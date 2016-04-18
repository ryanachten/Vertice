#pragma strict


//script for importing assets to be associated with imported objects

import UnityEngine.UI;

//public var texLocation : String;

public var contextMediaType : String = "Video"; //new - used to send to media viewer
public var textureLocation : String; //new


public var contextVid : GameObject;
private var contVid : RawImage;
public var contVidTitle : GameObject;
private var vidTex : MovieTexture;
public var contVidTex : MovieTexture;
public var vidAudio : AudioSource; 

public var playTime : float = 0f;


function Awake(){

	contVid = gameObject.GetComponent(RawImage);

}


function FixedUpdate()
{	
	playTime += Time.deltaTime;
	
	if (playTime >= 3.0f) // && contAudio != null
	{	
		//Debug.Log("Stopping Audio");
		contVidTex.Stop();
	}
}


public function ContextVidImp(texLocation){
	
	textureLocation = texLocation;	
				
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**

	var www : WWW = new WWW(wwwDirectory);
	var videoTex = www.movie;
	
	while(!www.isDone){
	
		yield www;
	
		//Debug.Log("Done Downloading Texture");
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}	
	contVid.texture = videoTex;
	contVidTex = contVid.texture as MovieTexture;
	
	//Video Audio assign
	vidAudio.clip = contVidTex.audioClip;
	vidAudio.enabled = false;
	
	//Debug.Log("Ready to play");
	contVidTex.Play();
	//vidAudio.Play();	

}

function sendActive() //used to activate media viewer
{
	var mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
	var mediaActiveScript = mediaViewer.GetComponent.<MetaPipe_MediaV_Activate>();

	var vidTitle = contVidTitle.GetComponent.<Text>().text;
	Debug.Log("vidTitle: " + vidTitle);
	mediaActiveScript.activeMediaViewer(vidTitle, contextMediaType, textureLocation);
}