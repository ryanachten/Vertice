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

var browseCamMove : BrowseCamMovement;
var navMode : boolean;

var instContextMedia : BrowseContextMediaInstant;

var mediaImgViewer : GameObject; //***NEW***
var mediaViewerScript : MetaPipe_MediaV_Activate; 	//***NEW***

var curObj : GameObject; //used for edit cur obj functionality


function Start()
{
	prevObjName = null;
}


function Update()
{
//	navMode = 
	if (browseCamMove.navMode)
	{
		detectObj();
	}

	if (selectPanelActive && Input.GetMouseButtonDown(0) && !contextPanelActive)
	{
		selectContextPanel.SetActive(true);
		contextPanelActive = true;	
	} else if (!selectPanelActive) 
	{
		if (mediaImgViewer.active) //***NEW*** prevents mediaViewer remaining active between objs
//			mediaViewerScript.deactivateMediaViewer(); //***NEW***
		
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

		if (selectPanelActive)// if panel is active
		{
			deactivateSelectPanel();
			selectPanelActive = false;
		}
	}
	else if (!foundHit) //hits nothing
	{
		foundHit = false;
	
		if (selectPanelActive)// if panel is active
		{
			deactivateSelectPanel();
			selectPanelActive = false;
		}
	}
	else if (foundHit) //if the ray hits a valid object
	{
		curObjName = hit.transform.name;
		curObj = hit.transform.gameObject;	
	
		if (!selectPanelActive)// if panel is not active
		{
			activateSelectPanel();
			selectPanelActive = true;
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
