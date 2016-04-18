#pragma strict


//Stripped out version of 03 minus tutorial crap


private var camStartPos : Vector3;
public var smooth : float = 1.5f; //smoothness of camera

public var curCamOffset = Vector3(10, 0, 0);

private var model : GameObject;
private var modelPos : Transform;
private var closeColPos : Vector3;


function OnEnable () {
	

	model = GameObject.Find("CurrentModel");
	modelPos = model.transform;
	Debug.Log("Initial cam position: " + transform.position);
	Debug.Log("Model position: " + modelPos.position);
	
	var curObjCol = model.GetComponent(BoxCollider);
	Debug.Log("Box Collider size " + curObjCol.size);
	closeColPos = curObjCol.ClosestPointOnBounds(transform.position);//closest point from the cam to collider bounds
	camStartPos = closeColPos + curCamOffset;
	Debug.Log("Closest Point" + closeColPos);
}


function FixedUpdate(){

	DefaultPos();
	
}

function DefaultPos(){ //moves cam into position facing model
	
	
	var thres : int = 1; //abs distance threshold
	
	
	//Process 01

	if ( Mathf.Abs(transform.position.x - camStartPos.x) <= thres || Mathf.Abs(transform.position.x - camStartPos.x) >= thres){ //detect when the cam and model are lined up
			Debug.Log("Defult Cam Position Reached");
			Debug.Log("Cam in Pos: " + "Cam x: " + transform.position.x + "Closest Point: " + camStartPos.x);
			GetComponent(MetaPipe_camController05).enabled = false;	
		} else {
			transform.position = Vector3.Lerp(transform.position, camStartPos, smooth * Time.deltaTime); //move the cam from its current position to startpos
			transform.LookAt(modelPos);	
		}





/*
//Process 02

//if ( Mathf.Abs(transform.position.x - camStartPos.x) <= thres || Mathf.Abs(transform.position.x - camStartPos.x) >= thres){ //detect when the cam and model are lined up
	
	
	//***HERE*** add while statement using vector3.distance
	Debug.Log
	transform.position = Vector3.Lerp(transform.position, camStartPos, smooth * Time.deltaTime); //move the cam from its current position to startpos
	transform.LookAt(modelPos);
	
	
	if (Vector3.Distance(transform.position, camStartPos) > 0.05f){
		
		Debug.Log("Defult Cam Position Reached");
		Debug.Log("Cam in Pos: " + "Cam x: " + transform.position.x + "Closest Point: " + camStartPos.x);

		}
	
	else {
	

		}
	
*/	
	
}