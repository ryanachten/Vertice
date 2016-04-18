#pragma strict

public var contextImgText : Text;
private var imgName : String;


function Awake(){

	contextImgText = gameObject.GetComponent(Text);
}

function ImgName(){

	imgName = ObjInfoControl.control.cntxtMediaName;

	if(imgName.Length < 1){
	
		contextImgText.text = "Contextual Image";

	} else {

		contextImgText.text = imgName;
	}
}


