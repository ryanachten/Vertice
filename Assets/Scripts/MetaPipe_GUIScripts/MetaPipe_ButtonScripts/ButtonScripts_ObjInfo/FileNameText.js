#pragma strict

public var fileNameText : Text;
private var curFileName : String;

function Awake(){

	fileNameText = gameObject.GetComponent(Text);
}

function OnGUI(){
	
	curFileName = ObjInfoControl.control.fileName;
	
	if(curFileName.Length < 1){
	
		fileNameText.text = "Upload a model";

	} else {

		fileNameText.text = curFileName;
	}

}