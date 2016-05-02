#pragma strict

public var modCurObjPanel : GameObject;	
public var panelActive : boolean;


function Start(){

	panelActive = false;
	modCurObjPanel.SetActive(false);
}


function PanelControl(){

	if (panelActive == false){
		
//		Debug.Log("This should turn the panel on");
		modCurObjPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
//		Debug.Log("This should turn the panel off");
		modCurObjPanel.SetActive(false);
		panelActive = false;
	}

}