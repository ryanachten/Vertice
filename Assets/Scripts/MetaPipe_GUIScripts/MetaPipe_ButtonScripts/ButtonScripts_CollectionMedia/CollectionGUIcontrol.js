#pragma strict

import System.Xml;


var collectionControl : ObjCollectionControl;
var collectSave : CollectionSaveCollectionInfo;


//GUI compontents
var collectionTitle : Text;
	var collectionTitleField : Text;
var collectionCreator : Text;
	var collectionCreatorField : Text;
var collectionDate : Text;
var collectionDescript : Text;
	var collectionDescriptField : Text;


function Start()
{	
	if (collectionControl == null) //needed for browse -> collection load issues
	{
		var gc = GameObject.FindGameObjectWithTag("GameController");
		collectionControl = gc.GetComponent.<ObjCollectionControl>();	
	}
}


function Update()
{	
	if (collectionControl == null) //needed for browse -> collection load issues
	{
		Debug.Log("collectionControl Lost: Finding again");
		var gc = GameObject.FindGameObjectWithTag("GameController");
		collectionControl = gc.GetComponent.<ObjCollectionControl>();	
	}
}


function getGuiInfo() //accesses XML for GUI information about collection
{
	var curCollectionNode = collectionControl.curCollectionListNode;

	var curCollectionName : String = curCollectionNode.SelectSingleNode("@name").Value;
	collectionTitle.text = curCollectionName;
	
	try {
		var curCollectionCreator : String = curCollectionNode.SelectSingleNode("./CollectionInfo/CollectionCreator").InnerText;
	}catch(err){
		curCollectionCreator = "";
	}
	collectionCreator.text = curCollectionCreator;
		
	var curCollectionDate : String = curCollectionNode.SelectSingleNode("./CollectionInfo/CollectionDate").InnerText;
	collectionDate.text = curCollectionDate;
	
	try {
		var curCollectionDescript : String = curCollectionNode.SelectSingleNode("./CollectionInfo/CollectionDescription").InnerText;
	}catch(err){
		curCollectionDescript = "";
	}
	collectionDescript.text = curCollectionDescript;
}


function changeCollectionTitle() //via input field
{
	if (collectionTitleField.text.length > 3)
	{
		collectionTitle.text = collectionTitleField.text;
		collectSave.SaveGuiInfo();
	}
}

function changeCollectionCreator() //via input field
{
	if (collectionCreatorField.text.length > 3)
	{
		collectionCreator.text = collectionCreatorField.text;
		collectSave.SaveGuiInfo();
	}
}


function changeCollectionDescription() //via input field
{
	if (collectionDescriptField.text.length > 3)
	{
		collectionDescript.text = collectionDescriptField.text;
		collectSave.SaveGuiInfo();
	}
}