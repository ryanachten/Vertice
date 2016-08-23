#pragma strict

import UnityEngine.UI;

var mvActivateScript : MetaPipe_MediaV_Activate;
var audSrce : AudioSource;

var playButton : Button;
var stopButton : Button;
var replayButton : Button;

var curMediaType : String;
var curVid : MovieTexture;


function setMedia( mediaType : String)
{
	curMediaType = mediaType;

	#if UNITY_WEBGL
	Debug.Log("MetaPipe_MediaV_ButonCont.setMedia not implemented for video");
	#else
	if (mediaType == "Video")
	{	
		curVid = mvActivateScript.vidTex;
		audSrce = mvActivateScript.audSrce;
	}
	#endif
	if (mediaType == "Audio")
	{
		Debug.Log("Trying to access audSrce");
		audSrce = mvActivateScript.audSrce;
	}
}


function playMedia()
{

	if (curMediaType == "Audio"){
		audSrce.Play();
	}
	#if UNITY_WEBGL
	Debug.Log("MetaPipe_MediaV_ButonCont.setMedia not implemented for video");
	#else
	else if (curMediaType == "Video")
	{
		curVid.Play();
		audSrce.Play();
	}
	#endif
		
}

function pauseMedia()
{

	if (curMediaType == "Audio")
		audSrce.Pause();
	#if UNITY_WEBGL
	Debug.Log("MetaPipe_MediaV_ButonCont.setMedia not implemented for video");
	#else
	else if (curMediaType == "Video")
	{
		curVid.Pause();
		audSrce.Pause();
	}
	#endif

}

#if UNITY_WEBGL

function replayMedia()
{
	Debug.LogError("MetaPipe_MediaV_ButonCont.replayMedia not implemented in WebGL");
	Debug.Break();
}
#else
function replayMedia()
{
	if (curMediaType == "Video")
	{
		curVid.Stop();
		audSrce.Stop();
		
		curVid.Play();
		audSrce.Play();
	}
	else if (curMediaType == "Audio")
	{
		audSrce.Stop();
		audSrce.Play();
	}
}
#endif