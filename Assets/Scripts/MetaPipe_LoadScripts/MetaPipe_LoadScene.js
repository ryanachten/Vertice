#pragma strict

//used for navigating between scenes
//also sets up prerequisites for ea. scene


var collectionControl : ObjCollectionControl;
var objInfoControl : ObjInfoControl;


function Start()
{
	var gc : GameObject;
	if (collectionControl == null)
	{
		gc = GameObject.FindGameObjectWithTag("GameController");
		collectionControl = gc.GetComponent(ObjCollectionControl);
	}
	if (objInfoControl == null)
	{
		gc = GameObject.FindGameObjectWithTag("GameController");
		objInfoControl = gc.GetComponent(ObjInfoControl);
	}
	
	
	var curLevel = Application.loadedLevel;
	
	//preps collection scene
	if (curLevel == 2)
	{
		collectionControl.Start();
		objInfoControl.Start();
	}	
}


function loadImportScene()
{
	Application.LoadLevel("MetaPipe_ImportScene");	
}


function loadBrowseScene()
{	
	Application.LoadLevel("MetaPipe_BrowseScene");
}


function loadCollectionScene()
{	
	Application.LoadLevel("MetaPipe_CollectionScene");
}


function OnLevelWasLoaded(level : int)
{
	//gets rid of importCam which seems to persist
	if (level != 1) //all scenes except import scene
	{
		try {
			var importCam : GameObject = GameObject.Find("ImportMainCam");
			if (importCam.activeInHierarchy){
				Destroy(importCam);
			}
		}catch(err){ 
		//don't do anything if the imp cam isn't here
		}

		//objects persisting from import scene when !isImpBrowse
		if (objInfoControl == null)
		{
			Debug.Log("ObjInfoCont Lost - Finding");
			objInfoControl = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();		
		}
		if (!objInfoControl.isImpBrowse)
		{
			var curObj = GameObject.FindGameObjectWithTag("Current Model");
				if (curObj != null)
			{
				Debug.Log("Deleting: " + curObj.name);
				Destroy(curObj);
			}
		}
	}
	
	if (level == 2) // browse scene -> this is actually collection scene; what's going on?
	{
		if (collectionControl == null)
		{
			collectionControl = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjCollectionControl>();
		}
	
		if (collectionControl.collectionList == null)
		{
			collectionControl.Start();
		}
	}
	
	if (level == 1) //import scene
	{
		if (objInfoControl == null)
		{
			objInfoControl = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
		}	
//		if (objInfoControl.root == null)
//		{
//			Debug.Log("Root is null for Import - Start()ing");
			objInfoControl.Start();
			Cursor.visible = true;
//		}
	
	}
}