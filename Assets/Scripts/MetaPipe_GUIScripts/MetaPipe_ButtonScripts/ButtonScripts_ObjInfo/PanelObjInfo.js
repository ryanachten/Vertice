#pragma strict

public var objInfoPanel : GameObject;	
public var panelActive : boolean;

function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		objInfoPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		objInfoPanel.SetActive(false);
		panelActive = false;
	}

}