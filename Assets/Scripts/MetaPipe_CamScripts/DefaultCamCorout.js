#pragma strict


//Derived from Unity Coroutines tutorial
//Derived from camCoroutine03

//Default into transition for model import - needs to be fixed


public var smoothing : float = 1f;
private var target : GameObject;
private var targOffset : float;
private var curObjoffset : Vector3;

//
//function OnEnable () {
//
//	
//	var curObjCol = target.GetComponent(BoxCollider);
//	
//	DefaultCoroutine(target);
//}



function DefaultCamCorout() { //intro model transition

	//target = GameObject.FindGameObjectWithTag("Current Model").transform;
	target = GameObject.FindGameObjectWithTag("Current Model");
	Debug.Log("target name: " + target.name);
	
	targOffset = target.transform.GetComponent.<Collider>().bounds.size.x;
	
	transform.LookAt(target.transform);
	while(Vector3.Distance(transform.position, target.transform.position) > targOffset) //**REVISE** calling before new transform is instantiated
	{
		transform.position = Vector3.Lerp(transform.transform.position, target.transform.position, smoothing * Time.deltaTime);		
		yield;
	}
	
	yield WaitForSeconds(3f);
	print("DefaultCoroutine is now complete");
	
}
