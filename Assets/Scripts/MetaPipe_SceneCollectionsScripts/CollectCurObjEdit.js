#pragma strict

import System.Xml;

//used to edit cur obj in Collection scene


var infoCont : ObjInfoControl;
var collectionCont : ObjCollectionControl;

var rayDetectScript : CollectRayDetectObj;



function editViewInfo()
{
	var curObj : GameObject = rayDetectScript.curObj;
	loadImportCurObj(curObj);
}

//disables objs
function loadImportCurObj(curObj : GameObject){
	
	//cur obj needs to persist between scenes and remain active
	curObj.tag = "Current Model";
	DontDestroyOnLoad(curObj);
	
	if (infoCont == null)
	{
		infoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();	
	}
	
	infoCont.control.prevCollection = collectionCont.curCollectionListNode;	 //is this still needed?   
	
	var curObjName = curObj.name;
	var curObjNode = infoCont.root.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
	var curObjFileName = curObjNode.SelectSingleNode("./FileName").InnerText;
	
	infoCont.control.fileName = curObjFileName;
	
	curObj.GetComponent(Rigidbody).isKinematic = true;
	curObj.transform.position = Vector3(0,0,0);
	
	Application.LoadLevel("MetaPipe_ImportScene");
	
	infoCont.Load();
}