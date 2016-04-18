#pragma strict

public var contextAudText : Text;
private var audName : String;


function Awake(){

	contextAudText = gameObject.GetComponent(Text);
}

function AudName(){

	audName = ObjInfoControl.control.cntxtMediaName;

	if(audName.Length < 1){
	
		contextAudText.text = "Contextual Audio";

	} else {

		contextAudText.text = audName;
	}
}


