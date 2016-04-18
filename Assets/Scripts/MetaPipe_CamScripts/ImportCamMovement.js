#pragma strict

var guiMode : boolean; 

var walkAcceleration : float; //how quickly the camera accelerates
var walkDeacceleration : float = 5;
var maxWalkSpeed : float = 20;
@HideInInspector
var camMovement : Vector3; 
@HideInInspector
var walkDeaccelerationVolX : float;
@HideInInspector
var walkDeaccelerationVolY : float; 
@HideInInspector
var walkDeaccelerationVolZ : float; 
@HideInInspector
var rb : Rigidbody;
var mbScroll : int = 3;


function Start()
{
	rb = GetComponent.<Rigidbody>();
	guiMode = false;
}

function Update()
{
	if (!guiMode) //used to prevent camera movement while data input -> toggled via impFieldCheck listeners
	{
		camMovement = Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
		
		if (camMovement.magnitude > maxWalkSpeed) //clips vector to max walk speed if over maxWalkSpeed
		{
			camMovement = camMovement.normalized; 
			camMovement *= maxWalkSpeed;	
		}
		
		rb.velocity.x = camMovement.x; //new
		rb.velocity.y = camMovement.y;
		rb.velocity.z = camMovement.z;
		
		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
		{
			rb.velocity.x = Mathf.SmoothDamp(rb.velocity.x, 0, walkDeaccelerationVolX, walkDeacceleration);
			rb.velocity.y = Mathf.SmoothDamp(rb.velocity.y, 0, walkDeaccelerationVolY, walkDeacceleration);
			rb.velocity.z = Mathf.SmoothDamp(rb.velocity.z, 0, walkDeaccelerationVolZ, walkDeacceleration);
		}
		
		rb.AddRelativeForce(walkAcceleration * Input.GetAxis("Horizontal"), walkAcceleration * Input.GetAxis("Vertical"), walkAcceleration * (Input.GetAxis("Mouse ScrollWheel"))* mbScroll);//new
	}
}

