#pragma strict

public var photogramSortPanel : GameObject;	
public var panelActive : boolean;

function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		photogramSortPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		photogramSortPanel.SetActive(false);
		panelActive = false;
	}

}