#pragma strict
import System.Xml;


//xml elements
var infoCont : ObjInfoControl;
private var xmlDoc : XmlDocument;
private var xmlRoot : XmlNode;

public var fileName : String;


var instContextMedia : BrowseContextMediaInstant;

//UI elements
var objName : Text;
var contributorName : Text;
var description : Text;

var rayDetectScript : BrowseRayDetectObj;
@HideInInspector
var curObjName : String;
@HideInInspector
var prevObjName : String; 
var scrollBar : Scrollbar;

var toggleContextPanelScript : BrowseObjContextPanelToggle;
var contextualMediaButton : GameObject; //new
var contextualMediaPanel : GameObject;


function Start()
{
	prevObjName = "";
}

function OnGUI()
{
	curObjName = rayDetectScript.curObjName;
	if ( curObjName != prevObjName)
	{
		toggleContextPanelScript.activeObjInfo(); //new
		UpdateContextInfo();
	}	
}

function UpdateContextInfo()
{
	xmlRoot = infoCont.control.root;
	
	objName.text = curObjName;
	
	var curObjNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
	var contribNameNode = curObjNode.SelectSingleNode("ContribName");
		contributorName.text = contribNameNode.InnerText;
	var descriptNode = 	curObjNode.SelectSingleNode("Description");
		description.text = descriptNode.InnerText;
	
	var fileNameNode = curObjNode.SelectSingleNode("FileName");
	fileName = fileNameNode.InnerText;
				
	var cntxtMediaNodes = curObjNode.SelectSingleNode("ContextualInfo").ChildNodes;
	if (cntxtMediaNodes.Count > 0)
	{
		contextualMediaButton.SetActive(true); //NEW
		if (contextualMediaPanel.activeSelf == true) //NEW
		{ 	
			LoadContextMedia(cntxtMediaNodes); //NEW
		}
	} 
	else if (cntxtMediaNodes.Count == 0 || cntxtMediaNodes.Count == null) 
	{
		contextualMediaButton.SetActive(false); //NEW
	}
	prevObjName = curObjName;
}





public function LoadContextMedia(cntxtMediaNodes : XmlNodeList){

	if (cntxtMediaNodes.Count > 0) //wont attempt to parse context media data if no nodes present
	{
		instContextMedia.clearChildren(); //new
		for (var i = 0; i < cntxtMediaNodes.Count; i++) //interate through context nodes
		{
			//For Each Media Node
			var curcntxtMediaNode = cntxtMediaNodes[i];
			//Get Temp Information
			var cntxtMediaName = curcntxtMediaNode.SelectSingleNode("MediaName").InnerText;
				//Debug.Log("cntxtMediaName: " + cntxtMediaName);
			var cntxtMediaType = curcntxtMediaNode.SelectSingleNode("MediaType").InnerText;
				//Debug.Log("cntxtMediaType: " + cntxtMediaType);
			var cntxtMediaLocation = curcntxtMediaNode.SelectSingleNode("MediaLocation").InnerText;
				//Debug.Log("cntxtMediaLocation: " + cntxtMediaLocation);
			//Execute GUI instantiation based on mediaType
			if(cntxtMediaType == "Image"){
				//Debug.Log("Execute Image GUI Instant");
				instContextMedia.instantXmlImg(cntxtMediaName, cntxtMediaLocation);
			} else if(cntxtMediaType == "Video"){
				//Debug.Log("Execute Video GUI Instant");
				instContextMedia.instantXmlVid(cntxtMediaName, cntxtMediaLocation); 
			} else if(cntxtMediaType == "Audio"){
				//Debug.Log("Execute Audio GUI Instant");
				instContextMedia.instantXmlAud(cntxtMediaName, cntxtMediaLocation);
			}
		}
	}
}
