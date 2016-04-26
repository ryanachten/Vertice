#pragma strict


var contextInfoXmlScript : BrowseContextInfoXml;

var mediaViewerScript : MetaPipe_MediaV_Activate; //***NEW***

var objInfoPanel : GameObject;
var contextMediaPanel : GameObject;


function Start()
{
	activeObjInfo();
}


function activeObjInfo()
{
	Debug.Log("Activating Obj Info Panel");

	if (objInfoPanel.activeSelf == false)
	{
		objInfoPanel.SetActive(true);
	}
	
	if (contextMediaPanel.activeSelf == true)
	{
		contextMediaPanel.SetActive(false);
		mediaViewerScript.deactivateMediaViewer(); //***NEW*** prevents mediaViewer remaining active between objs
	}
}


function activateContextMedia()
{
	Debug.Log("Activating Context Panel");
	
	if (contextMediaPanel.activeSelf == false)
	{
		contextMediaPanel.SetActive(true);
		contextInfoXmlScript.UpdateContextInfo();
	}
	
	if (objInfoPanel.activeSelf == true)
	{
		objInfoPanel.SetActive(false);
	} 
}