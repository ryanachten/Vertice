#pragma strict


function Awake() 
{
	DontDestroyOnLoad(transform.parent.gameObject);
	
	//ensures that there is only one nav per scene 
	var curPanel = this.gameObject;
	
	var otherNavPanels : GameObject[] = GameObject.FindGameObjectsWithTag("Navigation Panel");
	for ( var panel : GameObject in otherNavPanels)
	{
		if (panel != curPanel)
		{
			Destroy(panel.transform.parent.gameObject);
		}
	}
}


