#pragma strict

//used to detect whether or not fields in the Import scene have lost their connection to the GameController (due to scene change delete etc)
//will reconnect if this is the case

//held on Obj Info Panel

var modelCreatorField : InputField;
var modelCreateDateField : InputField;

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
	
	//MODEL CREATOR FIELD
	//used to prevent camera movement while data input
	modelCreatorField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
	
	modelCreatorField.onEndEdit.AddListener(function()
	{
		try
		{
			objInfoAdjust.modelCreator();	
		}
		catch(err)
		{
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.modelCreator();
		}
		
		impCamMoveScript.guiMode = false;
	});
	
	
	//MODEL DATE FIELD
	//used to prevent camera movement while data input
	modelCreateDateField.onValueChange.AddListener(function()
	{
		impCamMoveScript.guiMode = true;
	});
	
	modelCreateDateField.onEndEdit.AddListener(function()
	{	
		try
		{
			objInfoAdjust.modelCreateDate();	
		}
		catch(err)
		{
			Debug.Log("objInfoAdjust == null - finding");
			objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
			objInfoAdjust.modelCreateDate();
		}
		
		impCamMoveScript.guiMode = false;	
	});
}

