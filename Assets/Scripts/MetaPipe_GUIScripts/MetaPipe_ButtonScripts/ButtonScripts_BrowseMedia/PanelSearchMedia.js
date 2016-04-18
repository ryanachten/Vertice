#pragma strict

public var searchMediaPanel : GameObject;	
public var panelActive : boolean;

function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		searchMediaPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		searchMediaPanel.SetActive(false);
		panelActive = false;
	}

}