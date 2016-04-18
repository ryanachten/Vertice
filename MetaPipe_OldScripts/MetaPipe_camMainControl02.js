#pragma strict

//Derived from Design Games for Architecture's Slingshot GameCont script
//straight


var screenOrigin : Vector2;
var mousePosition : Vector2;
var mousePrevious : Vector2;
var mouseDelta : Vector2;

var sightRect : Rect;

var cameraDefaultPosition : Vector3;
var cameraRotation : Quaternion;
var cameraIsRotating : boolean;
var cameraSpeed : float;
var cameraRadius : float;


function Start () {
	screenOrigin = Vector2( Screen.width / 2, Screen.height / 2 ); //find screen origin
	sightRect = Rect( (screenOrigin.x - 32), (screenOrigin.y - 32), 64, 64); //rect mouse has to be in for UI
	mousePosition = Vector2(0, 0); //mouse pos at zeroed out
	mousePrevious = Vector2(0, 0); 
	mouseDelta = Vector2(0, 0);
	cameraRotation = Quaternion.identity;


}

function Update () {

	RecordMouse();
	RotateCamera();

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
	//if ( Input.GetMouseButtonDown(0) && !sightRect.Contains(Input.mousePosition) ) //dunno if the sight rect is
																					//necessary
	{
		cameraIsRotating = true;
		Debug.Log("Rotate Cam / Camera is Rotating");
	}
	if ( cameraIsRotating )
	{ 
		if ( Input.GetMouseButton(0) )
		{
			cameraRotation.x -= mouseDelta.y;
			cameraRotation.y += mouseDelta.x;
			var new_position = cameraRotation * cameraDefaultPosition;
			transform.position = cameraRotation * cameraDefaultPosition;
			transform.LookAt( Vector3.zero );
			Debug.Log("Rotate Cam / GetMouseButton 0");
		}
		if ( Input.GetMouseButtonUp(0) )
		{
			cameraIsRotating = false;
			cameraRotation = Quaternion.identity;
			Debug.Log("Rotate Cam / GetMouseButtonUp 0");
		}
	} else {
		if ( transform.position != cameraDefaultPosition )
		{
			transform.position = Vector3.MoveTowards( transform.position, cameraDefaultPosition, cameraSpeed );
			transform.LookAt( Vector3.zero );
			Debug.Log("Rotate Cam / ifelse/ Trans.Pos != camDefPos");
		}
	}
}