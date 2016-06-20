#pragma strict

var walkAcceleration : float = 1000;
var walkAccelAirRatio : float = 0.1; //1 = full control 0 = no control in air
var maxWalkSpeed : float = 20;

var walkDeacceleration : float = 0.5;
var airDeacceleration : float = 0.5; //***NEW***
@HideInInspector
var walkDeaccelerationVolx : float;
@HideInInspector
var walkDeaccelerationVolz : float;
@HideInInspector
var airDeaccelerationVolx : float; //***NEW***
@HideInInspector
var airDeaccelerationVolz : float; //***NEW***



@HideInInspector
var horizontalMovement : Vector2;
@HideInInspector
var cameraObj : GameObject;
@HideInInspector
var rb : Rigidbody;
var flyMass : int = 1;
var fallMass : float = 3;
var groundedMass : int = 1;

var jumpVelocity : float = 20;
@HideInInspector
var grounded : boolean = false;
var maxSlope : float = 60;
//@HideInInspector
var navMode : boolean;
var browsePanel : GameObject;
var curPos : Transform;

var navLocked : boolean; //used to prevent user trying to move while importing

function Start()
{
	rb = GetComponent.<Rigidbody>();
	navMode = true;
	Cursor.visible = false;
}


function Update()
{
	if (Input.GetKeyDown(KeyCode.Tab) && !navLocked)
	{
		toggleNav();
	}
	
	if(navMode)
	{
		navigationMode();	
	} 
}

//***Toggle Code***
function toggleNav()
{
	if (navMode)
	{
		//Debug.Log("Turn on Browse Mode");
		Cursor.visible = true;
		browsePanel.SetActive(true);
		navMode = false;
		curPos = transform;
		
		rb.isKinematic = true;
		rb.useGravity = false;
	} 
	else 
	{	
		//Debug.Log("Turn on Nav Mode");
		Cursor.visible = false;
		browsePanel.SetActive(false);
		
		rb.isKinematic = false;
		rb.useGravity = true;
		
		navMode = true;
	}
}



function navigationMode()
{
	horizontalMovement = Vector2(rb.velocity.x, rb.velocity.z);
	if(horizontalMovement.magnitude > maxWalkSpeed)
	{
		horizontalMovement = horizontalMovement.normalized;
		horizontalMovement *= maxWalkSpeed;
	}
	rb.velocity.x = horizontalMovement.x;
	rb.velocity.z = horizontalMovement.y;
	
	if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && grounded) //deacceleration when walking
	{
		rb.velocity.x = Mathf.SmoothDamp(rb.velocity.x, 0, walkDeaccelerationVolx, walkDeacceleration);
		rb.velocity.z = Mathf.SmoothDamp(rb.velocity.z, 0, walkDeaccelerationVolz, walkDeacceleration);
	
	}
	
	if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && !grounded) //***NEW*** deaccelerates in air 
	{
		rb.velocity.x = Mathf.SmoothDamp(rb.velocity.x, 0, airDeaccelerationVolx, airDeacceleration);
		rb.velocity.z = Mathf.SmoothDamp(rb.velocity.z, 0, airDeaccelerationVolz, airDeacceleration);
	}
	
	transform.rotation = Quaternion.Euler(0, cameraObj.GetComponent(BrowseCamLook).currentYrotation, 0);
	
	if(grounded)
	{
		rb.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration * Time.deltaTime, 0, Input.GetAxis("Vertical") * walkAcceleration * Time.deltaTime);
	} else 
	{
		rb.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration * walkAccelAirRatio * Time.deltaTime, 0, Input.GetAxis("Vertical") * walkAcceleration * walkAccelAirRatio * Time.deltaTime);
	}
	
		
	//Fly/Jump Code	
	//if(Input.GetButtonDown("Jump") && grounded){ //for jump
	if(Input.GetButton("Jump")){ //for fly
		rb.AddForce(0, jumpVelocity, 0);
		rb.mass = flyMass;
	} else if (!Input.GetButton("Jump") && !grounded)
	{
		rb.mass = fallMass;
	} else 
	{
		rb.mass = groundedMass;	
	}
	
	//Speed up / slow down 
	if (!Input.GetMouseButton(1))
	{
		var mmbScrollWheel = Input.GetAxis("Mouse ScrollWheel");
		maxWalkSpeed -= mmbScrollWheel;
		jumpVelocity -= mmbScrollWheel;
		fallMass -= mmbScrollWheel;
		fallMass = Mathf.FloorToInt(Mathf.Clamp(fallMass, 3f, 10f));
	}
}



//***Jump Code*** could be useful for detection purposes
//have left due to the grounded boolean; take this out for height nav
function OnCollisionStay(collision : Collision)
{
	for ( var contact : ContactPoint in collision.contacts)
	{
		if (Vector3.Angle(contact.normal, Vector3.up) < maxSlope)
			grounded = true;
	}
}

function OnCollisionExit()
{
	grounded = false;
}


