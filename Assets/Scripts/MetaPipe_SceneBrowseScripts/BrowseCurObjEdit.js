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
	
	curObj.GetComponent(Rigidbody).isKinematic = true;
	curObj.transform.position = Vector3(0,0,0);

	var particleSystem = curObj.transform.GetChild(0);
	Destroy(particleSystem.gameObject);
	
	Application.LoadLevel("MetaPipe_ImportScene");

	infoCont.Start(); //**new**	
	infoCont.Load();

}