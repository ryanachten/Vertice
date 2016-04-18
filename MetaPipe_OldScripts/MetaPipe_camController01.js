#pragma strict

var camVertFovDeg : float;
var curCamFov : float;


function Start () {
}

function Update () {

	moveToObj();
}

public function moveToObj(){ //move cam close to model
	
	var currentObj = GameObject.Find("CurrentModel");
	var curObjTrans = currentObj.transform;
	var curObjPos = curObjTrans.position; //current obj position
	var curCamPos = gameObject.transform.position; //current cam pos7
	
	var targViewDist = 20; //distance from the object target
	var targPos = Vector3(curObjPos.x, curObjPos.y, (curObjPos.z - 20)); //target position for the camera
	
	if (currentObj != null){
	
		Debug.Log("Current Object Position" + curObjPos);
		Debug.Log("Current Camera Position" + curCamPos);	
		
		if (curCamPos != targPos){
			
			Camera.current.transform.position = targPos; //move camera to target position
			Debug.Log("Current Camera Position" + curCamPos);
		//Move near object
		/*while(curCamPos.z < (curObjPos.z - targViewDist)){ //if current cam is less than current obj minus padding
			Camera.current.transform.position.x++;	
		}
		while(curCamPos.z > (curObjPos.z - targViewDist)){ //if current cam is less than current obj minus padding
			Camera.current.transform.position.x --;	
		}*/
		}
	}
}

public function lookAtObj(){ //adjust cam FOV to accont for model size
	
	var currentObj = GameObject.Find("CurrentModel");
	var camTarget : Transform;
	
	if (currentObj != null){	
		//Look at object
		var curObjTrans = currentObj.transform;
		transform.LookAt(curObjTrans);
		
		//Frame Object /Adjust Can FOV
		var curObjPos = curObjTrans.position; //current obj position
		var curCamPos = gameObject.transform.position; //current cam pos
		
		var targetHeading = curObjPos - curCamPos; //vector direction to face obj
		var targetDistance = targetHeading.magnitude; //distance from cam to obj 
		var targetDirection = targetHeading/targetDistance; //normalised target direction - dunno if needed
		
		var curObjSize = currentObj.GetComponent(Renderer).bounds.size;
		var curObjHeight = curObjSize.y;
				
		
		var camVertFovRad = 2 * Mathf.Atan((curObjHeight/2)/targetDistance); //vertical FOV in radians
		camVertFovDeg = camVertFovRad * Mathf.Rad2Deg; //vertical FOV in degrees
		
		curCamFov = Camera.main.fieldOfView;
		
		if (curCamFov != camVertFovDeg){
			Debug.Log("Current Camera FOV" + curCamFov);
			Camera.current.fieldOfView = camVertFovDeg;
			Debug.Log("New Camera FOV" + curCamFov);
		}
	}	
}