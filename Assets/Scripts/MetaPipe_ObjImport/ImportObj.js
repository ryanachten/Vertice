#pragma strict

import UnityEngine.UI;


public var openObj : OpenObj;
public var objReport : ObjReport; //holds objReport script 
public var objInfoCont : ObjInfoControl;
public var guiReset : Imp_GUIreset;


var standardMaterial : Material;
var transparentMaterial : Material;

var impMesh : Mesh;
var impTex : Material;

@HideInInspector
var instPos : Vector3;
var meshObj : GameObject;
var objMesh : Mesh;
var objTex : Material;



function Awake () {

	openObj = GetComponent(OpenObj);
	objReport = GetComponent(ObjReport); //access objreport script to activate function
}


function MeshImport(){	
	
	var curObj = GameObject.FindGameObjectWithTag("Current Model");
	if (curObj != null){
		curObj.tag = "Untagged";
		Destroy(curObj);
//		guiReset.resetTextFields(); //***NEW*** 
	}
	
	var meshLocation = ObjInfoControl.control.meshLocation;
	var objFileName = meshLocation;
	
	var importModels : GameObject[] = ObjReader.use.ConvertFile(objFileName, false, standardMaterial);

	for (var model : GameObject in importModels)
	{
		model.tag = "Current Model"; 
		
		//Split String for Import GameObject Name
		var directorySplit : String[];
		directorySplit = meshLocation.Split("/"[0]);
		var fileName = directorySplit[directorySplit.Length-1]; //original file name
	
		var objNameSplit = fileName.Split("."[0]); 
		var objName = objNameSplit[objNameSplit.Length-2]; //object name minus extension
		
		model.name = objName;
		
		//Initial Object Data Assignment
		ObjInfoControl.control.objName = objName; //object name minus extension
		ObjInfoControl.control.fileName = fileName; //file name with extension
		ObjInfoControl.control.meshLocation = meshLocation; //updates meshLocation each import
		
		model.AddComponent(Rigidbody); //add rigibody to currentModel to use physics and raycasting
		model.GetComponent(Rigidbody).useGravity = false; //make mass 0 to stop it falling
		
		var meshObjBoxCol = model.AddComponent(BoxCollider);	//add box collider to find scale etc **REVISE**
		
		objReport.ObjReport(); //reports obj info to GUI
		
	if (objInfoCont == null && Application.loadedLevel == 1)
	{
		Debug.Log("objInfoCont lost - finding");
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
		
		objInfoCont.fromImport = true; //used to allow auto tex import
		objInfoCont.Load(); //load xml data for object
	}
}


function TexImport(){ //assigns tex selected in GUI to current gameobject
	
	if (objInfoCont == null && Application.loadedLevel == 1)
	{
		Debug.Log("objInfoCont lost - finding");
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
		
	var curObj = GameObject.FindGameObjectWithTag("Current Model");
	var renderer : Renderer = curObj.GetComponent.<Renderer>();

	if(renderer.material.mainTexture != null){ //if tex already present delete it
		
		Debug.Log("Already a texture here! Deleted!");
		Destroy(renderer.material.mainTexture);	//***REVISE***
	}

	var texLocation = objInfoCont.control.texLocation;
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
	
	renderer.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT1, false);
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);
		
		yield www;
	
		Debug.Log("Done Downloading Texture");
		www.LoadImageIntoTexture(renderer.material.mainTexture);
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}
	objInfoCont.Save();
}