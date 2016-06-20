#pragma strict

var prefPanel : GameObject;

var helpPanel : GameObject;

var verticeHelpPanel : GameObject;
var browHelpPanel : GameObject;
var impHelpPanel : GameObject;
var collectHelpPanel : GameObject;


function helpToggle()
{
	if (!helpPanel.activeSelf){
		//Toggle Help On
		if(prefPanel.activeSelf)
			prefPanel.SetActive(false);
			
		helpPanel.SetActive(true);
		activateVerticeHelp();
	}
	else if (helpPanel.activeSelf){
		//Toggle Help Off
		helpPanel.SetActive(false);
	}
}

function prefsToggle()
{
	if (!prefPanel.activeSelf){
		//Toggle Prefs On
		if(helpPanel.activeSelf)
			helpPanel.SetActive(false);
			
		prefPanel.SetActive(true);
	}
	else if (prefPanel.activeSelf){
		//Toggle Prefs Off
		prefPanel.SetActive(false);
	}
}


function activateVerticeHelp()
{
	verticeHelpPanel.SetActive(true);
	
	browHelpPanel.SetActive(false);
	impHelpPanel.SetActive(false);
	collectHelpPanel.SetActive(false);	
}


function activateBrowseHelp()
{
	browHelpPanel.SetActive(true);
	
	verticeHelpPanel.SetActive(false);
	impHelpPanel.SetActive(false);
	collectHelpPanel.SetActive(false);	
}

function activateImportHelp()
{
	impHelpPanel.SetActive(true);
	
	verticeHelpPanel.SetActive(false);
	browHelpPanel.SetActive(false);
	collectHelpPanel.SetActive(false);	
}

function activateCollectHelp()
{
	collectHelpPanel.SetActive(true);
	
	verticeHelpPanel.SetActive(false);
	browHelpPanel.SetActive(false);
	impHelpPanel.SetActive(false);	
}
