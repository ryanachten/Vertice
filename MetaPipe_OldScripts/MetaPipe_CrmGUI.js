#pragma strict

import UnityEngine.UI;


//Still need to merge GUI script from the GUIScript_03.js file

function Start() {}


function update() {}

public function OpenDialogue() { //open dialogue window for opening .obj file
	var filePath = EditorUtility.OpenFilePanel("Import Wavefront .obj file", "", "obj");
	if (filePath.Length != 0) {
		var currentObj = GameObject.Find("TestObject"); //reference to current obj in scene
		Destroy(currentObj);
		var fileString = WWW(filePath).url;
		var objButton = GameObject.Find("ObjButtonText"); //text component of OpenObjButton
		objButton.GetComponent(Text).text; //access text return of button
		gameObject.Find("Main Camera").BroadcastMessage("ObjFileImport");
	}

}