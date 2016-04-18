#pragma strict

//send curObj to selected collection upon button press

import System.Xml;

var collectionControl : ObjCollectionControl;
var contextInfoXml : BrowseContextInfoXml;

var listNode : XmlNode;
var curObjName : String;


function Start()
{
	var gameController = GameObject.FindGameObjectWithTag("GameController");
	collectionControl = gameController.GetComponent(ObjCollectionControl);
	
	var selectObjContextPanel = GameObject.Find("SelectObjContext Panel");
	contextInfoXml = selectObjContextPanel.GetComponent(BrowseContextInfoXml);
}


function sendObjToAdd()
{
	curObjName = contextInfoXml.curObjName;
	
	//( collectionName : XmlNode, objName : String)	
	collectionControl.addObjToCollection(listNode, curObjName);
}