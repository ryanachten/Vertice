using UnityEngine;
using System.Collections;

public class BrowseCamMovement : MonoBehaviour {

 	public BrowseCamLook CamLookScript;

	public Rigidbody rb;
	public bool defaultMode; //used to set nav/gui mode at runtime
	public bool navMode; //used to toggle between GUI menu and navigation mode
	public bool navLocked; //used to prevent user trying to move while importing
//	private Transform transform; //use?
//	private Transform curPos; //use? not currently used

	//navigation vars
	public float maxWalkSpeed = 20f;
	private bool grounded;
	public float walkAcceleration = 1000;
	public float walkDeacceleration = 0.1f;
	private float walkDeaccelerationVolx;
	private float walkDeaccelerationVolz;
	public float walkAccelAirRatio = 0.1f; //1 = full control 0 = no control in air
	public float airDeacceleration = 1f;
	private float airDeaccelerationVolx;
	private float airDeaccelerationVolz;
	private float jumpVelocity = 20;

	private float maxSlope = 60;

	private int flyMass = 1;
	private int fallMass = 3;
	private int groundedMass = 1;

	public GameObject browseGui; //browse GUI to be toggled on an off depending on whether nav/gui mode


	void Start () {
		rb = GetComponent<Rigidbody>();
		navMode = defaultMode;
		toggleNav();
	}


	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab) && !navLocked)
		{
			toggleNav();
		}
		
		if(navMode)
		{
			navigationMode();	
		} 
	}

	void toggleNav()
	{
		if (navMode) { //turn off navMode
			Cursor.visible = true;
			browseGui.SetActive(true); 
//			curPos = transform; //this will be used to hold the player in place while scene is paused - I think; not currently used
			rb.isKinematic = true; 
			rb.useGravity = false; 
			navMode = false;
		}
		else { //turn on navMode
			Cursor.visible = false;
			browseGui.SetActive(false);
			rb.isKinematic = false; 
			rb.useGravity = true; 
			navMode = true;
		}
	}

	void navigationMode()
	{
		Vector2 horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z); 
		if(horizontalMovement.magnitude > maxWalkSpeed) 
		{
			horizontalMovement = horizontalMovement.normalized;
			horizontalMovement *= maxWalkSpeed;
		}

		Vector3 horizontalV = rb.velocity;
		horizontalV.x = horizontalMovement.x;
		horizontalV.z = horizontalMovement.y;
		rb.velocity = horizontalV;
//		Debug.Log("horizontalV.x: " + horizontalV.x + " horizontalV.z: " + horizontalV.z);

		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && grounded) //walking deacceleration
		{
			Vector3 walkDeclerationV = rb.velocity;
			walkDeclerationV.x = Mathf.SmoothDamp(rb.velocity.x, 0, ref walkDeaccelerationVolx, walkDeacceleration);
			walkDeclerationV.z = Mathf.SmoothDamp(rb.velocity.z, 0, ref walkDeaccelerationVolz, walkDeacceleration);
			rb.velocity = walkDeclerationV;
		}
		else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && !grounded) 
		{
			Vector3 airDeclerationV = rb.velocity;
			airDeclerationV.x = Mathf.SmoothDamp(rb.velocity.x, 0, ref airDeaccelerationVolx, airDeacceleration);
			airDeclerationV.z = Mathf.SmoothDamp(rb.velocity.z, 0, ref airDeaccelerationVolz, airDeacceleration);
		}

		transform.rotation = Quaternion.Euler(0, CamLookScript.currentYrotation, 0);

		if(grounded)
		{
			rb.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration * Time.deltaTime, 0, Input.GetAxis("Vertical") * walkAcceleration * Time.deltaTime);
		} else 
		{
			rb.AddRelativeForce(Input.GetAxis("Horizontal") * walkAcceleration * walkAccelAirRatio * Time.deltaTime, 0, Input.GetAxis("Vertical") * walkAcceleration * walkAccelAirRatio * Time.deltaTime);
		}

		//Flying
		if(Input.GetButton("Jump")){ 
			rb.AddForce(0, jumpVelocity, 0);
			rb.mass = flyMass;
		} else if (!Input.GetButton("Jump") && !grounded)
		{
			rb.mass = fallMass;
		} else 
		{
			rb.mass = groundedMass;	
		}

		//Speed up / slow down controller
		if (!Input.GetMouseButton(1))
		{
			float mmbScrollWheel = Input.GetAxis("Mouse ScrollWheel");
			maxWalkSpeed -= mmbScrollWheel;
			jumpVelocity -= mmbScrollWheel;

			float fFallMass = (float)fallMass;
			fFallMass -= mmbScrollWheel;
			fallMass = Mathf.FloorToInt(Mathf.Clamp(fFallMass, 3f, 10f));
		}
	}

	//***Jump Code*** could be useful for detection purposes
	//have left due to the grounded boolean; take this out for height nav
	void OnCollisionStay(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (Vector3.Angle(contact.normal, Vector3.up) < maxSlope)
				grounded = true;
		}
	}
	
	void OnCollisionExit()
	{
		grounded = false;
	}

}