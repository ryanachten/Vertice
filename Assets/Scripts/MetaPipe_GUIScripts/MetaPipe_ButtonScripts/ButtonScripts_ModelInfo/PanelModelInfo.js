#pragma strict

public var modelInfoPanel : GameObject;	
public var panelActive : boolean;
public var objReport : ObjReport;


function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		modelInfoPanel.SetActive(true);
		objReport.ObjReport();
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		modelInfoPanel.SetActive(false);
		panelActive = false;
	}

}