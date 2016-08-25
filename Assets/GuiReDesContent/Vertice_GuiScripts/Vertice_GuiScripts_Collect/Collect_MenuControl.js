#pragma strict

//controls the main Collection menu
//activates default functionality when no menu present

public var collectPresent : System.Boolean; //currently used as a temporary control for testing against different use cases
public var collectAvail : Collect_AvailCollectToggle;
public var menuInfoPanel : GameObject;


function Start () 
{
	if(!collectPresent) 
	{
		collectAvail.ViewCollections();
		menuInfoPanel.SetActive(false);
	}
}

