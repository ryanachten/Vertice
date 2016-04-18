#pragma strict

public var contextTitleText : Text;
private var vidName : String;


function Awake(){

	contextTitleText = gameObject.GetComponent(Text);
}

function VidName(){

	vidName = ObjInfoControl.control.cntxtMediaName;

	if(vidName.Length < 1){
	
		contextTitleText.text = "Contextual Image";

	} else {

		contextTitleText.text = vidName;
	}
}


