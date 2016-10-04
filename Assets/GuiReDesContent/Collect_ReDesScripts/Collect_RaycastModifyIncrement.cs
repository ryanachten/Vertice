using UnityEngine;
using System.Collections;

public class Collect_RaycastModifyIncrement : MonoBehaviour {

	public Cam_RayDetect RayDetect;
	public BrowseCamMovement CamMove;
	public GameObject modArtefact;
	public Collect_ModGrounded modGrounded;


	//modify controller variables
	public float modWaitTime = 1f;
	public float hoverHeight = 1f; //height artefact hovers above the ground
	public float modSensitivity = 5f;
	public float modSmoothDamp = 0.1f;

	//Rotate variables
	public float rotateIncrement;
	private bool canRotate;

	//Scale variables
	public float scaleIncrement;
	private bool canScale;



	void Start()
	{
		canRotate = true;
		canScale = true;
	}


	void FixedUpdate () {

		//Rotate
		if (Input.GetKey(KeyCode.R))
		{	
			if (Input.GetKeyDown(KeyCode.R))
			{
				SetupModArtefact("rotate");
			}

			if (canRotate && modArtefact != null)
			{	
				StartCoroutine(RotateObject(modArtefact));
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

			if (canScale && modArtefact != null)
			{	
				StartCoroutine(ScaleObject(modArtefact));
			}
		}
		if (Input.GetKeyUp(KeyCode.T))
		{
			ResetModArtefact("scale");
		}
	}
		


	void SetupModArtefact(string modType)
	{
		try {
			modArtefact = RayDetect.curArtefact; //gets current artefact from raydetect
		} 
		catch (System.Exception ex) {
			modArtefact = null;	
		}

		if (modArtefact != null)
		{
			if (modType == "rotate")
			{
				CamMove.navMode = false; //prevent controller turning while trying to scale

				Collect_ModGrounded groundScript = modArtefact.GetComponent<Collect_ModGrounded>();
				if (groundScript == null)
				{
					Debug.Log("Adding script");
					groundScript = modArtefact.AddComponent<Collect_ModGrounded>();
				}

				if(groundScript.grounded)
				{
					Bounds meshBounds = modArtefact.GetComponent<MeshRenderer>().bounds;
					float[] meshSize = new float[3]{meshBounds.size.x, meshBounds.size.y, meshBounds.size.z};
					float meshMax = Mathf.Max(meshSize) /2;
					Debug.Log("meshMax: " + meshMax);

					Debug.Log("HOVR modArtefact.transform.position: " + modArtefact.transform.position);
					modArtefact.transform.position = new Vector3(modArtefact.transform.position.x, modArtefact.transform.position.y + meshMax, modArtefact.transform.position.z);
					Debug.Log("GND modArtefact.transform.position: " + modArtefact.transform.position);
				}

				Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
				rb.freezeRotation = true;
				rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				rb.isKinematic = true;
			}

			if (modType == "scale")
			{
				CamMove.navMode = false; //prevent controller turning while trying to scale
				Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
				rb.freezeRotation = true;
				rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
			}
		}
	}


	void ResetModArtefact(string modType)
	{
		if (modArtefact != null)
		{
			if (modType == "rotate")
			{

				Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
				rb.freezeRotation = false;
				rb.constraints = RigidbodyConstraints.None;
				rb.isKinematic = false;
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
	}

	IEnumerator RotateObject(GameObject modObj)
	{
		canRotate = false;

		float mouseX = Input.GetAxis("Mouse X");
		float xRotateFactor = 0f;
		Debug.Log("mouseX: " + mouseX);
		if (mouseX > 0)
		{
			Debug.Log("x rot up");
			xRotateFactor = rotateIncrement;
		}
		else if (mouseX < 0){
			Debug.Log("x rot down");
			xRotateFactor = rotateIncrement * -1;
		}
			
		float mouseY = Input.GetAxis("Mouse Y");
		Debug.Log("mouseY: " + mouseY);
		float yRotateFactor = 0f;
		if (mouseY > 0)
		{
			Debug.Log("y rot up");
			yRotateFactor = rotateIncrement;
		}
		else if (mouseY < 0)
		{
			Debug.Log("y rot down");
			yRotateFactor = rotateIncrement * -1;
		}
			
		modObj.transform.Rotate(yRotateFactor, xRotateFactor, 0);


		yield return new WaitForSeconds(modWaitTime);
		canRotate = true;
	}


	IEnumerator ScaleObject(GameObject modObj)
	{
		canScale = false;

		float mouseVal = Input.GetAxis("Mouse X");
		float scaleFactor = 1f;
		if (mouseVal > 0)
		{
			Debug.Log("scale up");
			scaleFactor = 1 + scaleIncrement;
		}
		else if (mouseVal < 0) {
			Debug.Log("scale down");
			scaleFactor = 1 - scaleIncrement;
		}
			
		Vector3 newScale = modObj.transform.localScale;
		newScale = new Vector3(newScale.x * scaleFactor, newScale.y * scaleFactor, newScale.z * scaleFactor);

		modObj.transform.localScale = newScale;


		yield return new WaitForSeconds(modWaitTime);
		canScale = true;
	}
}
