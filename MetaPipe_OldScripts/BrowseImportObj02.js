#pragma strict

import System.Xml;

var infoCont : ObjInfoControl;
var xmlDoc : XmlDocument;
var xmlRoot : XmlNode;

var curObjNode : XmlNode;

//import declarations
//var objImp : ObjImporter;
private var curObj : GameObject;
var impTex : Material;
private var objTex : MeshRenderer;
@HideInInspector
public var texImportComplete : boolean;
@HideInInspector
public var objImportComplete : boolean;
var impLimitSlider : Slider;
var impLimitRangeMin : int; //used as start point for the import limit range
var impLimitRangeMax : int;
var totalResultsCount : int; // total number of results from query provided by list.count

//transform declarations
@HideInInspector
public var prevOrigin : Vector3;
private var newOrigin : Vector3;
@HideInInspector
public var prevObjDepth : float;
//private var curObjDepth : float;
public var objSpaceGap : float;

//Obj Reader Declarations
@HideInInspector
var standardMaterial : Material;
@HideInInspector
var transparentMaterial : Material;

var curSearch :  List.<String>;
var sameSearch : boolean;
var displayResultsPanel : GameObject;

var sortPositionScript : BrowseImpSortPositionObj;

function Start()
{
	sameSearch = false;
	displayResultsPanel.SetActive(false);
	
	objImportComplete = true;
	texImportComplete = true;
	
	prevOrigin = Vector3(0,0,0);
	prevObjDepth = 0;
}


function ShowMoreObj()
{
	sameSearch = true; //prevents min/max being overwritten

	impLimitRangeMin = impLimitRangeMax + 1;
	impLimitRangeMax = impLimitRangeMax + impLimitSlider.value + 1;
	
	if (impLimitRangeMax > totalResultsCount)
	{
		impLimitRangeMax = totalResultsCount; //ensures this won't go over the maximum avilable objects	
	}
	
	importList(curSearch);
}



function importList(sortResults : List.<String>)
{
	xmlRoot = infoCont.control.root;
	
	if (!displayResultsPanel.activeSelf) //activates diplay results panel if not already active
	{
		displayResultsPanel.SetActive(true);	
	}
	
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
	prevOrigin = Vector3(0,0,0);
	prevObjDepth = 0;
	
	
	
	//import limit slider code
		
	curSearch = sortResults;
	
	var curObjName : String;

	var impLimitSliderVal = impLimitSlider.value;
	totalResultsCount = sortResults.Count;
	
	if (!sameSearch)
	{
		impLimitRangeMin = 0;
		impLimitRangeMax = impLimitSliderVal;
		if (impLimitRangeMax > totalResultsCount)
		{
			impLimitRangeMax = totalResultsCount; // prevents going beyond index range		
		}
	}
	

	//for (curObjName in sortResults)
	for ( var i = impLimitRangeMin; i <= impLimitRangeMax -1; i++) //-1 added to account for list index beginning at 0
	{
		Debug.Log("Cycle number: " + i);
		curObjName = sortResults[i];
		
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
	
	if (sameSearch) //resets same search setting
	{
		sameSearch = false;
	}
	
	sortPositionScript.sortMode(sortResults); //sends objects to be positioned
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