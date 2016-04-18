#pragma strict




//MeshImport
var emptyObj: GameObject; //empty placeholder for mesh
var instPos: Vector3; //imported obj inst location
var meshObj: GameObject; //gameObj mesh to be assigned to
var impMesh : Mesh; //imported mesh to be assigned
var impTex : Material; //imported material (missing from import script atm)
var objMesh: Mesh; //mesh of meshObj
var objTex : Material; //assigns texture to model
var meshFilter : MeshFilter; //holds meshFilter



function Start() {



}

function Update() {

	var currentObj = GameObject.Find("PlaceHolderObj");
	if (Input.GetKey(KeyCode.Space)) {
		Destroy(currentObj);
		MeshImport(meshObj);
	}
}


function MeshImport(meshObj : GameObject) {
	var objImp = new ObjImporter(); //creates imp class instance
	var meshLocation = GameObject.Find("ObjButtonText").GetComponent(Text).text; //location of scan from UI nav
	var impMesh = objImp.ImportFile("" + meshLocation); //assigns meshImp w/ mesh


	meshObj = new GameObject("PlaceHolderObj", typeof(MeshRenderer), typeof(MeshFilter)); //create empty gameobject
	var meshFilter = meshObj.GetComponent(MeshFilter); //(new) mesh to var 
	meshFilter.mesh = impMesh; // (new) assigns imported mesh to mesh of gameobject
	var objTex = meshObj.GetComponent(MeshRenderer); //(new) tex to var
	objTex.material = impTex; //(new) impTex set in inspector at the moment

	Debug.Log("verts: " + meshFilter.mesh.vertexCount);
	return meshObj;
}


function MeshExport(){
	var meshExp = GameObject.Find("PlaceHolderObj"); //finds object created previously
	var expMesh = meshExp.GetComponent(MeshFilter); //gets meshFilter obj
	ObjExporter.MeshToFile(expMesh,"testObj.obj"); // export obj
	ObjExporter.MeshToString(expMesh); //export as txt
}



