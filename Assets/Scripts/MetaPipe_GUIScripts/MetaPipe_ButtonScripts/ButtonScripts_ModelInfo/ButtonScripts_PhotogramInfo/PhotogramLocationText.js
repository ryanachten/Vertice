#pragma strict

public var photogramLocationNameText : Text;
private var photogramLocationName : String;


function Awake(){

	photogramLocationNameText = gameObject.GetComponent(Text);
}

function OnGUI(){

	photogramLocationName = ObjInfoControl.control.photogramLocationName;

	if(photogramLocationName.Length < 1){
	
		photogramLocationNameText.text = "Upload a model";

	} else {

		photogramLocationNameText.text = photogramLocationName;
	}
}


