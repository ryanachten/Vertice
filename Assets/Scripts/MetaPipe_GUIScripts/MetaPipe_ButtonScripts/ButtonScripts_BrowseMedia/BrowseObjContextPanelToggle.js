#pragma strict


var contextInfoXmlScript : BrowseContextInfoXml;

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