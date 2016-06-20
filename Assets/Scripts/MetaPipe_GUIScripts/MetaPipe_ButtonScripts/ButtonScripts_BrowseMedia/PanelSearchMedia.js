#pragma strict

public var searchMediaPanel : GameObject;
public var panelActive : boolean;
public var photogramPanel : GameObject;
public var designPanel : GameObject;

function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		//Debug.Log("This should turn the panel on");
		if (photogramPanel.activeSelf)
			photogramPanel.SetActive(false);

		if (designPanel.activeSelf)
			designPanel.SetActive(false);				
		
		searchMediaPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		searchMediaPanel.SetActive(false);
		panelActive = false;
	}

}