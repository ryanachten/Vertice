#pragma strict

public var designSortPanel : GameObject;	
public var panelActive : boolean;

public var photogramSortPanel : GameObject;
public var searchMediaPanel : GameObject;


function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		if (photogramSortPanel.activeSelf)
			photogramSortPanel.SetActive(false);
			
		if (searchMediaPanel.activeSelf)
			searchMediaPanel.SetActive(false);
		
		
		designSortPanel.SetActive(true);
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		designSortPanel.SetActive(false);
		panelActive = false;
	}

}