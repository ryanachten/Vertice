#pragma strict

//script handles all of the loading requisites for returning to scenes 

import System.Xml;

var infoCont : ObjInfoControl;
var collectionCont : ObjCollectionControl;


function OnLevelWasLoaded(level : int) 
{
	/*		
	//**Browse Scene Setups**	
	if (level == 0 && infoCont.control.isImpBrowse) // 0 = browse scene 
	{
		var player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent(BrowseCurObjEditLoad).loadBrowseCurObj();
		infoCont.control.isImpBrowse = false;
	}*/
	
	//**Import Scene Setups**
	//else 
	/*
	if (level == 1 && infoCont.control.isBrowseEdit) // 1 = import scene
	{
		var curObj = GameObject.FindGameObjectWithTag("Current Model");
		curObj.GetComponent(Rigidbody).isKinematic = true;
		curObj.transform.position = Vector3(0,0,0);
		infoCont.Load();
		infoCont.control.isBrowseEdit = false;
		
		Debug.Log("CurObjPos: " + curObj.transform.position);
	}*/
	
	/*
	//**Collection Scene Setups**
	else if (level == 2 && infoCont.control.isImpBrowse) // 3 = collection scene
	{	
		
		Debug.Log("Should be loading collection");
		infoCont.Start();
		collectionCont.Start();
		collectionCont.curCollectionListNode = infoCont.control.prevCollection;
		
		var curCollectionList : XmlNode = infoCont.control.prevCollection;
		
		//Debug.Log("2 prevCollection NAME: " + infoCont.control.prevCollection.SelectSingleNode("@name").Value);
		//Debug.Log("2 curCollectionList NAME: " + curCollectionList.SelectSingleNode("@name").Value);
		
		//menu panel turned off, preventing this from being accessible
		var collectCamMoveScript = GameObject.FindGameObjectWithTag("Player").GetComponent(CollectionCamMovement);

		var collectPanel = GameObject.Find("Canvas").transform.GetChild(0); //be careful not to change heirachy or this will break
		collectPanel.gameObject.SetActive(true);
		
		var collectionLoadScript = collectPanel.GetComponentInChildren(CollectionObjCollectionLoad);
		collectionLoadScript.loadCollectionObjects(curCollectionList);
		
		var menuGuiControlScript = GameObject.Find("Collection Information Panel").GetComponent(CollectionGUIcontrol);
//		menuGuiControlScript.getGuiInfo(curCollectionList); // update Collection Menu GUI
		//menuGuiControlScript.getGuiInfo(); // changed for revision
				
		infoCont.control.isImpBrowse = false;
	}
	else if (level == 2 && !infoCont.control.isImpBrowse) // used to prevent browse objects presisting
	{
		var activeObjects : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model");
		if (activeObjects != null)
		{
			Debug.Log("Deleting Existing Objects for Collection");
			for (var go : GameObject in activeObjects)
			{
				Destroy(go);
			}
		}
	}*/
	
}

