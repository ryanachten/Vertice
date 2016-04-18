#pragma strict

public var modelCreateDateText : Text;
private var modelCreateDate : String;


function Awake(){

	modelCreateDateText = gameObject.GetComponent(Text);
}

function OnGUI(){

	modelCreateDate = ObjInfoControl.control.modelCreateDate;

	if(modelCreateDate.Length < 1){
	
		modelCreateDateText.text = "Upload a model";

	} else {

		modelCreateDateText.text = modelCreateDate;
	}
}


