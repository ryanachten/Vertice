#pragma strict

//Derived from Design Games for Architecture's Slingshot GameCont script
//edited


private var screenOrigin : Vector2;
private var mousePosition : Vector2;
private var mousePrevious : Vector2;
private var mouseDelta : Vector2;

private var cameraDefaultPosition : Vector3;
private var cameraRotation : Quaternion;
private var cameraIsRotating : boolean;

//private var targPos : Transform;

public var cameraSpeed : float;
public var cameraRadius : float;
public var camMoveSpeed : float = 2.0;


function Start () {
	screenOrigin = Vector2( Screen.width / 2, Screen.height / 2 ); //find screen origin
	mousePosition = Vector2(0, 0); //mouse pos at zeroed out
	mousePrevious = Vector2(0, 0); 
	mouseDelta = Vector2(0, 0);
	cameraRotation = Quaternion.identity;
}



function Update () {

	RecordMouse();
	MoveCamera();
	RotateCamera();
}



function MoveCamera(){
	//Horizontal and vertical axis range -1 -> 1
	var moveZ : float = Input.GetAxis("Vertical") * camMoveSpeed;
	var moveX : float = Input.GetAxis("Horizontal") * camMoveSpeed;

	transform.Translate(moveX, 0, moveZ);
}


function RecordMouse ()
{
	mousePrevious.x = mousePosition.x;
	mousePrevious.y = mousePosition.y;
	mousePosition.x = Input.mousePosition.x - screenOrigin.x;
	mousePosition.y = Input.mousePosition.y - screenOrigin.y;
	mouseDelta.x = (mousePosition.x - mousePrevious.x) / (Screen.width / 2);
	mouseDelta.y = (mousePosition.y - mousePrevious.y) / (Screen.height / 2);
}

function RotateCamera () 
{
	if (Input.GetMouseButton(0)){	//if left mouse button is down apply rotation
		transform.rotation.x -= mouseDelta.y;
		transform.rotation.y += mouseDelta.x;
		
		
		var new_position = cameraRotation * cameraDefaultPosition;
	}																	

}