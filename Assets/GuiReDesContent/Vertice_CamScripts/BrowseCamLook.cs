using UnityEngine;
using System.Collections;

public class BrowseCamLook : MonoBehaviour {

	Camera cam;
	public BrowseCamMovement CamMoveScript;
	private bool navMode;
	private float defaultCameraAngle = 60;
	public float curTargetCamAngle = 30;
	public float ratioZoom = 1f;
	private float ratioZoomV;
	public float ratioZoomSpeed = 0.2f;
	private float yRotation;
	private float xRotation;
	private float yRotationV;
	private float xRotationV;
	public float lookSensitivity = 5f;
	public float lookSmoothDamp = 0.125f;
	public float currentXrotation;
	public float currentYrotation;

	void Start () {
	
		cam = GetComponent<Camera>();
	}

	void Update () {
		navMode = CamMoveScript.navMode;
		if (navMode) 
		{
			MouseMove();
		}
	}

	void MouseMove()
	{
		if (Input.GetMouseButton(1)) 
		{
			//Zoom code -> move into seperate function
			float mmbScroll = Input.GetAxis("Mouse ScrollWheel");
			curTargetCamAngle += (mmbScroll*2); //why is this *2? Seems sloppy

			ratioZoom = Mathf.SmoothDamp(ratioZoom, 0, ref ratioZoomV, ratioZoomSpeed);
		} else 
		{
			ratioZoom = Mathf.SmoothDamp(ratioZoom, 1, ref ratioZoomV, ratioZoomSpeed);
		}

		cam.fieldOfView = Mathf.Lerp(curTargetCamAngle, defaultCameraAngle, ratioZoom);

		yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
		xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;

		xRotation = Mathf.Clamp(xRotation, -90, 90);

		currentXrotation = Mathf.SmoothDamp(currentXrotation, xRotation, ref xRotationV, lookSmoothDamp);
		currentYrotation = Mathf.SmoothDamp(currentYrotation, yRotation, ref yRotationV, lookSmoothDamp);

		transform.rotation = Quaternion.Euler(currentXrotation, currentYrotation, 0);
	}
}

