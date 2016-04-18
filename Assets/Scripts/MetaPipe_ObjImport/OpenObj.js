#pragma strict

//updated to use UniFileBrowser over EditorUtility.OpenFilePanel
//used to be called CRMgui03
//GUI script handles UI not the importing process


import UnityEngine.UI;

public var importObjScript : ImportObj; //holds import script
public var objInfoCont : ObjInfoControl;
public static var meshLocation : String;
public var texLocation : String;

public var curObj : GameObject;

private var pathChar = "/"[0];




function Awake(){
	
	importObjScript = gameObject.GetComponent(ImportObj);
}


public function OpenModel(){ //**REVISE** this function is pretty redundant
	
	UniFileBrowser.use.OpenFileWindow(ModelFile);
}

public function ModelFile(pathToModel : String){

	meshLocation = pathToModel; //assigned to public var - reference script to access
	objInfoCont.control.meshLocation = meshLocation;
	importObjScript.MeshImport(); //imports obj mesh
}


public function OpenTex(){ //**REVISE** this function is pretty redundant

	//Open Dialogue
	UniFileBrowser.use.OpenFileWindow(TexFile);
}

public function TexFile(pathToTex : String){
	
	texLocation = pathToTex;
	objInfoCont.control.texLocation = texLocation;
	importObjScript.TexImport();

}


