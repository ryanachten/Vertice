#pragma strict


//var modifyObjMode : boolean;

//var loadPlane : GameObject;
var modObj : GameObject;

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

var guiMode : boolean; //toggled by navMode to prevent transformations whilst GUI input

function Start()
{
	modObj = null;
	guiMode = false;
}


function Update()
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
				modObj.GetComponent(Rigidbody).isKinematic = true;
//				
				initPos = modObj.transform.position;
				var initRotation = modObj.transform.rotation;	
			}
	
			moveObject(modObj, initPos, initRotation);
		}
		else{
			modObj.GetComponent(BoxCollider).enabled = true;
			modObj.GetComponent(Rigidbody).isKinematic = false;
		}
		
		
		
		//Rotate
		if ( Input.GetKey(KeyCode.R)) 
		{
	
			if (Input.GetKeyDown(KeyCode.R))
			{
				//Debug.Log("Rotating Object");
				setupPos = false;
				curPos = modObj.transform.position;
				initRot = modObj.transform.rotation;
			} else{
				setupPos = true;			
			}
			  
			modObj.GetComponent(Rigidbody).isKinematic = true;
			
			rotateObject(curPos, initRot, setupPos);		
		} else 
		{
			modObj.GetComponent(Rigidbody).isKinematic = false;
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			var boxCol = modObj.GetComponent(BoxCollider);
			Destroy(boxCol); //destroys current box coll to make one for the new obj size
			modObj.AddComponent(BoxCollider);
		
		}
		
		
		
		//Scale
		if ( Input.GetKey(KeyCode.T))
		{		
			
			if (Input.GetKeyDown(KeyCode.T))
			{
				//Debug.Log("Scaling Object");
				setupPos = false;
				curPos = modObj.transform.position;
				initScale = modObj.transform.localScale;
				initDimen = modObj.GetComponent.<Renderer>().bounds.size;
			} else{
				setupPos = true;			
			}
			modObj.GetComponent(Rigidbody).isKinematic = true;
			
			scaleObject(curPos, setupPos, initScale, initDimen);	
		}
		else 
		{
			modObj.GetComponent(Rigidbody).isKinematic = false;
		}
		if (Input.GetKeyUp(KeyCode.T))
		{
			boxCol = modObj.GetComponent(BoxCollider);
			Destroy(boxCol); //destroys current box coll to make one for the new obj size
			modObj.AddComponent(BoxCollider);
		
		}
		
	}
}

function moveObject(modObj : GameObject, initPos : Vector3, initRotation : Quaternion)
{
	//Debug.Log("Moving Object");
	
	
	//**Raycast stuff for search scene objects
	var hit : RaycastHit;
	var foundHit : boolean = false;
	
	foundHit = Physics.Raycast(transform.position, transform.forward, hit);
	//Debug.DrawRay(transform.position, transform.forward, Color.blue);
	
	if(foundHit && hit.transform.tag != "Player")
	{
		//Debug.Log("Move to Hit Point: " + hit.point);
		modifyObjGUIscript.activateMoveDisplay(initPos, hit.point);		
		
		
		var meshHalfHeight = modObj.GetComponent.<MeshRenderer>().bounds.size.y /2; //helps account for large and small objects
//		Debug.Log("CurObj Mesh Min: " + meshHalfHeight);
		
		modObj.transform.position = hit.point;
		modObj.transform.position.y =  modObj.transform.position.y + meshHalfHeight + hoverHeight;
		
		modObj.transform.rotation = initRotation;
		//modObj.transform.rotation = Quaternion.LookRotation(hit.normal);
	}
	
}


function rotateObject(originalPos : Vector3, initRot : Quaternion, setupPos : boolean)
{
	var curObj = modObj;
	
	if (!setupPos)
	{
		var boxColsize = curObj.GetComponent(BoxCollider).bounds.size;
		var rotatePos : Vector3 = originalPos;
		rotatePos.y = rotatePos.y + boxColsize.y/2;
		curObj.transform.position = rotatePos;
		setupPos = true;
	}
		
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
	
	if (!setupPos)
	{
		var boxColsize = curObj.GetComponent(BoxCollider).bounds.size;
		var scalePos : Vector3 = originalPos;
		scalePos.y = scalePos.y + boxColsize.y/2;
		curObj.transform.position = scalePos;
		setupPos = true;
	}
		
	xScale -= Input.GetAxis("Mouse Y") * modSensitivity;
	
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