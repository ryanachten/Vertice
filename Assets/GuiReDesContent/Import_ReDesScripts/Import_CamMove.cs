using UnityEngine;
using System.Collections;

public class Import_CamMove : MonoBehaviour {

	public bool GuiMode;
	public Rigidbody camRb; 


	Vector3 camMovement; //var camMovement : Vector3; 

	public float maxWalkSpeed = 20; //var maxWalkSpeed : float = 20;

	public float walkAcceleration; //var walkAcceleration : float; //how quickly the camera accelerates
	public float walkDeacceleration = 5; //var walkDeacceleration : float = 5;
	float walkDeaccelerationVolX; //var walkDeaccelerationVolX : float;
	float walkDeaccelerationVolY;
	float walkDeaccelerationVolZ;

	public int mbScroll = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GuiMode) //used to prevent camera movement while data input -> toggled via impFieldCheck listeners
		{

			camMovement = camRb.velocity;

			if (camMovement.magnitude > maxWalkSpeed) //clips vector to max walk speed if over maxWalkSpeed
			{
				camMovement = camMovement.normalized; 
				camMovement *= maxWalkSpeed;	
			}

			if (Input.GetAxis("Horizontal") == 0)
			{
				camMovement.x = Mathf.SmoothDamp(camRb.velocity.x, 0, ref walkDeaccelerationVolX, walkDeacceleration);
			}

			if (Input.GetAxis("Vertical") == 0)
			{
				camMovement.y = Mathf.SmoothDamp(camRb.velocity.y, 0, ref walkDeaccelerationVolY, walkDeacceleration);
			}

			if (Input.GetAxis("Mouse ScrollWheel") == 0)
			{
				camMovement.z = Mathf.SmoothDamp(camRb.velocity.z, 0, ref walkDeaccelerationVolZ, walkDeacceleration);
			}

			camRb.velocity = camMovement;

			camRb.AddRelativeForce(walkAcceleration * -Input.GetAxis("Horizontal"), walkAcceleration * -Input.GetAxis("Vertical"), walkAcceleration * -(Input.GetAxis("Mouse ScrollWheel"))* mbScroll); //fixed inverted issue
		}
	}
}
