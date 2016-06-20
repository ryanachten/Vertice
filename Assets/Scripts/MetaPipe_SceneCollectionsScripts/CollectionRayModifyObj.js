#pragma strict


//var modifyObjMode : boolean;

//var loadPlane : GameObject;
var modObj : GameObject;

var speed : float = 0.1f; 
var velocity : Vector3 = Vector3.zero;
var smoothTime : float = 0.3f;

var modSensitivity : float = 5; //used to create right feeling btwn mouse and obj
var hoverHeight :float = 1f;

@HideInInspector
var yRotation : float; //target rotation where obj is to turn to based on mouse position
@HideInInspector
var xRotation : float;
@HideInInspector
var currentYrotation : float;
@HideInInspector
var currentXrotation : float;
@HideInInspector
var yRotationV : float; //speed to catch up
@HideInInspector
var xRotationV : float;
var modSmoothDamp : float = 0.1; //amount of time taken to catch up - effects accuracy

@HideInInspector
var xScale : float;
@HideInInspector
var currentXscale : float;
@HideInInspector
var xScaleV : float;
@HideInInspector
var setupPos : boolean;
@HideInInspector
var initPos : Vector3;
@HideInInspector
var curPos : Vector3;
@HideInInspector
var initRot : Quaternion;
@HideInInspector
var initScale : Vector3;
@HideInInspector
var initDimen : Vector3;

var modifySelectObjPanel : GameObject;
var modifyObjGUIscript : CollectionModifyObjInputGUI;
var selectModPanelActive : boolean;
var selectObjPanel : GameObject;
var camMoveScript : CollectionCamMovement; 

var guiMode : boolean; //toggled by navMode to prevent transformations whilst GUI input

function Start()
{
	modObj = null;
	guiMode = false;
}


function FixedUpdate()
{

	if (modObj != null && !guiMode){
	
		//Panel Control 
		if (!selectObjPanel.activeSelf && !modifySelectObjPanel.activeSelf) //if the selectpanel not open and modSelect not already activated
		{
			activateModSelectObjPanel(true); //activate it
		} 
		else if (selectObjPanel.activeSelf)
		{
			activateModSelectObjPanel(false);
		}

	
		//Move
		if ( Input.GetKey(KeyCode.E))
		{	
			if (Input.GetKeyDown(KeyCode.E))
			{
				modObj.GetComponent(BoxCollider).enabled = false;				
				initPos = modObj.transform.position;
				var initRotation = modObj.transform.rotation;	
			}
			
			//Non-Rigidbody approach:
			modObj.GetComponent(Rigidbody).isKinematic = true;
			
			//Rigidbody approach:
//			var movRb = modObj.GetComponent(Rigidbody);
//			movRb.useGravity = false;
//			movRb.freezeRotation = true; //***NEW*** 
	
			moveObject(modObj, initPos, initRotation);
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			modObj.GetComponent(BoxCollider).enabled = true;
			
			//Non-Rigidbody approach:
			modObj.GetComponent(Rigidbody).isKinematic = false;

			//Rigidbody approach:
//			movRb = modObj.GetComponent(Rigidbody);
//			movRb.useGravity = true;
//			movRb.freezeRotation = false;
		}
		
		
		//Rotate
		if ( Input.GetKey(KeyCode.R)) 
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				//Debug.Log("Rotating Object");
				setupPos = false;
				camMoveScript.navMode = false; 
				curPos = modObj.transform.position;
				initRot = modObj.transform.rotation;	
			} else{
				setupPos = true;			
			}  
			
			var rotRb = modObj.GetComponent.<Rigidbody>();
			rotRb.freezeRotation = true; //***NEW***
			rotRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
//			modObj.GetComponent(Rigidbody).isKinematic = true;

			rotateObject(curPos, initRot, setupPos);		
		} 
		if (Input.GetKeyUp(KeyCode.R))
		{
			rotRb = modObj.GetComponent.<Rigidbody>();
			rotRb.constraints = RigidbodyConstraints.None;
			rotRb.freezeRotation = false; //***NEW***
			
//			modObj.GetComponent(Rigidbody).isKinematic = false;
			camMoveScript.navMode = true; 
		}
				
						
		//Scale
		if ( Input.GetKey(KeyCode.T))
		{		
			
			if (Input.GetKeyDown(KeyCode.T))
			{
				//Debug.Log("Scaling Object");
				setupPos = false;
				camMoveScript.navMode = false;
				curPos = modObj.transform.position;
				initScale = modObj.transform.localScale;
				initDimen = modObj.GetComponent.<Renderer>().bounds.size;
			} else{
				setupPos = true;			
			}
			var sclRb = modObj.GetComponent.<Rigidbody>();
			sclRb.freezeRotation = true; //***NEW***
			sclRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
//			modObj.GetComponent(Rigidbody).isKinematic = true;
//			modObj.GetComponent(Rigidbody).useGravity = false; 
			
			scaleObject(curPos, setupPos, initScale, initDimen);	
		}
		if (Input.GetKeyUp(KeyCode.T))
		{
			sclRb = modObj.GetComponent.<Rigidbody>();
			sclRb.constraints = RigidbodyConstraints.None;
			sclRb.freezeRotation = false; //***NEW***
//			modObj.GetComponent(Rigidbody).isKinematic = false;
//			modObj.GetComponent(Rigidbody).useGravity = true; 
			camMoveScript.navMode = true;
		}
	}
}

function moveObject(modObj : GameObject, initPos : Vector3, initRotation : Quaternion)
{
	//Debug.Log("Moving Object");

	var hit : RaycastHit;
	var foundHit : boolean = false;
	
	foundHit = Physics.Raycast(transform.position, transform.forward, hit);
	//Debug.DrawRay(transform.position, transform.forward, Color.blue);
	
	if(foundHit && hit.transform.tag == "Terrain") 
	{
		modifyObjGUIscript.activateMoveDisplay(initPos, hit.point);		
		
		var meshHalfHeight = modObj.GetComponent.<MeshRenderer>().bounds.size.y /2; //helps account for large and small objects
		
		modObj.transform.position = hit.point; //***method 01***
		
		//Non-Rigidbody approach:
//		modObj.transform.position = Vector3.Lerp(initPos, hit.point, speed); //***method 02***
		
		modObj.transform.position.y =  modObj.transform.position.y + meshHalfHeight + hoverHeight;
//		modObj.transform.rotation = initRotation;
		
		//Rigidbody approach:
//		var movRb = modObj.GetComponent.<Rigidbody>(); //***NEW - method 04***
//		movRb.position = Vector3.Lerp(initPos, hit.point, speed); 
		
//		movRb.position.y =  meshHalfHeight + hoverHeight;
//		movRb.rotation = initRotation;
	}
}


function rotateObject(originalPos : Vector3, initRot : Quaternion, setupPos : boolean)
{
	var curObj = modObj;	
		
	yRotation += Input.GetAxis("Mouse X") * modSensitivity;
	xRotation -= Input.GetAxis("Mouse Y") * modSensitivity;
	
	currentXrotation = Mathf.SmoothDamp(currentXrotation, xRotation, xRotationV, modSmoothDamp); // moves from curPos to intendedPos
	currentYrotation = Mathf.SmoothDamp(currentYrotation, yRotation, yRotationV, modSmoothDamp); 
	
	if (curObj != null){
		
		//update curObj
		curObj.transform.rotation = Quaternion.Euler(currentXrotation, currentYrotation, 0); //0 is for the Z rotation (currently not catered for but may be of use)
		
		modifyObjGUIscript.activateRotateDisplay(initRot, curObj.transform.rotation); //update GUI
	}
}



function scaleObject(originalPos : Vector3, setupPos : boolean, initScale : Vector3, initDimen : Vector3)
{
	var curObj = modObj;
		
	xScale -= Input.GetAxis("Mouse Y") * modSensitivity;
	xScale = Mathf.Abs(xScale);
	
	currentXscale = Mathf.SmoothDamp(currentXscale, xScale, xScaleV, modSmoothDamp); // moves from curPos to intendedPos
	
	if (curObj != null){
		curObj.transform.localScale = Vector3(currentXscale, currentXscale, currentXscale); //0 is for the Z rotation (currently not catered for but may be of use)
		
		//update GUI
		var newSize = curObj.GetComponent.<Renderer>().bounds.size;
		modifyObjGUIscript.activateScaleDisplay(initScale, initDimen, curObj.transform.localScale, newSize);
	}	
}


function activateModSelectObjPanel(activePanel : boolean)
{
	if (activePanel)
	{
		modifySelectObjPanel.SetActive(true);	
	}
	else if(!activePanel)		
	{
		modifySelectObjPanel.SetActive(false);	
	}	

}