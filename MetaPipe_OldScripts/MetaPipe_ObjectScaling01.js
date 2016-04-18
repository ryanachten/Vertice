#pragma strict

//added to the Object Size panel to activate when the user wants to access the scaling

var curObj : GameObject;

function Start () {
	
	curObj = GameObject.FindGameObjectWithTag("Current Model");	
	
	if(curObj == null){
		
		GetSize();
	}
}

function Update () {

}


function MouseDistance() : boolean {
	
	if(GameObject.Find("ObjResizeToggle").GetComponent(Toggle).isOn){
		
		Debug.Log("ObjScale is on");
		return true;
		}
		
		else{
		return false;
		
		
		}
		
	//***Continue Here for Mouse Distance work
	//	if(Input.GetMouseButtonDown(1)){ //right mouse button activate 
	//		var hit : RaycastHit;
	//		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	//}
}



function GetSize(){ //input fields currently turned off - acting as displays only
	
	if(curObj == null){
		curObj = GameObject.FindGameObjectWithTag("Current Model");	
	}
	
	var objSize = curObj.GetComponent(MeshFilter).mesh.bounds.size;
	Debug.Log("Mesh Scale: " + objSize);


	var widthField : Text = GameObject.Find("ObjectWidthField").GetComponentInChildren(Text);
	var objWidth : String = objSize.x.ToString("#.00") + "m"; //argument designates two decimal places
	widthField.text = objWidth;
	
	var heightField  : Text = GameObject.Find("ObjectHeightField").GetComponentInChildren(Text);
	var objHeight : String = objSize.y.ToString("#.00") + "m";
	heightField.text = objHeight;
	
	var depthField = GameObject.Find("ObjectDepthField").GetComponentInChildren(Text);
	var objDepth : String = objSize.z.ToString("#.00") + "m";
	depthField.text = objDepth;	
}

