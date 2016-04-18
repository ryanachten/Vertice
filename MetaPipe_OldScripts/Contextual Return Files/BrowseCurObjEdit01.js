#pragma strict

//Derived from LoadCurObjEdit

import System.Collections.Generic;

var infoCont : ObjInfoControl;
var rayDetectScript : BrowseRayDetectObj;
var browseXml : BrowseContextInfoXml;


function editViewInfo()
{
	var curObj : GameObject = rayDetectScript.curObj;
	loadImportCurObj(curObj);
}

//disables objs
function loadImportCurObj(curObj : GameObject){

	//used to load import scene to view full info of selected browse obj
	
	//cur obj needs to persist between scenes and remain active
	curObj.tag = "Current Model";
	DontDestroyOnLoad(curObj);
	
	if (infoCont == null)
	{
		infoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();	
	}
	
	infoCont.control.fileName = browseXml.fileName;
	infoCont.control.isBrowseEdit = true;
	
	curObj.GetComponent(Rigidbody).isKinematic = true;
	curObj.transform.position = Vector3(0,0,0);
	
	
	/*var curObjPrevPos : Vector3 = curObj.transform.position;
	infoCont.control.curObjPrevPos = curObjPrevPos; //saves previous pos for reload later
	
	var rb = curObj.GetComponent(Rigidbody);
	rb.isKinematic = true;
	
	//browse scene model names are added to a list for later reimport
	var activeModels : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model"); //var browseResults = new List.<String>();
	Debug.Log("No. active models: " + activeModels.Length);
	
	var sceneModels = new List.<GameObject>();
	
	for (var go : GameObject in activeModels)
	{
		sceneModels.Add(go);
		go.SetActive(false);
		DontDestroyOnLoad(go);	
	}
	
	infoCont.control.browseActiveModels = sceneModels;*/
	
	
	
	
	Application.LoadLevel("MetaPipe_ImportScene");
	infoCont.Load();
	
	/*
	var navPanel = GameObject.FindGameObjectWithTag("Navigation Panel");
	navPanel.GetComponent(SceneNavGUI).returnToBrowse = true;
	
	Debug.Log("Return to browse = true");*/
}