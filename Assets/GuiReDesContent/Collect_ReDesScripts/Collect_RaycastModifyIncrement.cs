using UnityEngine;
using System.Collections;

public class Collect_RaycastModifyIncrement : MonoBehaviour {

	public Cam_RayDetect RayDetect;
	public BrowseCamMovement CamMove;
	public GameObject modArtefact;

	//modify controller variables
	public float modWaitTime = 1f;
	public float hoverHeight = 1f; //height artefact hovers above the ground
	public float modSensitivity = 5f;
	public float modSmoothDamp = 0.1f;

	//Scale variables
	public float scaleIncrement;
	private bool canScale;
	private float xScale;
	private float currrentXscale;
	private float xScaleV;



	void Start()
	{
		canScale = true;
	}


	void FixedUpdate () {

		//Move
		if (Input.GetKey(KeyCode.T))
		{	
			if (Input.GetKeyDown(KeyCode.T))
			{
				SetupModArtefact("scale");
			}

			if (canScale)
			{
				Debug.Log("Starting increment");
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
		modArtefact = RayDetect.curArtefact; //gets current artefact from raydetect

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
		if (modType == "scale")
		{
			Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
			rb.freezeRotation = false;
			rb.constraints = RigidbodyConstraints.None;
			CamMove.navMode = true;
		}
	}


	IEnumerator ScaleObject(GameObject modObj)
	{
		canScale = false;

		float mouseVal = Input.GetAxis("Mouse X");
		float scaleFactor = 0f;
		if (mouseVal > 0)
		{
			Debug.Log("scale up");
			scaleFactor = 1 + scaleIncrement;
		}
		else{
			Debug.Log("scale down");
			scaleFactor = 1 - scaleIncrement;
		}

//		Debug.Log("scaleFactor: " + scaleFactor);

		Vector3 newScale = modObj.transform.localScale;
		newScale = new Vector3(newScale.x * scaleFactor, newScale.y * scaleFactor, newScale.z * scaleFactor);

//		Debug.Log("newScale: " + newScale);

		modObj.transform.localScale = newScale;


//		xScale -= Input.GetAxis("Mouse X") * modSensitivity;
//		xScale = Mathf.Abs(xScale);
//
//		currrentXscale = Mathf.SmoothDamp(currrentXscale, xScale, ref currrentXscale, modSmoothDamp);
//
//		Debug.Log("currrentXscale: " + currrentXscale);
//		Debug.Log("Input.GetAxis(Mouse X): " + Input.GetAxis("Mouse X"));
//
//		modObj.transform.localScale = new Vector3(currrentXscale, currrentXscale, currrentXscale);


		yield return new WaitForSeconds(modWaitTime);
		Debug.Log("Done increment");
		canScale = true;
	}
}
