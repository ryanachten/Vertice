#pragma strict

//used to load number of collections avilable from script and instantiate correlating buttons
import System.Xml;


var collectionControl : ObjCollectionControl;
private var collectionAvail : XmlNodeList; //number of collections available in xml
//var root : XmlNode;

var collectionNameButton : GameObject;
var instantParent : Transform;


function getCollectionList() {
	
	var instParentChildren = instantParent.childCount;
	//Debug.Log("instParentChildren: " + instParentChildren);
	
	if (instParentChildren > 0)
	{
		for ( var j = 0; j < instParentChildren; j++)
		{
			var curChild = instantParent.GetChild(j).gameObject;
			Destroy(curChild);
		}
	}
	

	//root = collectionControl.root;
	if (collectionControl == null)
	{
		Debug.Log("collectionControl == null - finding");
		collectionControl = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjCollectionControl>();
	}
	
	
	collectionAvail = collectionControl.collectionList;
	
	if (collectionAvail == null) // used for when returning back to collection scene and collections are lost
	{
		Debug.Log("Collection Control collectionAvail = null: retrieving collections");
		collectionControl.Start();
		collectionAvail = collectionControl.collectionList;
	}
	else 
	{
		Debug.Log("Collection Count: " + collectionAvail.Count);
	}
	
	for (var i = 0; i < collectionAvail.Count; i++)
	{	
		var curListNode : XmlNode = collectionAvail[i];
		instantCollectionButtons(curListNode);
	}
	
}

function instantCollectionButtons( curListNode : XmlNode )
{
	var listName = curListNode.SelectSingleNode("@name").Value;

	var instButton = Instantiate(collectionNameButton, instantParent.position, instantParent.rotation);
	instButton.transform.SetParent(instantParent, false);
	
	var buttonText = instButton.GetComponentInChildren.<Text>();
	
	buttonText.text = listName;
	
	//assign list node to their script for later use
	var addCurObjScript : BrowseAddCurObjCollection = instButton.GetComponent(BrowseAddCurObjCollection);
	addCurObjScript.listNode = curListNode;	
}
