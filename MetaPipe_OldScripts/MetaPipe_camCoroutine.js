#pragma strict


//Derived from Unity Coroutines tutorial

public var smoothing : float = 1f;
private var target : Transform;
public var targOffset : float = 10f;
private var curObjoffset : Vector3;


function OnEnable () {

	var target = GameObject.Find("CurrentModel").transform;
	
	DefaultCoroutine(target);
}

function DefaultCoroutine (target : Transform) {

	while(Vector3.Distance(transform.position, target.position) > targOffset)
	{
		transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
		transform.LookAt(target);	
		
		yield;
	}
	
	print("Reached the target");
	
	yield WaitForSeconds(3f);

	print("DefaultCoroutine is now complete");
}