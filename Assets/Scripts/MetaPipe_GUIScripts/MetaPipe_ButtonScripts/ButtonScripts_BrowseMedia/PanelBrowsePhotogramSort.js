#pragma strict

public var photogramSortPanel : GameObject;	
public var panelActive : boolean;

public var searchMediaPanel : GameObject;
public var designPanel : GameObject;


function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		//Debug.Log("This should turn the panel on");
		
		if (designPanel.activeSelf)
			designPanel.SetActive(false);
			
		if (searchMediaPanel.activeSelf)
			searchMediaPanel.SetActive(false);

		photogramSortPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		photogramSortPanel.SetActive(false);
		panelActive = false;
	}

}