#pragma strict

public var modelCreateTypeText : Text;
private var modelCreateType : String;


function Awake(){

	modelCreateTypeText = gameObject.GetComponent(Text);
}

function OnGUI(){

	modelCreateType = ObjInfoControl.control.modelCreateType;

	if(modelCreateType.Length < 1){
	
		modelCreateTypeText.text = "Upload a model";

	} else {

		modelCreateTypeText.text = modelCreateType;
	}
}


