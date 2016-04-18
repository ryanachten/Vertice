#pragma strict

//used to allow users to return to their collection after doing activities in another scene

import System.Xml;

static var saveList : XmlNode;

var collectionLoadScript : CollectionObjCollectionLoad; //hosted on Xml Collections Panel in Collection scene

/*
function Start()
{
	var collectionControl = GameObject.FindGameObjectWithTag("GameController").GetComponent(ObjCollectionControl);
	var curList = collectionControl.GetComponent

}


function loadSavedCollection()
{
	collectionLoadScript.loadCollectionObjects(saveList);
}
*/