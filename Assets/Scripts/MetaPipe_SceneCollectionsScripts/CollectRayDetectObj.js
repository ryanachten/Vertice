#pragma strict

//detect nearest obj using raycast

//***Raycasting Stuff
var raycastDist : float = 2.5;
var tagCheck : String = "Active Model";
var checkAllTags : boolean = false;
//var foundHit : boolean = false;

//***ContextInfo Stuff***
var curObjName : String;
var prevObjName : String;
var selectObjPanel : GameObject;
var selectObjText : Text;
var selectPanelActive : boolean;
var contextPanelActive : boolean;
var selectContextPanel : GameObject;

var collectCamMove : CollectionCamMovement;
var navMode : boolean;

var instContextMedia : BrowseContextMediaInstant;

var mediaImgViewer : GameObject; //***NEW***
var mediaViewerScript : MetaPipe_MediaV_Activate; 	//***NEW***

var curObj : GameObject; //used for edit cur obj functionality
var modifyObjScript :  CollectionRayModifyObj;


function Start()
{
	prevObjName = null;
}


function Update()
{
	navMode = collectCamMove.navMode;
	if (navMode)
	{
		detectObj();
	}
	
	
	if (selectPanelActive && Input.GetMouseButtonDown(0) && !contextPanelActive)
	{
		//Debug.Log("Open Context Panel");
		selectContextPanel.SetActive(true);
		contextPanelActive = true;
		modifyObjScript.modObj = curObj; //update mod object in mod script
				
	} else if (!selectPanelActive) 
	{
		if (mediaImgViewer.active) //***NEW*** - prevents mediaViewer remaining active between objs
			mediaViewerScript.deactivateMediaViewer(); //***NEW***
	
		selectContextPanel.SetActive(false);
		contextPanelActive = false;	
	}
}

function detectObj() {
		
	var	foundHit : boolean = false;
	var hit : RaycastHit;
	
	foundHit = Physics.Raycast(transform.position, transform.forward, hit, raycastDist);
	

	if (foundHit && !checkAllTags && hit.transform.tag != tagCheck) //hit but not right object
	{
		foundHit = false;
		//Debug.DrawRay(transform.position, transform.forward * raycastDist, Color.blue);
		//Debug.Log("Ray hit not valid");
		
		if (selectPanelActive)// if panel is active
		{
			deactivateSelectPanel();
			selectPanelActive = false;
			//Debug.Log("DEACTIVE select panel");
		}
	}
	else if (!foundHit) //hits nothing
	{
		foundHit = false;
		//Debug.DrawRay(transform.position, transform.forward * raycastDist, Color.red);
		//Debug.Log("Ray miss");
		
		if (selectPanelActive)// if panel is active
		{
			deactivateSelectPanel();
			selectPanelActive = false;
			//Debug.Log("DEACTIVE select panel");
		}
	}
	else if (foundHit) //if the ray hits a valid object
	{
		curObjName = hit.transform.name;
		curObj = hit.transform.gameObject;	
		//Debug.Log("RAY HIT: " + curObjName);
		//Debug.DrawRay(transform.position, transform.forward * raycastDist, Color.green);
		
		if (!selectPanelActive)// if panel is not active
		{
			activateSelectPanel();
			selectPanelActive = true;
			//Debug.Log("ACTIVE select panel");
		}
			
	}
	
}
	
function activateSelectPanel()
{
	if (prevObjName != curObjName)
	{
		instContextMedia.clearChildren();
		prevObjName = curObjName;
	}
	selectObjPanel.SetActive(true); //activate context panel
	selectObjText.text = curObjName;
}

function deactivateSelectPanel()
{

	selectObjPanel.SetActive(false); //deactivate context panel
}
