#pragma strict

public var browseMediaPanel : GameObject;	
public var panelActive : boolean;

function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		browseMediaPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		browseMediaPanel.SetActive(false);
		panelActive = false;
	}

}