#pragma strict


//Derived from Unity Coroutines tutorial


public var smoothing : float = 1f;
private var target : Transform;
private var targOffset : float;
private var curObjoffset : Vector3;


function OnEnable () {

	var target = GameObject.FindGameObjectWithTag("Current Model").transform;
	
	var curObjCol = target.GetComponent(BoxCollider);
	
	Debug.Log("CurrentModel size x: " + target.GetComponent.<Collider>().bounds.size.x);
	
	targOffset = target.GetComponent.<Collider>().bounds.size.x;
	
	DefaultCoroutine(target);
}

function DefaultCoroutine (target : Transform) { //intro model transition

	transform.LookAt(target);
	while(Vector3.Distance(transform.position, target.position) > targOffset)
	{
		transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
				
		yield;
	}
	
	print("Reached the target");
	
	yield WaitForSeconds(3f);

	print("DefaultCoroutine is now complete");
	
	GetComponent(MetaPipe_camCoroutine03).enabled = false;
}