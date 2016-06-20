#pragma strict


var browCam : Camera;

function FixedUpdate () {

			transform.LookAt(transform.position + browCam.transform.rotation * Vector3.forward, browCam.transform.rotation * Vector3.up);

}