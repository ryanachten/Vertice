#pragma strict

public var designInfoPanel : GameObject;	
public var panelActive : boolean;


function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		designInfoPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		designInfoPanel.SetActive(false);
		panelActive = false;
	}

}