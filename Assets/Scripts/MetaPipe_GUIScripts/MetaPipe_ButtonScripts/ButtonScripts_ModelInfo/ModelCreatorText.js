#pragma strict

public var modelCreatorText : Text;
private var modelCreatorName : String;


function Awake(){

	modelCreatorText = gameObject.GetComponent(Text);
}

function OnGUI(){

	modelCreatorName = ObjInfoControl.control.modelCreatorName;

	if(modelCreatorName.Length < 1){
	
		modelCreatorText.text = "Upload a model";

	} else {

		modelCreatorText.text = modelCreatorName;
	}
}


