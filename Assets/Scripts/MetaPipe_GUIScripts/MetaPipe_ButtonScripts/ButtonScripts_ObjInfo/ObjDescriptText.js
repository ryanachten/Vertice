#pragma strict

public var objDescriptText : Text;

//public var objNamePlaceholderText : Text;
private var curObjDescript : String;




function OnGUI(){

	curObjDescript = ObjInfoControl.control.objDescript;

	if(curObjDescript.Length < 1){
	
		objDescriptText.text = "Upload a model to add a description";

	} else {

		objDescriptText.text = curObjDescript;
	}
}


