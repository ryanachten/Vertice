#pragma strict

//used to detect whether or not fields in the Import scene have lost their connection to the GameController (due to scene change delete etc)
//will reconnect if this is the case

//held on Obj Info Panel

var posXField : InputField;
var posZField : InputField;

var rotXField : InputField;
var rotYField : InputField;
var rotZField : InputField;

var scaleField : InputField;
var widthField : InputField;
var heightField : InputField;
var depthField : InputField;

var modObjGuiScript : CollectModifyObjGUI;

var objInfoAdjust : ObjInfoAdjust;
var objInfoCont : ObjInfoControl;


function Start () {
	
	/*if (objInfoAdjust == null)
	{
		objInfoAdjust = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoAdjust>();
	}
	
	if (objInfoCont == null)
	{
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}*/
	
	UpdateFieldActions();

}

function UpdateFieldActions () {
	
	if (modObjGuiScript == null)
	{
		modObjGuiScript = gameObject.GetComponent.<CollectModifyObjGUI>();	
	}
	
	//POSITION
	posXField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modXPos();
	});
	
	posZField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modZPos();
	});
	
	//ROTATION
	rotXField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modXRotate();
	});
	
	rotYField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modYRotate();
	});
	
	rotZField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modZRotate();
	});
	
	//SCALE
	scaleField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modScale();
	});	
	
	widthField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modWidth();
	});	
	
	heightField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modHeight();
	});	
	
	depthField.onEndEdit.AddListener(function()
	{
		modObjGuiScript.modDepth();
	});	
}

