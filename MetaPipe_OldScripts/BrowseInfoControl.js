#pragma strict

//Responsible for saving gameObj data
//singleton + binary serialisation
//to add XML saving functionality


import System.Runtime.Serialization.Formatters.Binary;
import System.IO;
import System.Xml;
import System.Xml.XmlWriter;


//Class Reference
public static var browseControl : BrowseInfoControl; //this will be used to make sure there is only one present in scene
public var instContextMedia : InstantContextMedia;
public var impContextImg : ImpContextImg;
public var contextualMediaPanel : GameObject;
public var impObj : ImportObj;
public var createType : ModelCreateTypeToggle;

//XML Declarations
public var doc : XmlDocument; 
public var root : XmlNode;
public var nodeList : XmlNodeList;
public var cntxtMediaNodes : XmlNodeList;
private var curObjNode : XmlNode;



//XML Variables
public var objName : String; //imported object name
public var fileName : String; //original file name
public var meshLocation : String;
public var texLocation : String;
public var objImgCap : String; //screencap of model from last save
public var objDescript : String; //user defined description of object
public var contribUsr : String; //contributer of file
public var contribDate : String; //date contributed (auto)
public var modelCreatorName : String;
public var modelCreateDate : String;
public var modelCreateType : String;
public var cntxtMediaName : String;
public var cntxtMediaType : String;
public var cntxtMediaLocation : String;

public var initialObjName : String; //initial name of object chached on load


	
function Awake () {
	//Singleton design
	if (browseControl == null){
		DontDestroyOnLoad(gameObject);
		browseControl = this; 
	} 
	else if ( browseControl != this){
		Destroy(gameObject);
	}
}



function Start(){
	//Automatically loads XML doc for save etc
	doc = new XmlDocument();
	doc.Load(Application.dataPath + "/objMetaTest01.xml");

	root = doc.DocumentElement;
	Debug.Log("XML Root: " + root.Name);
	
	nodeList = root.SelectNodes("MetaPipeObject");
	Debug.Log("***XML Log Begin ***");
	Debug.Log("Number of Objects: " + nodeList.Count); //returns total number of MPObjs
	
	for (var i = 0; i < nodeList.Count; i++)
	{
		var node = nodeList[i];
	}
	Debug.Log("***XML Log End ***");
}



public class BrowseObjectData {
	//holds object information
	//this class is used in GUI text control and XML output

	public var doc : XmlDocument;
	public var root : XmlNode;
	
	public var objName : String; 	
	public var fileName : String;
	public var meshLocation : String;
	public var texLocation : String;
	public var objImgCap : String;
	public var objDescript : String;
	public var contribUsr : String;
	public var contribDate : String;
	public var modelCreatorName : String;
	public var modelCreateDate : String;
	public var modelCreateType : String;

	public var cntxtMediaName : String;
	public var cntxtMediaType : String;
	public var cntxtMediaLocation : String;
	
}

/*

public function Load(){ //this could be used in OnEnable for autoload

	
	var existNodeCheck = root.SelectNodes("MetaPipeObject");
	for (var i = 0; i < existNodeCheck.Count; i++){
		var fileCheckNode = existNodeCheck[i].SelectSingleNode("FileName");
		if (fileCheckNode.InnerText == fileName){
			curObjNode = fileCheckNode.ParentNode;
		} else {
			curObjNode = null;
		}
	}
	
	//var curObjNode = root.SelectSingleNode("MetaPipeObject[@name='"+ objName +"']"); 
	
	
	
	if (curObjNode == null){//if obj name is present on the XML file
	
		Debug.Log("No XML Node that matches CurObj name");
		Debug.Log("Creating new node...");
		CreateNewNode();
								
	} else { //if obj name is present on the XML file
		
		//***Obj Information Data***
		objName = curObjNode.SelectSingleNode("@name").Value;		
		fileName = curObjNode.SelectSingleNode("FileName").InnerText;
		texLocation = curObjNode.SelectSingleNode("TexLocation").InnerText;
			if(texLocation.Length > 3){
				impObj.TexImport();	
			}
		objImgCap = curObjNode.SelectSingleNode("ObjImgCap").InnerText;
		contribUsr = curObjNode.SelectSingleNode("ContribName").InnerText; 
		contribDate = curObjNode.SelectSingleNode("ContribDate").InnerText;
		objDescript = curObjNode.SelectSingleNode("Description").InnerText;
		
		//***Model Information Data***
		var modelInfoNode = curObjNode.SelectSingleNode("ModelInfo");
			modelCreatorName = modelInfoNode.SelectSingleNode("ModelCreator").InnerText;
			modelCreateDate = modelInfoNode.SelectSingleNode("ModelCreateDate").InnerText;
			modelCreateType = modelInfoNode.SelectSingleNode("ModelCreateType").InnerText;
				createType.LoadType();
		
		//***Contextual Media Data***
		cntxtMediaNodes = curObjNode.SelectSingleNode("ContextualInfo").ChildNodes;
	
		Debug.Log("no. of contextual media items: " + cntxtMediaNodes.Count);
		
		LoadContextMedia();
		
		Debug.Log("LOADED existing node: " + objName);
	}
}


public function LoadContextMedia(){

	if (contextualMediaPanel.activeSelf){
		if (cntxtMediaNodes.Count > 0) //wont attempt to parse context media data if no nodes present
			{
				for (var i = 0; i < cntxtMediaNodes.Count; i++) //interate through context nodes
				{
					//For Each Media Node
					var curcntxtMediaNode = cntxtMediaNodes[i];
					//Get Temp Information
					cntxtMediaName = curcntxtMediaNode.SelectSingleNode("MediaName").InnerText;
						//Debug.Log("cntxtMediaName: " + cntxtMediaName);
					cntxtMediaType = curcntxtMediaNode.SelectSingleNode("MediaType").InnerText;
						//Debug.Log("cntxtMediaType: " + cntxtMediaType);
					cntxtMediaLocation = curcntxtMediaNode.SelectSingleNode("MediaLocation").InnerText;
						//Debug.Log("cntxtMediaLocation: " + cntxtMediaLocation);
					//Execute GUI instantiation based on mediaType
					if(cntxtMediaType == "Image"){
						//Debug.Log("Execute Image GUI Instant");
						instContextMedia.instantXmlImg();
					} else if(cntxtMediaType == "Video"){
						//Debug.Log("Execute Video GUI Instant");
						instContextMedia.instantXmlVid();
					} else if(cntxtMediaType == "Audio"){
						//Debug.Log("Execute Audio GUI Instant");
						instContextMedia.instantXmlAud();
					}
				}
			}
		}
}



public function Save(){ //this could be changed to OnDisable for autosave

	//XML Method 02
	//object: change object data and save to xml
		
	//var curObjNode = root.SelectSingleNode("MetaPipeObject[@name='"+ objName +"']");
	
	curObjNode.SelectSingleNode("FileName").InnerText = fileName;
	curObjNode.SelectSingleNode("MeshLocation").InnerText = meshLocation;
	curObjNode.SelectSingleNode("TexLocation").InnerText = texLocation;
	curObjNode.SelectSingleNode("ObjImgCap").InnerText = objImgCap;
	curObjNode.SelectSingleNode("ContribName").InnerText = contribUsr;
	curObjNode.SelectSingleNode("Description").InnerText = objDescript;
	
	var modelInfoNode = curObjNode.SelectSingleNode("ModelInfo");
		modelInfoNode.SelectSingleNode("ModelCreator").InnerText = modelCreatorName;
		modelInfoNode.SelectSingleNode("ModelCreateDate").InnerText = modelCreateDate;
		modelInfoNode.SelectSingleNode("ModelCreateType").InnerText = modelCreateType;
		
	doc.Save(Application.dataPath + "/objMetaTest01.xml");
		
}


public function CreateNewNode(){
	//Creates new node for new objects
	//this will currently copy contextual media too - change away from copy
	var lastObjNode = root.SelectSingleNode("MetaPipeObject[last()]"); //select last MP node to add the new one behind
	var newObjNode = doc.CreateElement("MetaPipeObject");
	newObjNode.SetAttribute("name", objName);
	
	var fileNameNode = doc.CreateElement("FileName");
		fileNameNode.InnerText = fileName;
		newObjNode.AppendChild(fileNameNode);
	
	var meshLocationNode = doc.CreateElement("MeshLocation");
		meshLocationNode.InnerText = meshLocation;
		newObjNode.AppendChild(meshLocationNode);
		
	var texLocationNode = doc.CreateElement("TexLocation");
		texLocationNode.InnerText = texLocation;
		newObjNode.AppendChild(texLocationNode);			
					
	var objImgCapNode = doc.CreateElement("ObjImgCap");
		objImgCapNode.InnerText = objImgCap;
		newObjNode.AppendChild(objImgCapNode);
	
	var contribUserNode = doc.CreateElement("ContribName");
		contribUserNode.InnerText = "Add Contributor Name";
		newObjNode.AppendChild(contribUserNode);
		contribUsr = contribUserNode.InnerText;
	
	var impDate = System.DateTime.Now.ToString("dd/MM/yyyy");
	var contribDateNode = doc.CreateElement("ContribDate");
		contribDateNode.InnerText = impDate;
		newObjNode.AppendChild(contribDateNode);
		contribDate = contribDateNode.InnerText;	
		
	var descriptionNode = doc.CreateElement("Description");
		descriptionNode.InnerText = "No Description Added Yet";
		newObjNode.AppendChild(descriptionNode);	
		objDescript = descriptionNode.InnerText;
	
	var modelInfoNode = doc.CreateElement("ModelInfo");
		newObjNode.AppendChild(modelInfoNode);
		
		var modelCreatorNode = doc.CreateElement("ModelCreator");
			modelCreatorNode.InnerText = "No Model Creator Added Yet";
			modelInfoNode.AppendChild(modelCreatorNode);	
			modelCreatorName = modelCreatorNode.InnerText;
			
		var modelCreateDateNode = doc.CreateElement("ModelCreateDate");
			modelCreateDateNode.InnerText = "No Date Added Yet";
			modelInfoNode.AppendChild(modelCreateDateNode);
			modelCreateDate = modelCreateDateNode.InnerText;
			
		var modelCreateTypeNode = doc.CreateElement("ModelCreateType");
			modelCreateTypeNode.InnerText = "";
			modelInfoNode.AppendChild(modelCreateTypeNode);
			modelCreateType = modelCreateTypeNode.InnerText;
		
	var contextualInfoNode = doc.CreateElement("ContextualInfo");
		newObjNode.AppendChild(contextualInfoNode);	
	
	root.InsertAfter(newObjNode, lastObjNode); //add to the bottom of the xml doc
	
	doc.Save(Application.dataPath + "/objMetaTest01.xml");
	
	curObjNode = newObjNode;
	
	Debug.Log("CREATED new node: " + newObjNode.GetAttribute("name"));

}


public function RenameNode(){

	var curNameAttr = curObjNode.SelectSingleNode("@name");
	var oldName = curNameAttr.Value;
	curNameAttr.Value = objName;
	var newName = curNameAttr.Value;
	
	Debug.Log("RENAMED old node: " + oldName + " to new node: " + newName);
}


public function CreateContextNode(mediaType : String){

	//called everytime and image is successfully uploaded
	//var curObjNode = root.SelectSingleNode("MetaPipeObject[@name='"+ objName +"']");
	var contextInfoNode = curObjNode.SelectSingleNode("ContextualInfo");
	
	var contextMedia = doc.CreateElement("ContextMedia");
		contextInfoNode.AppendChild(contextMedia);
	
	var mediaNameNode = doc.CreateElement("MediaName");
		mediaNameNode.InnerText = cntxtMediaName;
		contextMedia.AppendChild(mediaNameNode);
		
	var mediaTypeNode = doc.CreateElement("MediaType");
		if(mediaType == "Image"){
			mediaTypeNode.InnerText = "Image";
		} else if (mediaType == "Video"){
			mediaTypeNode.InnerText = "Video";
		}else if (mediaType == "Audio"){
			mediaTypeNode.InnerText = "Audio";
		}
		contextMedia.AppendChild(mediaTypeNode);
		
	var mediaLocationNode = doc.CreateElement("MediaLocation");
		mediaLocationNode.InnerText = cntxtMediaLocation;
		contextMedia.AppendChild(mediaLocationNode);
				
}

*/