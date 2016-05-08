#pragma strict


var feedback : GameObject;


function Feedback()
{
//	Debug.Log("Feedback activated");
	feedback.SetActive(true);
	
	yield WaitForSeconds(3);

	feedback.SetActive(false);
//	Debug.Log("Feedback deactivated");
}