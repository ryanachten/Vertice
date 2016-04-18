#pragma strict

//Script to enable and instantiate Unity's standard package FPS controller
//at the last location of the Main Camera from its default transition


/*
public var fpsControl : GameObject; //FPScontroller declarations
private var mainCam : GameObject;
private var curCamPos : Vector3;
private var fpsPos : Transform;
private var targetPos : Transform;
*/


//elements derived from MVCodeClub's create fp controller unity tut


//Mouse Look code
var lookSpeed : float = 3.0; //default 5.0
var verticalRotation : float = 0.0;
var horiztonalRotation : float = 0.0;
var verticalRange : float = 60.0;
var horizontalRange : float = 360.0;

//Player (cam in this case) control code


function OnEnable () {

	Cursor.visible = false;
	transform.LookAt(GameObject.FindGameObjectWithTag("Current Model").transform);

}

function Update () {

	//Mouse Look code
	horiztonalRotation = Input.GetAxis("Mouse X") * lookSpeed;
	//transform.Rotate(0, horiztonalRotation, 0);
	horiztonalRotation = Mathf.Clamp(horiztonalRotation, -horizontalRange, horizontalRange);
	
	
	verticalRotation -= Input.GetAxis("Mouse Y") * lookSpeed;
	//verticalRotation = Mathf.Clamp(verticalRotation, -verticalRange, verticalRange);
	
	Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, horiztonalRotation, 0);
	


}





/*
function FPScontroller(){  //for use to activate the FP Controller in Standard Assets

	//Get Cur Cam
	mainCam = gameObject;
	curCamPos = transform.position;

	//Assign Target
	targetPos = GameObject.Find("Current Model").transform;

	//Position FPS Controller
	fpsControl = GameObject.Find("Import_FPSController");
	fpsControl.transform.position = curCamPos;
	//fpsControl.transform.LookAt(targetPos);

	Debug.Log("Current Cam Pos: " + curCamPos);
	Debug.Log("Current FPS Pos: " + fpsControl.transform.position);

	//
	//fpsControl.SetActive(true);
	//mainCam.GetComponent(Camera).enabled = false; //make sure to reactivate this when changing models
}*/