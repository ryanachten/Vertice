#pragma strict


var tagetInputField : GameObject;


function toggleField()
{
	if (tagetInputField.activeSelf == false)
	{
		Debug.Log("Field on");
		tagetInputField.SetActive(true);
	}
	
	else if (tagetInputField.activeSelf == true)
	{
		Debug.Log("Field off");
		tagetInputField.SetActive(false);
	}
}
