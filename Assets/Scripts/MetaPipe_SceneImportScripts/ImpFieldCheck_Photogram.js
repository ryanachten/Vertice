#pragma strict

//used to detect whether or not fields in the Import scene have lost their connection to the GameController (due to scene change delete etc)
//will reconnect if this is the case

//held on Photogram Info Panel

var locateNameField : InputField;

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
	
	//PHOTO LOCATE FIELD
	//used to prevent camera movement while data input
	locateNameField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
	//reconnects listeners assigned in editor
	locateNameField.onEndEdit.AddListener(function()
	{
		try
		{
			objInfoAdjust.photogramLocateName();	
		}
		catch(err)
		{
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.photogramLocateName();
		}
		
		impCamMoveScript.guiMode = false;
	});
}

