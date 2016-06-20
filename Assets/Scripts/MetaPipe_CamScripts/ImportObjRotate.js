#pragma strict


var curObj : GameObject;

var lookSensitivity : float = 5; //used to create right feeling btwn mouse and obj
@HideInInspector
var yRotation : float; //target rotation where obj is to turn to based on mouse position
@HideInInspector
var xRotation : float;
@HideInInspector
var currentYrotation : float; //actual rotation used to catch up to above
@HideInInspector
var currentXrotation : float;
@HideInInspector
var yRotationV : float; //speed to catch up
@HideInInspector
var xRotationV : float;
var lookSmoothDamp : float = 0.1; //amount of time taken to catch up - effects accuracy


function Update()
{
	if (Input.GetMouseButton(1))
	{
		ObjRotate();	
	}
}

function ObjRotate()
{
	curObj = GameObject.FindGameObjectWithTag("Current Model");
//	yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
//	xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;

	yRotation += -Input.GetAxis("Mouse X") * lookSensitivity;
	xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;

			
	//xRotation = Mathf.Clamp(xRotation, -90, 90); //used to prevent backflips but necessary for my needs
	currentXrotation = Mathf.SmoothDamp(currentXrotation, xRotation, xRotationV, lookSmoothDamp); // moves from curPos to intendedPos
	currentYrotation = Mathf.SmoothDamp(currentYrotation, yRotation, yRotationV, lookSmoothDamp); 
	
	if (curObj != null){
		curObj.transform.rotation = Quaternion.Euler(currentXrotation, currentYrotation, 0); //0 is for the Z rotation (currently not catered for but may be of use)
	}
}