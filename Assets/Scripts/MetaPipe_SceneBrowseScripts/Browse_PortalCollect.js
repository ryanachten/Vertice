#pragma strict

var loadScript : MetaPipe_LoadScene;


function OnTriggerEnter()
{
//	Debug.Log("Load Import Scene");
	loadScript.loadCollectionScene();
}