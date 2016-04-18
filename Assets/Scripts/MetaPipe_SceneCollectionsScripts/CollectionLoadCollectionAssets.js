#pragma strict

//used to load number of collections avilable from script and instantiate correlating buttons
import System.Xml;


var collectionControl : ObjCollectionControl;
private var collectionAvail : XmlNodeList; //number of collections available in xml
var root : XmlNode;

var collectionNameButton : GameObject;
var instantParent : Transform;




function Start()
{
	getCollectionList();
}

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
	
	root = collectionControl.root;
	
	collectionAvail = collectionControl.collectionList;
	
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
	
	var listScript = instButton.GetComponent(CollectionLoadButtons);
	listScript.listNode = curListNode;
}
