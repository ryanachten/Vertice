#pragma strict

var goYpos : float;

function OnTriggerEnter( other : Collider)
{
	if (other.gameObject.tag == "Active Model")
	{
		//Debug.Log("Move object up");
		goYpos = other.gameObject.transform.position.y;
		
		//other.attachedRigidbody.isKinematic = true;
		//other.attachedRigidbody.useGravity = false;
	
	}
}

function OnTriggerStay( other : Collider)
{
	if (other.gameObject.tag == "Active Model")
	{
		//Debug.Log("Moving Object Up");
		 other.gameObject.transform.position.y = goYpos++;
		//other.attachedRigidbody.AddForce(Vector3.up * 10);
	
	}

}

