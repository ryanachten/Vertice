#pragma strict

import UnityEngine.UI;
//GUI script handles UI not the importing process

public var liBlue : Color;//light blue colour for styling
public var medBlue : Color;
public var liGreen : Color;
public var medGreen : Color;
public var liRed : Color;
public var medRed : Color;


function Start () {

}

function Update () {

}



public function OpenModel(){ //opens file browser dialogue for selecting models
	
	//Open Dialogue
	var filePath = EditorUtility.OpenFilePanel("Import Wavefront .obj file", "", "obj");
	var fileString = WWW(filePath).url;
	
	
	//Clear Current Scene
	var currentObj = GameObject.FindGameObjectWithTag("Current Model");
	Destroy(currentObj); //currently destroys gameobject to clear scene - this isn't ideal
	
	//GUI link
	var objButton = GameObject.Find("ObjButtonText");
	objButton.GetComponent(Text).text = ("" + fileString);
	gameObject.Find("Main Camera").BroadcastMessage("MeshImport");

}



public function OpenTex(){ //opens file browser dialogue for selecting model texture

	//Open Dialogue
	var filePath = EditorUtility.OpenFilePanel("Import Model Texture", "", "jpg");
	var fileString = WWW(filePath).url;
	
	//GUI link
	var texButton = GameObject.Find("TexButtonText");
	texButton.GetComponent(Text).text = ("" + fileString);
	gameObject.Find("Main Camera").BroadcastMessage("TexImport");
}


public function ObjReport(){ //reports vert info - called on import
	
	var curObj : GameObject = GameObject.FindGameObjectWithTag("Current Model");
	var placeHolder = GameObject.Find("ObjectNameFieldPlaceholder").GetComponent(Text).text;
	
	
	if (curObj != null){	
	
		//Report Obj Name for Placeholder
		var fileName : String = curObj.name;
		GameObject.Find("ObjectNameFieldPlaceholder").GetComponent(Text).text = fileName; //change UI file name field at import
		
		//Vert Report
		var meshVert : Vector3 [] = curObj.GetComponent(MeshFilter).mesh.vertices;
		var vertCount : int = meshVert.length;
		GameObject.Find("VertCountTextField").GetComponentInChildren(Text).text = "" + vertCount;
		
		//Tri Report
		var meshTri : int[] = curObj.GetComponent(MeshFilter).mesh.triangles;
		var triCount : int = meshTri.length /3; //by itself retusn indices in tri /3 to get tri count
		
		GameObject.Find("TriCountTextField").GetComponentInChildren(Text).text = "" + triCount;
		
		//Tri Slider Report
		var triSlider = GameObject.Find("TriCountSlider");
		
		triSlider.GetComponent(Slider).value = triCount;
			
		if (triCount < 10000){ //Light Weight Model
			Debug.Log("Model Tri: Real time ready (10,000 -> 30,000)");
			GameObject.Find("TriSliderBG").GetComponent(Image).color = liBlue;
			GameObject.Find("TriSliderFill").GetComponent(Image).color = medBlue;
		}
		if (triCount > 10000 && triCount < 30000){ //Real time ready bracket
			Debug.Log("Model Tri: Real time ready (10,000 -> 30,000)");
			GameObject.Find("TriSliderBG").GetComponent(Image).color = Color(0.7, 0.9, 0.8);
			GameObject.Find("TriSliderFill").GetComponent(Image).color = Color(0.3, 0.8, 0.6);
		}
	} else { 
		Debug.Log("Upload a model!");
	}
}


public function ObjNameEd(){ //function for editing the name of gameobject via GUI

	var curObj : GameObject = GameObject.FindGameObjectWithTag("Current Model");
	var userField = GameObject.Find("ObjectNameFieldText").GetComponent(Text);
	
	if(curObj != null){
		
		if(userField.text.Length > 5 ){ //Correct Input
			
			var newObjName : String = userField.text;
			curObj.name = newObjName;
		
		} else if (userField.text.Length < 5){ //Incorrect Input: Error **HERE**
			var errorRep : String = "Must be 5+ chars"; //dunno if this is necessary anymore but use for test
			
			Debug.Log("Must be 5+ chars");
		}
	} else {
		Debug.Log("No model uploaded");
	
	}
}



