#pragma strict

//Derived from LoadCurObjEdit

import System.Collections.Generic;

var infoCont : ObjInfoControl;
var browseCamMove : BrowseCamMovement;
var browsePanel : GameObject;



function loadBrowseCurObj()
{
	//used to return to browse scene from editting curObj
	//cur obj needs to persist between scenes and remain active

	var sceneModels = new List.<GameObject>();
	sceneModels = infoCont.control.browseActiveModels;
	for (var model : GameObject in sceneModels)
	{
		model.SetActive(true);
		Debug.Log("Scene Model: " + model.name);
	}
}


/* //reimport method

function loadBrowseCurObj()
{
	//used to return to browse scene from editting curObj
	//cur obj needs to persist between scenes and remain active

	var sceneModels = new List.<String>();
	sceneModels = infoCont.control.browseActiveModels;
	for (var modelName : String in sceneModels)
	{
		Debug.Log("Scene Model: " + modelName);
	}
	
	var navMode = browseCamMove.navMode;
	navMode = false;
	Debug.Log(navMode);
	
	browsePanel.SetActive(true);
	var browseImpObj = browsePanel.GetComponent(BrowseImportObj);
	
	browseImpObj.objImp = new ObjImporter();
	browseImpObj.objImportComplete = true;
	browseImpObj.texImportComplete = true;
	browseImpObj.prevOrigin = Vector3(0,0,0);
	browseImpObj.prevObjDepth = 0;
	browseImpObj.importList(sceneModels);
		
	

}
*/