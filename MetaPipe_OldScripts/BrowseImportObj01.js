#pragma strict

import System.Xml;

var infoCont : ObjInfoControl;
var xmlDoc : XmlDocument;
var xmlRoot : XmlNode;

var curObjNode : XmlNode;

var objImp : ObjImporter;
private var curObj : GameObject;
var impTex : Material;
private var objTex : MeshRenderer;

public var texImportComplete : boolean;
public var objImportComplete : boolean;

//transform declarations
public var prevOrigin : Vector3;
private var newOrigin : Vector3;
public var prevObjDepth : float;
//private var curObjDepth : float;
public var objSpaceGap : float;

//Obj Reader Declarations
var standardMaterial : Material;
var transparentMaterial : Material;

function Start()
{
	objImp = new ObjImporter();
	objImportComplete = true;
	texImportComplete = true;
	
	prevOrigin = Vector3(0,0,0);
	prevObjDepth = 0;
}


function importList(sortResults : List.<String>)
{
	xmlRoot = infoCont.control.root;
	
	//Check to see if existing search results are present
	var existSearchResults  = GameObject.FindGameObjectsWithTag("Active Model");
	if (existSearchResults.Length > 0)
	{
		for (var go : GameObject in existSearchResults)
		{
			Destroy(go);
		}
	}
	
	//iterates through list, finds node and times import seq
	var curObjName : String;
	
	prevOrigin = Vector3(0,0,0);
	prevObjDepth = 0;
	
	for (curObjName in sortResults)
	{
		Debug.Log("CurObjList Name: " + curObjName);
		if (objImportComplete && texImportComplete)
		{
			var curNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']"); 
			importObj(curNode);
		}
		if (!objImportComplete || !texImportComplete) 
		{
			yield; //operation must wait for the current download to finish before beginning new one
		}
	}
}


function importObj(curObjNode : XmlNode)
{
	objImportComplete = false;
	texImportComplete = false;
	
	var meshLocation : String = curObjNode.SelectSingleNode("./MeshLocation").InnerText;
	var objName : String = curObjNode.SelectSingleNode("@name").Value;
	
	var importModels : GameObject[] = ObjReader.use.ConvertFile(meshLocation, false, standardMaterial);

	for (var model : GameObject in importModels)
	{
		curObj = model;
		
		//Add Tag
		curObj.tag = "Active Model"; 
		curObj.name = objName;
		
		//Add RigidBody
		var rb = curObj.AddComponent(Rigidbody); //add rigibody to currentModel to use physics and raycasting
		rb.useGravity = false; //make mass 0 to stop it falling (doesn't work for some reason)
		rb.isKinematic = true; //make mass 0 to stop it falling
		
		objTex = curObj.GetComponent(MeshRenderer);		
		
		//Use BoxCol to find placement
		var curObjBoxCol = curObj.AddComponent(BoxCollider);	//add box collider to find scale etc **REVISE**
		
		moveToPosition(curObj);
	
		importTex(curObjNode);
		
		while (!texImportComplete)
		{
			yield;
		}
		objImportComplete = true;
		Debug.Log("Done Downloading Obj: " + objName);
	}
	rb.useGravity = true; //make mass 0 to stop it falling (doesn't work for some reason)
	rb.isKinematic = false;
}



function importTex(curObjNode : XmlNode)
{
	var texLocation = curObjNode.SelectSingleNode("./TexLocation").InnerText;
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	
	objTex.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT1, false);
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);
		
		yield www;
	
		www.LoadImageIntoTexture(curObj.GetComponent.<Renderer>().material.mainTexture);
		
		if (www.isDone){
			texImportComplete = true;
			Debug.Log("Done Downloading Texture");
			break; //if done downloading image break loop
		}
	}
}



function moveToPosition(curObj : GameObject)
{
	//used to move the imported object to correct position
	var curObjBoxCol = curObj.GetComponent(BoxCollider);
	var curObjDepth = curObjBoxCol.size.z;
	
	newOrigin.z = prevOrigin.z + prevObjDepth/2 + objSpaceGap + curObjDepth/2;
	
	curObj.transform.position = newOrigin;
	
	prevObjDepth = curObjDepth;
	prevOrigin = newOrigin;

}

/*

function importObj(curObjNode : XmlNode)
{
	objImportComplete = false;
	texImportComplete = false;
	
	var meshLocation : String = curObjNode.SelectSingleNode("./MeshLocation").InnerText;
	var objName : String = curObjNode.SelectSingleNode("@name").Value;
	Debug.Log("curObj Mesh Location: " + curObjNode.SelectSingleNode("./MeshLocation").Value);
	Debug.Log("curObj Name: " + curObjNode.SelectSingleNode("@name").Value);
	var impMesh = objImp.ImportFile(meshLocation);
			
	//Add Imported Mesh to Scene
	curObj = new GameObject(objName, typeof(MeshRenderer), typeof(MeshFilter));
	var objMesh = curObj.GetComponent(MeshFilter);
	objTex = curObj.GetComponent(MeshRenderer);
	objMesh.mesh = impMesh;
	
	objTex.material = impTex; 
	
	//Add RigidBody
	var rb = curObj.AddComponent(Rigidbody); //add rigibody to currentModel to use physics and raycasting
	rb.useGravity = false; //make mass 0 to stop it falling (doesn't work for some reason)
	rb.isKinematic = true; //make mass 0 to stop it falling

	//Use BoxCol to find placement
	var curObjBoxCol = curObj.AddComponent(BoxCollider);	//add box collider to find scale etc **REVISE**
	
	moveToPosition(curObj);
	
	importTex(curObjNode);
	
	while (!texImportComplete)
	{
		yield;
	}
	curObj.tag = "Active Model";
	objImportComplete = true;
	Debug.Log("Done Downloading Obj: " + objName);
}

*/