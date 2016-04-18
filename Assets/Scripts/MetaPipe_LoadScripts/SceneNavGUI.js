#pragma strict

static var curScene : int;
static var prevScene : int;
//static var returnToBrowse : boolean;
//static var returnToCollection : boolean;

//GUI assets
var importSceneButton : Button;
var browseSceneButton : Button;
var collectionSceneButton : Button;
//var returnToBrowseButton : GameObject;
//var returnToCollectionButton : GameObject;


function OnGUI()
{
	curScene =  Application.loadedLevel;
	
	if (prevScene != curScene) //not on the same 
	{
		updateCurScene();
		//activateContextLinks();		
	}
	
	prevScene = curScene;
}


function updateCurScene()
{
	//Direct Links
	if (curScene == 0) //browse (main) scene
	{
		browseSceneButton.interactable = false;
		importSceneButton.interactable = true;
		collectionSceneButton.interactable = true;
	}
			
	else if (curScene == 1) //import scene
	{
		importSceneButton.interactable = false;
		
		browseSceneButton.interactable = true;
		collectionSceneButton.interactable = true;
	}
	
	else if (curScene == 2) //collection scene
	{
		collectionSceneButton.interactable = false;
		
		browseSceneButton.interactable = true;
		importSceneButton.interactable = true;
	}
//	Debug.Log("prevScene: " + prevScene + " curScene: " + curScene);	
}
/*
//now managed by by curObj GUI instead of being automatic due to dependancies required
function activateContextLinks()
{
	//Contextual Links
	
	//Return to Browse
	if (prevScene == 0 && returnToBrowse == true)
	{
		returnToBrowseButton.SetActive(true);
	}
	else
	{
		returnToBrowseButton.SetActive(false);
		returnToBrowse = false;
	}
	
	//Return to Collection
	if (prevScene == 2 && returnToBrowse)
	{
		returnToCollectionButton.SetActive(true);
	}
	else
	{
		returnToCollectionButton.SetActive(false);
	}
}*/