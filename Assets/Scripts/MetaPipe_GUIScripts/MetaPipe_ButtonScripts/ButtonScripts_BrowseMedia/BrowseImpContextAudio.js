#pragma strict


//script for importing assets to be associated with imported objects

import UnityEngine.UI;

public var contextMediaType : String = "Audio"; //new - used to send to media viewer
public var audLocation : String; //new

//public var audioLocation : String;
public var contextAudio : GameObject;
private var contAudio: AudioSource;
public var contAudTitle : GameObject;

private var assetFromFile : boolean;

public var playTime : float = 0f;


function Awake(){

	contAudio = gameObject.GetComponent(AudioSource);
}

function FixedUpdate()
{
	playTime += Time.deltaTime;
	
	if (playTime >= 3.0f) // && contAudio != null
	{	
		//Debug.Log("Stopping Audio");
		contAudio.Stop();
	}
}

public function ContextAudioImp(audioLocation : String){

	audLocation = audioLocation;
		
	var wwwDirectory = "file://" + audioLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	var www : WWW = new WWW(wwwDirectory);
	
	while(!www.isDone){
	
		yield www;
		//Debug.Log("Done Downloading Texture");	
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}	
	
	var contAudioClip = www.GetAudioClip(false, false);
	
	contAudio.clip = contAudioClip;
	
	//Debug.Log("Ready to play");
	contAudio.Play();

}

function sendActive() //used to activate media viewer
{
	var mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
	var mediaActiveScript = mediaViewer.GetComponent.<MetaPipe_MediaV_Activate>();

	var audTitle = contAudTitle.GetComponent.<Text>().text;
	Debug.Log("audTitle: " + audTitle);
//	mediaActiveScript.activeMediaViewer(audTitle, contextMediaType, audLocation);
}