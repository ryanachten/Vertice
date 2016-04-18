#pragma strict

//used to detect whether or not fields in the Import scene have lost their connection to the GameController (due to scene change delete etc)
//will reconnect if this is the case

//held on Photogram Info Panel

var designCreateField : InputField;

var objInfoAdjust : ObjInfoAdjust;
var objInfoCont : ObjInfoControl;

var impCamMoveScript : ImportCamMovement;

function Start () {
	
	if (objInfoAdjust == null)
	{
		Debug.Log("objInfoAdjust LOST - Finding");
		objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
	}
	
	if (objInfoCont == null)
	{
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
	
	if (impCamMoveScript == null)
	{
		impCamMoveScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent.<ImportCamMovement>();
	}
	
	UpdateFieldActions();

}

function UpdateFieldActions () {

	//DESIGN CREATE FIELD
	//used to prevent camera movement while data input
	designCreateField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
		
	designCreateField.onEndEdit.AddListener(function()
	{
		try
		{
			objInfoAdjust.designCreateField();	
		}
		catch(err)
		{
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.designCreateField();
		}
		
		impCamMoveScript.guiMode = false;
	});
}

