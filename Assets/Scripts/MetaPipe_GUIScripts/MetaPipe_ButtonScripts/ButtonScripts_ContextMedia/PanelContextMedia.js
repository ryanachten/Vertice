#pragma strict

import System.Xml;

public var contextInfoScrollPanel : GameObject;
public var contextMediaPanel : GameObject;	
public var instContextMedia : InstantContextMedia;
public var panelActive : boolean;
public var panelUpdated : boolean;
public var objCont : ObjInfoControl;

function Start(){

	panelActive = false;
	panelUpdated = false;
}


function PanelControl(){

	if (panelActive == false){
		
		//Debug.Log("This should turn the panel on");
		contextInfoScrollPanel.SetActive(true);
		if (objCont.control.objName.Length > 1)
		{
			loadContextMedia();
		}
		panelActive = true;
	}
	
	else if (panelActive == true){
		
		//Debug.Log("This should turn the panel off");
		
//		//Deletes context media instances to prevent double up and allow refresh
		var childs : int = contextMediaPanel.transform.childCount;
		for(var i = childs -1; i>=1; i--){
			Destroy(contextMediaPanel.transform.GetChild(i).gameObject);
		}
		
		contextInfoScrollPanel.SetActive(false);
		panelActive = false;
	}

}

function loadContextMedia()
{
	if (contextMediaPanel.activeSelf){
		
		if (objCont == null)
		{
			objCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>(); //FindGameObjectWithTag("MainCamera").GetComponent.<ImportObj>();
		}
		
		if (objCont.control.cntxtMediaNodes != null)
		{
			var cntxtMediaNodes : XmlNodeList = objCont.control.cntxtMediaNodes;
			if (cntxtMediaNodes.Count > 0) //wont attempt to parse context media data if no nodes present
			{
				Debug.Log("Context Working Here");
				for (var i = 0; i < cntxtMediaNodes.Count; i++) //interate through context nodes
				{
					//For Each Media Node
					var curcntxtMediaNode = cntxtMediaNodes[i];
					//Get Temp Information
					objCont.control.cntxtMediaName = curcntxtMediaNode.SelectSingleNode("MediaName").InnerText;
					objCont.control.cntxtMediaType = curcntxtMediaNode.SelectSingleNode("MediaType").InnerText;
					objCont.control.cntxtMediaLocation = curcntxtMediaNode.SelectSingleNode("MediaLocation").InnerText;
					//Execute GUI instantiation based on mediaType
					if(objCont.control.cntxtMediaType == "Image"){
						instContextMedia.instantXmlImg();
					} else if(objCont.control.cntxtMediaType == "Video"){
						instContextMedia.instantXmlVid();
					} else if(objCont.control.cntxtMediaType == "Audio"){
						instContextMedia.instantXmlAud();
					}
				}
			}
		}
	}
}