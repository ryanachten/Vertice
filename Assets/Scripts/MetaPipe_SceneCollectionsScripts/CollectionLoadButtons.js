#pragma strict

//send curObj to selected collection upon button press

import System.Xml;

var collectionControl : ObjCollectionControl;
var collectionLoadScript : CollectionObjCollectionLoad;


var listNode : XmlNode;


function Start()
{

}


function sendToLoad()
{
	collectionControl = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjCollectionControl>();
	
	var xmlCollectionsPanel = GameObject.Find("Xml Collections Panel");
	collectionLoadScript = xmlCollectionsPanel.GetComponent(CollectionObjCollectionLoad);

	//Check first to see if collection has alread been loaded
	if (collectionControl.curCollectionListNode != null)
	{
		//Debug.Log("Destroying Exist Collection Objects");
		var existCollection : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model");
		for ( var go : GameObject in existCollection)
		{
			Destroy(go);			
		}
	}

	collectionLoadScript.loadCollectionObjects(listNode);
	
	collectionControl.curCollectionListNode = listNode;
	
	var collectGUIcont = GameObject.Find("Collection Information").GetComponent.<CollectionGUIcontrol>();
	collectGUIcont.getGuiInfo(); //added w/ collect save revisions
	
	Debug.Log("curCollectionListNode: " + collectionControl.curCollectionListNode.SelectSingleNode("@name").Value);

}