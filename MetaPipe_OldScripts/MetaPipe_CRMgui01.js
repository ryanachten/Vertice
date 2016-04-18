#pragma strict

import UnityEngine.UI;



function Start () {

}

function Update () {

}

public function OpenDialogue(){
	
	var filePath = EditorUtility.OpenFilePanel("Import Wavefront .obj file", "", "obj");
	
	var currentObj = GameObject.FindGameObjectWithTag("Current Model");
	Destroy(currentObj); //currently destroys gameobject to clear scene - this isn't ideal
	
	
	var fileString = WWW(filePath).url;
	var objButton = GameObject.Find("ObjButtonText");
	
	objButton.GetComponent(Text).text = ("" + fileString);
	gameObject.Find("Main Camera").BroadcastMessage("MeshImport");

}