#pragma strict

public var designCreationFieldText : Text;
private var designCreationField : String;


function Awake(){

	designCreationFieldText = gameObject.GetComponent(Text);
}

function OnGUI(){

	designCreationField = ObjInfoControl.control.designCreateField;

	if(designCreationField.Length < 1){
	
		designCreationFieldText.text = "Upload a model";

	} else {

		designCreationFieldText.text = designCreationField;
	}
}


