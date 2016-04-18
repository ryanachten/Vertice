#pragma strict

public var contribNameText : Text;
private var curUsrName : String;


function Awake(){

	contribNameText = gameObject.GetComponent(Text);
}

function OnGUI(){

	curUsrName = ObjInfoControl.control.contribUsr;

	if(curUsrName.Length < 1){
	
		contribNameText.text = "Upload a model";

	} else {

		contribNameText.text = curUsrName;
	}
}


