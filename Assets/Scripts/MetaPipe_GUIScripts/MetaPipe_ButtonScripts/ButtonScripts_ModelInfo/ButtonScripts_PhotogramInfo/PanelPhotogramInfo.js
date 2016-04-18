#pragma strict

public var photoInfoPanel : GameObject;	
public var panelActive : boolean;


function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		photoInfoPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		photoInfoPanel.SetActive(false);
		panelActive = false;
	}

}