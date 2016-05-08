#pragma strict


//script for importing assets to be associated with imported objects

import UnityEngine.UI;

public var infoCont : ObjInfoControl;
public var contextMediaType : String = "Audio"; //new - used to send to media viewer

public var audioLocation : String;
public var contextAudio : GameObject;
private var contAudio: AudioSource;
public var contAudTitle : GameObject;

private var assetFromFile : boolean;

public var playTime : float = 0f;

var audFeedback : FeedbackScript; //***NEW***

function Awake(){

	contAudio = contextAudio.GetComponent(AudioSource);
	
	var gameControl = GameObject.FindGameObjectWithTag("GameController");
	infoCont = gameControl.GetComponent(ObjInfoControl);
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


public function OpenAudioAsset(){ //**REVISE** this function is pretty redundant
	
	//Filter Files
	var fileExtensions : String[] = [".ogg"]; //***NEW***
	UniFileBrowser.use.SetFileExtensions(fileExtensions); //***NEW***
	
	UniFileBrowser.use.OpenFileWindow(AudioAssetFile);
}

public function AudioAssetFile(pathToAudio : String){
	
	audioLocation = pathToAudio;
	ContextAudioImp();
	
	assetFromFile = true;
}



public function ContextAudioImp(){

	Debug.Log("Context Audio Located at: " + audioLocation);
		
	var wwwDirectory = "file://" + audioLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**

	var www : WWW = new WWW(wwwDirectory);
	
	while(!www.isDone){
	
		yield www;
	
		Debug.Log("Done Downloading Texture");
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}	
	
	var contAudioClip = www.GetAudioClip(false, false);
	
	contAudio.clip = contAudioClip;
	
	//Debug.Log("Ready to play");
	contAudio.Play();	

	
	if (assetFromFile){
		AudNameSplit(); //only execute namesplit if from file
		
		//execute add create node for the context media
		infoCont.CreateContextNode("Audio");
		infoCont.Save();  //**new** for autosave
		
		if (audFeedback == null) //***NEW***
			audFeedback = GameObject.Find("AddAudioButton").GetComponent.<FeedbackScript>(); //***NEW***
		audFeedback.Feedback(); //***NEW***
	}
}

function AudNameSplit(){

	var directorySplit : String[];
	directorySplit = audioLocation.Split("/"[0]);
	var audFileName = directorySplit[directorySplit.Length-1]; //original file name
	
	var audNameSplit = audFileName.Split("."[0]); 
	var audName = audNameSplit[audNameSplit.Length-2]; //object name minus extension
	
	infoCont.control.cntxtMediaName = audName;
	infoCont.control.cntxtMediaLocation = audioLocation;
	var audNameScript = contAudTitle.GetComponent(ContextAudTitle);
	audNameScript.AudName();
	
	
	Debug.Log("Context Audio Name: " + audName);
	
	assetFromFile = false; //reset after import from file
}

function sendActive() //used to activate media viewer
{
	var mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
	var mediaActiveScript = mediaViewer.GetComponent.<MetaPipe_MediaV_Activate>();

	var audTitle = contAudTitle.GetComponent.<Text>().text;
	Debug.Log("audTitle: " + audTitle);
	mediaActiveScript.activeMediaViewer(audTitle, contextMediaType, audioLocation);
}


