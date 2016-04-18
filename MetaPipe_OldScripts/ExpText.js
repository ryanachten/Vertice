#pragma strict


public var expText : Text;


function Awake(){

		
	expText = gameObject.GetComponent(Text);

}

function OnGUI(){

	var expStr = ObjInfoControl.control.experience.ToString();
	expText.text = "Experience: " + expStr;
	
}