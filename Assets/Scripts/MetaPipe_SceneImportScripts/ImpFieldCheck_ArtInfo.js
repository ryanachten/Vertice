#pragma strict

//used to detect whether or not fields in the Import scene have lost their connection to the GameController (due to scene change delete etc)
//will reconnect if this is the case

//held on Obj Info Panel

var objNameField : InputField;
var contributorNameField : InputField;
var objDescriptField : InputField;


var objInfoAdjust : ObjInfoAdjust;
var objInfoCont : ObjInfoControl;

var impCamMoveScript : ImportCamMovement;


function Start () {
	
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
	});
	
	
	//CONTRIB NAME FIELD
	//used to prevent camera movement while data input
	contributorNameField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
	
	contributorNameField.onEndEdit.AddListener(function()
	{
		try
		{
			objInfoAdjust.contribName();
		}
		catch(err)
		{
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.contribName();
		}
		
		impCamMoveScript.guiMode = false;
	});


	//OBJ DESCRIPT FIELD
	//used to prevent camera movement while data input
	objDescriptField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
	
	objDescriptField.onEndEdit.AddListener(function()
	{
		try
		{
			objInfoAdjust.objDescript();
		}
		catch(err)
		{	
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.objDescript();	
		}
		
		impCamMoveScript.guiMode = false;
	});
}

