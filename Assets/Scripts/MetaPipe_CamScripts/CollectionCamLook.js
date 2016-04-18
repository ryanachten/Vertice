#pragma strict

var lookSensitivity : float = 5;
private var yRotation : float;
private var xRotation : float;
@HideInInspector
var currentYrotation : float;
private var currentXrotation : float;
private var yRotationV : float; //velocity
private var xRotationV : float;
var lookSmoothDamp : float = 0.1;

//***Zoom vars***
var cam : Camera;
@HideInInspector
var defaultCamAngle : float = 60; //60 is Unity's default setting
var curTargetCamAngle : float = 5;
var ratioZoom : float = 1; //1 = zoomed out 0 = zoomed in
var ratioZoomV : float; //velocity
var ratioZoomSpeed : float = 0.1;

var collectCamMove : CollectionCamMovement;
var navMode : boolean;


function Start()
{
	cam = GetComponent(Camera);
}


function Update()
{
	navMode = collectCamMove.navMode;
	if (navMode)
	{
		mouseMove();
	}
}

function mouseMove()
{
	if (Input.GetMouseButton(1))
		{
			var mmbScrollWheel = Input.GetAxis("Mouse ScrollWheel");
			curTargetCamAngle += (mmbScrollWheel*2);
			
			ratioZoom = Mathf.SmoothDamp(ratioZoom, 0, ratioZoomV, ratioZoomSpeed);
		}
		else
		{
			ratioZoom = Mathf.SmoothDamp(ratioZoom, 1, ratioZoomV, ratioZoomSpeed);
		}
		
		cam.fieldOfView = Mathf.Lerp(curTargetCamAngle, defaultCamAngle, ratioZoom);
		
		yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
		xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
		
		xRotation = Mathf.Clamp(xRotation, -90, 90);
		
		currentXrotation = Mathf.SmoothDamp(currentXrotation, xRotation, xRotationV, lookSmoothDamp);
		currentYrotation = Mathf.SmoothDamp(currentYrotation, yRotation, yRotationV, lookSmoothDamp);
		
		
		transform.rotation = Quaternion.Euler(currentXrotation, currentYrotation, 0);
		
	}