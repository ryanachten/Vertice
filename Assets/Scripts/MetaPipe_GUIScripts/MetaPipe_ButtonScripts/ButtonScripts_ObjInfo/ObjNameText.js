#pragma strict

public var objNameText : Text;
private var curObjName : String;


function Awake(){

	objNameText = gameObject.GetComponent(Text);
}

function OnGUI(){

	curObjName = ObjInfoControl.control.objName;

	if(curObjName.Length < 1){
	
		objNameText.text = "Upload a model";

	} else {

		objNameText.text = curObjName;
	}
}


