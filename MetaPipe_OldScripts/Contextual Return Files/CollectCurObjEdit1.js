#pragma strict

//used to edit cur obj in Collection scene


var infoCont : ObjInfoControl;
var collectionCont : ObjCollectionControl;

var rayDetectScript : CollectRayDetectObj;

var browseXml : BrowseContextInfoXml; //??What's this purpose


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
	
	infoCont.control.isBrowseEdit = true; //use?? -> perhaps change to isCollectionEdit
	
	infoCont.control.prevCollection = collectionCont.curCollectionListNode;
	//Debug.Log("1 prevCollection NAME: " + infoCont.control.prevCollection.SelectSingleNode("@name").Value);
	//Debug.Log("1 curCollectionList NAME: " + collectionCont.curCollectionListNode.SelectSingleNode("@name").Value);
	    
	
	Application.LoadLevel("MetaPipe_ImportScene");
	
	//var navPanel = GameObject.FindGameObjectWithTag("Navigation Panel");
	//navPanel.GetComponent(SceneNavGUI).returnToBrowse = true; -> change to collection button

	//Debug.Log("Return to collection = true"); 
}