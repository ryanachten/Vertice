#pragma strict


var objNameField : InputField;

var objInfoAdjust : ObjInfoAdjust;
var objInfoCont : ObjInfoControl;

var impCamMoveScript : ImportCamMovement;

function Start()

{
	if (objInfoAdjust == null)
	{
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
	
	//OBJ NAME FIELD
	//used to prevent camera movement while data input
	objNameField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
	//reconnects listeners assigned in editor
	objNameField.onEndEdit.AddListener(function()
	{
		try
		{
			objInfoAdjust.objName();
		}
		catch(err)
		{
			Debug.Log("updating name field");
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.objName();	
		}
		
		impCamMoveScript.guiMode = false;
		Debug.Log("Updated Obj Name Field");
	});
	
}