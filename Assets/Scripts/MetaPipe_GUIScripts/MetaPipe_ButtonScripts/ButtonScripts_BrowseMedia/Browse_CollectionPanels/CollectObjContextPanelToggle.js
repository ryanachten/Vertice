#pragma strict


var contextInfoXmlScript : CollectContextInfoXml;

var mediaViewerScript : MetaPipe_MediaV_Activate; //***NEW***

var objInfoPanel : GameObject;
var contextMediaPanel : GameObject;


function Start()
{
	activeObjInfo();
}


function activeObjInfo()
{
	if (objInfoPanel.activeSelf == false)
	{
		objInfoPanel.SetActive(true);
	}
	
	if (contextMediaPanel.activeSelf == true)
	{
		mediaViewerScript.deactivateMediaViewer(); //***NEW*** prevents mediaViewer remaining active between objs
		contextMediaPanel.SetActive(false);
	}
}


function activateContextMedia()
{
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