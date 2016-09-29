using UnityEngine;
using System.Collections;

public class Collect_RaycastModifyArtefact : MonoBehaviour {

	public Cam_RayDetect RayDetect;
	public BrowseCamMovement CamMove;
	public GameObject modArtefact;
	private string previousIdentifier;

	//modify controller variables
	public float hoverHeight = 1f; //height artefact hovers above the ground
	public float modSensitivity = 5f;
	public float modSmoothDamp = 0.1f;

	//Rotation variables
	private float yRotation;
	private float xRotation;
	private float currentYrotation;
	private float currentXrotation;
	private float yRotationV;
	private float xRotationV;

	//Scale variables
	private float xScale;
	private float currrentXscale;
	private float xScaleV;


	void FixedUpdate () {

		//Move
		if (Input.GetKey(KeyCode.E))
		{	
			if (Input.GetKeyDown(KeyCode.E))
			{
				SetupModArtefact("move");
			}

			if (modArtefact != null)
			{
				MoveObject(modArtefact);
			}
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			ResetModArtefact("move");
		}

		//Rotate
		if (Input.GetKey(KeyCode.R))
		{	
			if (Input.GetKeyDown(KeyCode.R))
			{
				SetupModArtefact("rotate");
			}

			if (modArtefact != null)
			{
				RotateObject(modArtefact);
			}
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			ResetModArtefact("rotate");
		}

		//Scale
		if (Input.GetKey(KeyCode.T))
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				SetupModArtefact("scale");
			}
				
			if (modArtefact != null)
			{
				ScaleObject(modArtefact);
			}
		}
		if (Input.GetKeyUp(KeyCode.T))
		{
			ResetModArtefact("scale");
		}
	}


	void SetupModArtefact(string modType)
	{
		modArtefact = RayDetect.curArtefact; //gets current artefact from raydetect

		if (modType == "move")
		{
			modArtefact.GetComponent<BoxCollider>().enabled = false;				
			modArtefact.GetComponent<Rigidbody>().isKinematic = true;
		}

		if (modType == "rotate") // && modArtefact != null
		{
			CamMove.navMode = false; //prevent controller turning while trying to rotate
			Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
			rb.freezeRotation = true;
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}

		if (modType == "scale")
		{
			CamMove.navMode = false; //prevent controller turning while trying to scale
			Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
			rb.freezeRotation = true;
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		}
	}


	void ResetModArtefact(string modType)
	{
		if (modType == "move")
		{
			modArtefact.GetComponent<BoxCollider>().enabled = true;
			modArtefact.GetComponent<Rigidbody>().isKinematic = false;
		}

		if (modType == "rotate")
		{
			Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
			rb.freezeRotation = false;
			rb.constraints = RigidbodyConstraints.None;
			CamMove.navMode = true;
		}

		if (modType == "scale")
		{
			Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
			rb.freezeRotation = false;
			rb.constraints = RigidbodyConstraints.None;
			CamMove.navMode = true;
		}
	}


	void MoveObject(GameObject modObj)
	{
		RaycastHit hit;
		bool foundHit = Physics.Raycast(transform.position, transform.forward, out hit);
		Debug.DrawRay(transform.position, transform.forward, Color.green);

		if(foundHit && hit.transform.tag == "Terrain")
		{
			float meshHalfHeight = modObj.GetComponent<MeshRenderer>().bounds.size.y /2; //helps account for large and small objects

			Vector3 newPos = hit.point;
			newPos.y =  newPos.y + meshHalfHeight + hoverHeight;
			modObj.transform.position = newPos;
		}
	}


	void RotateObject(GameObject modObj)
	{
		yRotation += Input.GetAxis("Mouse X") * modSensitivity;
		xRotation += Input.GetAxis("Mouse Y") * modSensitivity;

		currentXrotation = Mathf.SmoothDamp(currentXrotation, xRotation, ref xRotationV, modSmoothDamp);
		currentYrotation = Mathf.SmoothDamp(currentYrotation, yRotation, ref yRotationV, modSmoothDamp);

		modObj.transform.rotation = Quaternion.Euler(currentXrotation, currentYrotation, 0); //ZRotation currently not accounted for
	}


	void ScaleObject(GameObject modObj)
	{
		xScale -= Input.GetAxis("Mouse Y") * modSensitivity;
		xScale = Mathf.Abs(xScale);

		currrentXscale = Mathf.SmoothDamp(currrentXscale, xScale, ref currrentXscale, modSmoothDamp);

		modObj.transform.localScale = new Vector3(currrentXscale, currrentXscale, currrentXscale);
	}
}