#pragma strict

//Via Unity Tut / Stealth tut / Camera Pos 
//script to follow object and find best angle to view from in real time



public var smooth : float = 1.5f; //smoothness of camera
public var curCamOffset = Vector3(10, 0, 0); //10 units offset from target on x axis before framing
private var camStartPos : Vector3;

private var model : Transform; //this will need to be swapped out later
private var relCameraPos : Vector3; //to make it easy to change the pos of the cam, we will store this relative to the player

//to make the camera responsive to the environment, we need to be able to store other camera postions it can be in
//to make sure that these are all the same distance from the camera we use the magnitude of the vector
//rather than calculate this every frame, store this in a var
private var relCameraPosMag : float;

//once we have the position the camera needs to move to we need to lerp between the two
private var newPos : Vector3;



function OnEnable () { //wait until script is enabled
	
	
	model = GameObject.Find("CurrentModel").transform;
	camStartPos = model.position + curCamOffset;
	Debug.Log("Initial cam position: " + transform.position);
	Debug.Log("Model position: " + model.position);
	
	relCameraPos = transform.position - model.position;
	relCameraPosMag = relCameraPos.magnitude - 0.5f; //this has been reduced slightly to account for the axis being at feet
}


function FixedUpdate () { //to get movement as smooth as possible, both updates will need to happen at same time; hence fixed update

	
	//store all potential camera positions
	var standardPos : Vector3 = model.position + relCameraPos; //standard pos
	var abovePos  : Vector3 = model.position + Vector3.up * relCameraPosMag; //camera looking directly down on player/model
	
	//these will be points to check that the camera can see targ hence var name
	var checkPoints : Vector3[] = new Vector3[5];
	//the degree of this % is defined by the third param (0-1)
	checkPoints[0] = standardPos;
	checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
	checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
	checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.5f);
	checkPoints[4] = abovePos;
	
	
	
	for(var i = 0; i < checkPoints.Length; i++ )
	{
		//checks to see if model can see the camera
		if(ViewingPosCheck(checkPoints[i])){
			break; //if bool true break loop
		}
	
	}
	
	//***HERE***	
	//var camStartPos : Vector3 = transform.position + curCamOffset;
	Debug.Log("Old Pos: " + transform.position);
	transform.position = Vector3.Lerp(transform.position, camStartPos, smooth * Time.deltaTime); //move the cam from its current position to startpos
	Debug.Log("New Pos: " + transform.position);
	
	
	//once correct pos found, move to newPos
	transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
	//lookAt playd in the update would appear jerky - too precise
	//create smoothLookAt function instead
	SmoothLookAt();
	Debug.Log("Final cam position: " + transform.position);
	Debug.Log("Model position: " + model.position);
	
	if (SmoothLookAt){
		GetComponent(MetaPipe_camController03).enabled = false;
	}
}


function ViewingPosCheck(checkPos : Vector3) : boolean{
	//use a raycaster to check if the model has been hit or something else
	var hit : RaycastHit;
	//the raycast must go from the position currently being checked and the camera
	//and the length must be from the distance to the camera
	if(Physics.Raycast(checkPos, model.position - checkPos, hit, relCameraPosMag)){
		//if there is something there check to see if it not currentModel
		//if it is not, find another pos for cam
		if(hit.transform != model){
			return false;
		}
	}
	newPos = checkPos;
	return true;



}

function SmoothLookAt() : boolean{
	//use the vector between target and cam to create quaternion for rotation
	//then lerp between this current rotation and new rotation
	
	//find vector from camera to player
	var relModelPosition = model.position - transform.position;
	var lookAtRotation = Quaternion.LookRotation(relModelPosition, Vector3.up);
	transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
	if (Mathf.Approximately(transform.rotation.x, lookAtRotation.x)){
		if (Mathf.Approximately(transform.rotation.y, lookAtRotation.y)){
			if (Mathf.Approximately(transform.rotation.z, lookAtRotation.z)){
				if (Mathf.Approximately(transform.rotation.w, lookAtRotation.w)){
						return true;
				}
			}
		}			
	}
	else{
		return false;
	}
}





