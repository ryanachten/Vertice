#pragma strict

import UnityEngine.UI;

var impMesh : Mesh;
var impTex : Material;
var matShader : Shader;

var emptyObj : GameObject;
var instPos : Vector3;
var meshObj : GameObject;
var objMesh : Mesh;
var objTex : Material;


function Start () {
}

function Update () {
}


function MeshImport(){	
	
	var objImp = new ObjImporter();
	var meshLocation = GameObject.Find("ObjButtonText").GetComponent(Text).text;
	var impMesh = objImp.ImportFile("" + meshLocation);
	
	//Split String for Import GameObject Name
	var directorySplit : String[];
	directorySplit = meshLocation.Split("/"[0]);
	Debug.Log("String: " + meshLocation);
	
	var fileName = directorySplit[directorySplit.Length-1];
	Debug.Log("File name: " + fileName);
	
	//Add Imported Mesh to Scene
	var meshObj = new GameObject(fileName, typeof(MeshRenderer), typeof(MeshFilter)); //now adds file name as the gameobject name
	var objMesh = meshObj.GetComponent(MeshFilter);
	var objTex = meshObj.GetComponent(MeshRenderer);
	objMesh.mesh = impMesh;
	objTex.material = impTex; //set in inspector atm
	
	meshObj.tag = "Current Model";
	
	meshObj.AddComponent(Rigidbody); //add rigibody to currentModel to use physics
	meshObj.GetComponent(Rigidbody).useGravity = false; //make mass 0 to stop it falling
	
	var meshObjBoxCol = meshObj.AddComponent(BoxCollider);	//add box collider to find scale etc
	
	GetComponent(MetaPipe_camCoroutine03).enabled = true;
	
}
