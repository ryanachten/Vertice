#pragma strict

import System.Collections.Generic;

var infoCont : ObjInfoControl;
var rayDetectScript : BrowseRayDetectObj;
var browseXml : BrowseContextInfoXml;


function editViewInfo()
{
	var curObj : GameObject = rayDetectScript.curObj;
	loadImportCurObj(curObj);
}


function loadImportCurObj(curObj : GameObject){

	//used to load import scene to view full info of selected browse obj
	
	//browse scene model names are added to a list for later reimport
	var activeModels : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model"); //var browseResults = new List.<String>();
	Debug.Log("No. active models: " + activeModels.Length);
	
	var sceneModelNames = new List.<String>();
	//infoCont.control.browseActiveModels = activeModels;
	
	for (var go : GameObject in activeModels)
	{
		sceneModelNames.Add(go.name);
//		go.SetActive(false);
//		DontDestroyOnLoad(go);		
	}
	infoCont.control.browseActiveModels = sceneModelNames;
	
	//cur obj needs to persist between scenes and remain active
	curObj.tag = "Current Model";
	DontDestroyOnLoad(curObj);
	curObj.transform.position = Vector3(0, 0, 0);
	
	/*
	//other scene active models are kept but disabled
	var activeModels : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model");
	Debug.Log("No. active models: " + activeModels.Length);
	infoCont.control.browseActiveModels = activeModels;
	
	for (var go : GameObject in activeModels)
	{
		go.SetActive(false);
		DontDestroyOnLoad(go);		
	}*/
	
	infoCont.control.fileName = browseXml.fileName;
	
	Application.LoadLevel("MetaPipe_ImportScene");
	
	infoCont.Load();
}


function loadBrowseCurObj()
{
	//used to return to browse scene from editting curObj
	//cur obj needs to persist between scenes and remain active
	var curObj = GameObject.FindGameObjectWithTag("Current Model");
	Destroy(curObj);
	//curObj.tag = "Active Model";
	
	Application.LoadLevel("MetaPipe_BrowseScene");
	
	
//	var sceneModels = new List.<String>();
//	sceneModels = infoCont.control.browseActiveModels;
//	for (var modelName : String in sceneModels)
//	{
//		Debug.Log("Scene Model: " + modelName);
//	}
//	
//	var browseCam : GameObject = GameObject.Find("PlayerCapsule");
//	if (browseCam != null)
//	{
//		Debug.Log("Browse Cam here");
//		browseCam.GetComponent(BrowseCamLook).navMode = false;
//	}
	
	
	/*
	//other scene active models are kept but disabled
	var activeModels : GameObject[] = infoCont.control.browseActiveModels;
	//var activeModels : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model");
	Debug.Log("No. active models: " + activeModels.Length);
	for (var go : GameObject in activeModels)
	{
		Debug.Log("Go Name: " + go.name);
		go.SetActive(true);	
		DontDestroyOnLoad(go);
		Debug.Log("Go active? " + gameObject.activeSelf);
	}*/
	
	var importCam : GameObject = GameObject.Find("ImportMainCam");
	if (importCam.activeInHierarchy){
		importCam.SetActive(false);
	}
}

function OnLevelWasLoaded(level : int) // 0 = browse scene 
{
	if (level == 0)
	{
		var sceneModels = new List.<String>();
		sceneModels = infoCont.control.browseActiveModels;
		for (var modelName : String in sceneModels)
		{
			Debug.Log("Scene Model: " + modelName);
		}
		
		var browseCam : GameObject = GameObject.Find("BrowseMainCam");
		if (browseCam != null)
		{
			Debug.Log("Browse Cam here");
			browseCam.GetComponent(BrowseCamLook).navMode = false;
		}
	}
}