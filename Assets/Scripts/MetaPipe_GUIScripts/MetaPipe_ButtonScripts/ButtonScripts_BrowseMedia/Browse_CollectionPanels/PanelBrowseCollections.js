#pragma strict

public var collectionPanel : GameObject;	
public var panelActive : boolean;

public var loadCollectionAssetsScript : BrowseLoadCollectionAssets;


function Start(){

	panelActive = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		collectionPanel.SetActive(true);
		loadCollectionAssetsScript.getCollectionList();
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		collectionPanel.SetActive(false);
		panelActive = false;
	}

}