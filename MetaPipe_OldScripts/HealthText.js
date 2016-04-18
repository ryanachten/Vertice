#pragma strict


public var healthTxt : Text;


function Awake(){

		
	healthTxt = gameObject.GetComponent(Text);

}

function OnGUI(){

	var healthStr = ObjInfoControl.control.health.ToString();
	healthTxt.text = "Health: " + healthStr;
	
}