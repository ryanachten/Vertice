#pragma strict

public var contribDateText : Text;
private var contribDate : String;


function Awake(){

	contribDateText = gameObject.GetComponent(Text);
}

function OnGUI(){

	contribDate = ObjInfoControl.control.contribDate;

	if(contribDate.Length < 1){
	
		contribDateText.text = "Upload a model";

	} else {

		contribDateText.text = contribDate;
	}
}


