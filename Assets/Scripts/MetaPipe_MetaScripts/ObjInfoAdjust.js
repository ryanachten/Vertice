#pragma strict

import System.Xml;

var objInfoCont : ObjInfoControl;
var collectCont : ObjCollectionControl;

public var objNameFieldText : Text;
public var objDescriptText : Text;
public var contribUsrText : Text;
public var modelUsrText : Text;
public var modelCreateDateText : Text;
public var photogramLocateNameText : Text;
public var designCreateFieldText : Text;

/*
function saveData(){

	ObjInfoControl.control.Save();

}

function loadData(){

	ObjInfoControl.control.Load();

}*/

function objName(){	

	if (objNameFieldText == null)
	{
		objNameFieldText = GameObject.Find("ObjectNameFieldText").GetComponent.<Text>();
	}
	
	if (objInfoCont == null)
	{
		objInfoCont = gameObject.GetComponent.<ObjInfoControl>();	
	}
	
	if (objNameFieldText.text.length > 3)
	{
		var newName = objNameFieldText.text;
		var curObj = GameObject.FindGameObjectWithTag("Current Model");
		curObj.name = newName;
		var oldName = objInfoCont.control.objName; //stored for collection find
		Debug.Log("Old Object Name: " + oldName);
		objInfoCont.control.objName = newName;
		Debug.Log("New Object Name: " + newName);
		
		var objInfoRoot = objInfoCont.root;
		var objInfoDoc = objInfoCont.doc;
		var objInfoNode = objInfoRoot.SelectSingleNode("MetaPipeObject[@name='"+ oldName +"']");
		objInfoNode.SelectSingleNode("@name").Value = newName;
		Debug.Log("objInfoNode: " + objInfoNode.SelectSingleNode("@name").Value);
		
		if (objInfoDoc == null)
			Debug.Log("objInfoDoc == null");
		objInfoDoc.Save(Application.dataPath + "//Metapipe_ObjArchive.xml");
		
		
		//change the object's name in collection XML
		if (collectCont == null)
		{
			collectCont = gameObject.GetComponent.<ObjCollectionControl>();	
		}
		var collectDoc = collectCont.collectionDoc;
		var collectRoot = collectCont.root;
		var collectObjNodes : XmlNodeList = collectRoot.SelectNodes("MetaPipeCollection/MetaPipeObject[@name='"+ oldName +"']");
		Debug.Log("mpObjNodes: " + collectObjNodes.Count);
		
		if (collectObjNodes.Count > 0)
		{
			for (var objNode : XmlNode in collectObjNodes)
			{
				objNode.SelectSingleNode("@name").Value = newName;
			}
			collectDoc.Save(Application.dataPath + "/Metapipe_UserCollections.xml");
		}
	}
}


function objDescript(){

	if (objDescriptText == null)
	{
		objDescriptText = GameObject.Find("ObjDescriptFieldText").GetComponent.<Text>();
	}
	
	if (objDescriptText.text.length > 3)
	{
		var usrText = objDescriptText;
		ObjInfoControl.control.objDescript = usrText.text;
		ObjInfoControl.control.Save(); //used for autosave functionality
		Debug.Log("New Object Description: " + usrText.text);
	}
}


function contribName(){

	if (contribUsrText == null)
	{
		contribUsrText = GameObject.Find("ContribNameFieldText").GetComponent.<Text>();
	}
	
	if (objInfoCont == null)
	{
		Debug.Log("objInfoCont == null");
		objInfoCont = gameObject.GetComponent.<ObjInfoControl>();	
	}

	if (contribUsrText.text.length > 3)
	{
		var usrText = contribUsrText;
		
		ObjInfoControl.control.contribUsr = usrText.text;
		ObjInfoControl.control.Save(); 

		Debug.Log("New Contributor Name: " + usrText.text);
	}
}


function modelCreator(){

	if (modelUsrText == null)
	{
		modelUsrText = GameObject.Find("ModelCreatorFieldText").GetComponent.<Text>();
	}
	
	if (modelUsrText.text.length > 3)
	{
		var usrText = modelUsrText;
		ObjInfoControl.control.modelCreatorName = usrText.text;
		ObjInfoControl.control.Save(); 
		Debug.Log("New Model Creator Name: " + usrText.text);
	}
}


function modelCreateDate(){

	if (modelCreateDateText == null)
	{
		modelCreateDateText = GameObject.Find("ModelCreateDateFieldText").GetComponent.<Text>();
	}
	
	if (modelCreateDateText.text.length > 3)
	{
		var usrText = modelCreateDateText;
		ObjInfoControl.control.modelCreateDate = usrText.text;
		ObjInfoControl.control.Save(); 
		Debug.Log("New Model Creation Date: " + usrText.text);
	}
}


function photogramLocateName()
{
	//Add input field catches here
	if (photogramLocateNameText == null)
	{
		photogramLocateNameText = GameObject.Find("PhotogramLocateNameFieldText").GetComponent.<Text>();
	}
	
	if (photogramLocateNameText.text.length > 3)
	{
		var usrText = photogramLocateNameText;
		ObjInfoControl.control.photogramLocationName = usrText.text;
		ObjInfoControl.control.Save();
		Debug.Log("New Photogram Location Name: " + usrText.text);
	}
}

function designCreateField()
{
	if (designCreateFieldText == null)
	{
		designCreateFieldText = GameObject.Find("DesignCreateFieldTitleInputText").GetComponent.<Text>();
	}

	if (designCreateFieldText.text.length > 3)
	{
		var usrText = designCreateFieldText;
		ObjInfoControl.control.designCreateField = usrText.text;
		ObjInfoControl.control.Save(); 
		Debug.Log("New Design Create Field: " + usrText.text);
	}
}