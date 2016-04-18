#pragma strict

var walkAcceleration : float = 1000;
var walkAccelAirRatio : float = 0.1; //1 = full control 0 = no control in air
var walkDeacceleration : float = 0.5;
var maxWalkSpeed : float = 20;
@HideInInspector
var walkDeaccelerationVolx : float;
@HideInInspector
var walkDeaccelerationVolz : float;

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

var rayModScript : CollectionRayModifyObj;

var navLocked : boolean; //used to prevent user trying to move while importing

function Start()
{
	rb = GetComponent.<Rigidbody>();
	navMode = true;
	Cursor.visible = false;
	
	if (rayModScript == null)
	{
		rayModScript = GameObject.FindGameObjectWithTag("Player").GetComponent.<CollectionRayModifyObj>();	
	}
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
		
		rayModScript.guiMode = true;
	} 
	else 
	{	
		//Debug.Log("Turn on Nav Mode");
		Cursor.visible = false;
		browsePanel.SetActive(false);
		
		rb.isKinematic = false;
		rb.useGravity = true;
		
		navMode = true;
		rayModScript.guiMode = false;
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
	
	//if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && grounded)
	if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && grounded) //always slows to 0 as long as touching ground
	{
		rb.velocity.x = Mathf.SmoothDamp(rb.velocity.x, 0, walkDeaccelerationVolx, walkDeacceleration);
		rb.velocity.z = Mathf.SmoothDamp(rb.velocity.z, 0, walkDeaccelerationVolz, walkDeacceleration);
	}
	
	transform.rotation = Quaternion.Euler(0, cameraObj.GetComponent(CollectionCamLook).currentYrotation, 0);
	
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


