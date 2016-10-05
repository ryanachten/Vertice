using UnityEngine;
using System.Collections;

public class Collect_RaycastModifyIncrement : MonoBehaviour {

	public Cam_RayDetect RayDetect;
	public BrowseCamMovement CamMove;
	public GameObject modArtefact;
	public Collect_ModGrounded modGrounded;
	public GameObject modifyHelpPanel;
	public Collect_ModifyHelp modHelpCont;

	//modify controller variables
	public float modWaitTime = 1f;
	public float hoverHeight = 1f; //height artefact hovers above the ground
	public float modSensitivity = 5f;
	public float modSmoothDamp = 0.1f;

	//Move variables
	public Camera mainCamera;
	public AudioListener mainCamListener;
	public GameObject topDownCam;
	private bool canMove;
	public float screenSpaceZ = 475f;

	//Rotate variables
	public float rotateIncrement;
	private bool canRotate;

	//Scale variables
	public float scaleIncrement;
	private bool canScale;


	void Start()
	{
		canMove = true;
		canRotate = true;
		canScale = true;
	}


	void FixedUpdate () 
	{

		//Move TODO this might need to moved to a seperate script
		if (Input.GetKey(KeyCode.E))
		{	
			if (Input.GetKeyDown(KeyCode.E))
			{
				SetupModArtefact("move");
			}

			if (canMove && modArtefact != null && Input.GetMouseButtonDown(0))
			{	
				StartCoroutine(MoveObject(modArtefact));
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
			modifyHelpPanel.SetActive(true);

			if (modType == "move")
			{
				modHelpCont.ActivateModifyHelp("move");

				CamMove.navMode = false;

				ToggleMainCam(false);

			}

			if (modType == "rotate")
			{
				modHelpCont.ActivateModifyHelp("rotate");

				CamMove.navMode = false; //prevent controller turning while trying to scale

				Collect_ModGrounded groundScript = modArtefact.GetComponent<Collect_ModGrounded>();
				if (groundScript == null)
				{
					groundScript = modArtefact.AddComponent<Collect_ModGrounded>();
				}

				if(groundScript.grounded)
				{
					Bounds meshBounds = modArtefact.GetComponent<MeshRenderer>().bounds;
					float[] meshSize = new float[3]{meshBounds.size.x, meshBounds.size.y, meshBounds.size.z};
					float meshMax = Mathf.Max(meshSize) /2;

					modArtefact.transform.position = new Vector3(modArtefact.transform.position.x, modArtefact.transform.position.y + meshMax, modArtefact.transform.position.z);
				}

				Rigidbody rb = modArtefact.GetComponent<Rigidbody>();
				rb.freezeRotation = true;
				rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
				rb.isKinematic = true;
			}

			if (modType == "scale")
			{
				modHelpCont.ActivateModifyHelp("scale");

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
			modifyHelpPanel.SetActive(false);

			if (modType == "move")
			{
				ToggleMainCam(true);
				CamMove.navMode = true;
			}

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


	void ToggleMainCam(bool mainCamOn)
	{
		if (!mainCamOn)
		{
			topDownCam.SetActive(true);
			mainCamera.enabled = false;
			mainCamListener.enabled = false;
			Cursor.visible = true;
		}
		else
		{
			topDownCam.SetActive(false);
			mainCamera.enabled = true;
			mainCamListener.enabled = true;
		}
	}



	IEnumerator MoveObject(GameObject modObj)
	{
		canMove = false;

		Vector3 mousePos = Input.mousePosition;
		mousePos.z = screenSpaceZ;
		Vector3 worldMousePos = topDownCam.GetComponent<Camera>().ScreenToWorldPoint(mousePos);

		RaycastHit hit;
		if (Physics.Raycast(worldMousePos, -Vector3.up, out hit))
		{
			Bounds meshBounds = modObj.GetComponent<MeshRenderer>().bounds;
			float[] meshSize = new float[3]{meshBounds.size.x, meshBounds.size.y, meshBounds.size.z};
			float meshMax = Mathf.Max(meshSize) /2;

			worldMousePos.y = hit.point.y + meshMax;
		}

		modObj.transform.position = worldMousePos;


		yield return new WaitForSeconds(modWaitTime);
		canMove = true;
	}


	IEnumerator RotateObject(GameObject modObj)
	{
		canRotate = false;

		float xRotateFactor = 0f;
		if(!Input.GetMouseButton(0))
		{
			float mouseX = Input.GetAxis("Mouse X");
			if (mouseX > 0)
			{
				xRotateFactor = rotateIncrement * -1;
			}
			else if (mouseX < 0)
			{
				xRotateFactor = rotateIncrement;
			}
		}

		float yRotateFactor = 0f;
		if(Input.GetMouseButton(0))
		{
			float mouseY = Input.GetAxis("Mouse Y");

			if (mouseY > 0)
			{
				yRotateFactor = rotateIncrement;
			}
			else if (mouseY < 0)
			{
				yRotateFactor = rotateIncrement * -1;
			}
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
			scaleFactor = 1 + scaleIncrement;
		}
		else if (mouseVal < 0) 
		{
			scaleFactor = 1 - scaleIncrement;
		}
			
		Vector3 newScale = modObj.transform.localScale;
		newScale = new Vector3(newScale.x * scaleFactor, newScale.y * scaleFactor, newScale.z * scaleFactor);

		modObj.transform.localScale = newScale;


		yield return new WaitForSeconds(modWaitTime);
		canScale = true;
	}
}
