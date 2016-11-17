#pragma strict

//Responsible for saving gameObj data
//singleton + binary serialisation
//to add XML saving functionality


import System.Runtime.Serialization.Formatters.Binary;
import System.IO;
import System.Xml;
import System.Xml.XmlWriter;


//Class Reference
public static var control : ObjInfoControl; //this will be used to make sure there is only one present in scene
public var instContextMedia : InstantContextMedia;
public var impContextImg : ImpContextImg;
public var impObj : ImportObj;

//XML Declarations
public var doc : XmlDocument; 
public var root : XmlNode;
public var nodeList : XmlNodeList;
public var cntxtMediaNodes : XmlNodeList;
private var curObjNode : XmlNode;

//XML Variables
@HideInInspector
public var objName : String; //imported object name
@HideInInspector
public var fileName : String; //original file name
@HideInInspector
public var meshLocation : String;
@HideInInspector
public var texLocation : String;
@HideInInspector
public var objDescript : String; //user defined description of object
@HideInInspector
public var contribUsr : String; //contributer of file
@HideInInspector
public var contribDate : String; //date contributed (auto)

@HideInInspector
public var modelCreatorName : String;
@HideInInspector
public var modelCreateDate : String;
@HideInInspector
public var modelCreateType : String;
public var modelScale : String; // used to change artifact local scale in scenes

@HideInInspector
public var photogramLocationName : String;
public var designCreateField : String;

@HideInInspector
public var cntxtMediaName : String;
@HideInInspector
public var cntxtMediaType : String;
@HideInInspector
public var cntxtMediaLocation : String;
@HideInInspector
public var initialObjName : String; //initial name of object chached on load

@HideInInspector
public var fromImport : boolean; //used for auto tex import 
//@HideInInspector
public var isImpBrowse : boolean;
//@HideInInspector
public var isBrowseEdit : boolean; //used in LoadCurObjEdit
@HideInInspector
public var curObjPrevPos : Vector3;
@HideInInspector
public var browseActiveModels : List.<GameObject>; 
@HideInInspector
public var prevCollection : XmlNode; // used for collection -> import -> collection nav


function Awake () {
	//Singleton design
	if (control == null){
		DontDestroyOnLoad(gameObject);
		control = this; 
	} 
	else if ( control != this){
		Destroy(gameObject);
	}
}


function Start(){
	//Automatically loads XML doc for save etc
	doc = new XmlDocument();

	#if UNITY_WEBGL
	Debug.Log("Loading ObjArchive XML data from AWS");
	var url = null;
	var xmlWWW = new WWW(url);
	while(!xmlWWW.isDone){
		yield;
	}
	doc.LoadXml(xmlWWW.text);

	#else
	doc.Load(Application.dataPath + "/Metapipe_ObjArchive.xml");
	#endif


	root = doc.DocumentElement;
	
	nodeList = root.SelectNodes("MetaPipeObject");
	//Debug.Log("Number of MetaPipe Objects: " + nodeList.Count); //returns total number of MPObjs
	fromImport = false;
	
	if (Application.loadedLevel == 1) //import scene only
	{ 
		if (impObj == null)
		{
			impObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent.<ImportObj>();
		}
	} 
}



public class ObjectData {
	//holds object information
	//this class is used in GUI text control and XML output

	public var objName : String; 	
	public var fileName : String;
	public var meshLocation : String;
	public var texLocation : String;
	public var objDescript : String;
	public var contribUsr : String;
	public var contribDate : String;
	public var modelCreatorName : String;
	public var modelCreateDate : String;
	public var modelCreateType : String;
	public var modelScale : String;
	
	public var photogramLocationName : String;
	public var designCreateField : String;

	public var cntxtMediaNodes : XmlNodeList;
	public var cntxtMediaName : String;
	public var cntxtMediaType : String;
	public var cntxtMediaLocation : String;
	
	public var isImpBrowse : boolean;
	public var isBrowseEdit : boolean;
	public var curObjPrevPos : Vector3;
	public var browseActiveModels : List.<GameObject>; //used to store browse search when editing info
	
	public var prevCollection : XmlNode; // used for collection -> import -> collection nav
}


public function ChangeFolderLocation( newSplitFolderDirect : String, newFullFolderDirect : String) //***NEW*** used to change archive folder location in XML media paths
{
	var metaPipeObjects : XmlNodeList = root.SelectNodes("MetaPipeObject");
	Debug.Log("***Changing XML Folder Locations***");
	Debug.Log("Number of MetaPipe Objects: " + metaPipeObjects.Count);

	var verticePath : String = "/VerticeArchive/"; //used to identify substring index
	
	for (var i = 0; i < metaPipeObjects.Count; i++)
//	for (var i = 0; i < 2; i++) //Debug test
	{	
		Debug.Log("");
		Debug.Log("CHANGE OBJ: " + i);
			
		var oldMeshLocation = metaPipeObjects[i].SelectSingleNode("MeshLocation").InnerText;
		Debug.Log("oldMeshLocation: " + oldMeshLocation);
		
			var meshDirectSplitIndex = oldMeshLocation.IndexOf(verticePath);
//			Debug.Log("meshDirectSplitIndex: " + meshDirectSplitIndex);
			
			var meshOldDirect = oldMeshLocation.Substring(0, meshDirectSplitIndex); //Substring(0, pathSplitIndex)
//			Debug.Log("meshOldDirect: " + meshOldDirect);
		
			var newMeshDirect = oldMeshLocation.Replace(meshOldDirect, newSplitFolderDirect);
			Debug.Log("newMeshDirect: " + newMeshDirect);
			
			metaPipeObjects[i].SelectSingleNode("MeshLocation").InnerText = newMeshDirect;
			
		var oldTexLocation = metaPipeObjects[i].SelectSingleNode("TexLocation").InnerText;
		Debug.Log("oldTexLocation: " + oldTexLocation);	
		
			var texDirectSplitIndex = oldTexLocation.IndexOf(verticePath);
//			Debug.Log("texDirectSplitIndex: " + texDirectSplitIndex);
			
			var texOldDirect = oldTexLocation.Substring(0, texDirectSplitIndex); //Substring(0, pathSplitIndex)
//			Debug.Log("texOldDirect: " + texOldDirect);
		
			var newTexDirect = oldTexLocation.Replace(texOldDirect, newSplitFolderDirect);
			Debug.Log("newTexDirect: " + newTexDirect);
		
			metaPipeObjects[i].SelectSingleNode("TexLocation").InnerText = newTexDirect;		
		
		var contextMediaNode = metaPipeObjects[i].SelectSingleNode("ContextualInfo");
		var contextualMediaNodes : XmlNodeList = contextMediaNode.SelectNodes("ContextMedia");
		Debug.Log("contextualMediaNodes: " + contextualMediaNodes.Count);
		
		if (contextualMediaNodes.Count != 0)
		{
			for (var j = 0; j < contextualMediaNodes.Count; j++)
			{
				var oldMediaLocation = contextualMediaNodes[j].SelectSingleNode("MediaLocation").InnerText;
				Debug.Log("oldMediaLocation: " + oldMediaLocation);
				
					var mediaDirectSplitIndex = oldMediaLocation.IndexOf(verticePath);
//					Debug.Log("mediaDirectSplitIndex: " + mediaDirectSplitIndex);
					
					var mediaOldDirect = oldMediaLocation.Substring(0, mediaDirectSplitIndex); //Substring(0, pathSplitIndex)
//					Debug.Log("mediaOldDirect: " + mediaOldDirect);
				
					var newMediaDirect = oldMediaLocation.Replace(mediaOldDirect, newSplitFolderDirect);
					Debug.Log("newMediaDirect: " + newMediaDirect);
					
					contextualMediaNodes[j].SelectSingleNode("MediaLocation").InnerText = newMediaDirect;
			}
		}
		doc.Save(Application.dataPath + "/Metapipe_ObjArchive.xml");
	}
	
	Debug.Log("***/Changing XML Folder Locations***");
}



public function Load(){ //this could be used in OnEnable for autoload
	
	if(root == null) //used in browse -> imp edit; not sure why this is needed but solves nullref
	{
		Debug.Log("Root lost");
		doc = new XmlDocument();
		doc.Load(Application.dataPath + "/Metapipe_ObjArchive.xml");
		root = doc.DocumentElement;
	}
	
	var existNodeCheck = root.SelectNodes("MetaPipeObject");
	//Debug.Log("existNodeCheck.Count: " + existNodeCheck.Count);
	for (var i = 0; i < existNodeCheck.Count; i++){
		var fileCheckNode = existNodeCheck[i].SelectSingleNode("FileName");
		if (fileCheckNode.InnerText == control.fileName){ //changed to use the *control* fileName vs the public var fileName
			//Debug.Log("fileCheckNode.InnerText: " + fileCheckNode.InnerText);
			//Debug.Log("fileName: " + control.fileName);
			curObjNode = fileCheckNode.ParentNode;
			
			if (control == null)
			{
				Debug.Log("control NULL");
			}
			break;
		} else {
			curObjNode = null;
		}
	}
	
	if (curObjNode == null){//if obj name isn't present on the XML file
	
		Debug.Log("No XML Node that matches CurObj name");
		Debug.Log("Creating new node...");
		CreateNewNode();
								
	} else { //if obj name is present on the XML file
		
		//***Obj Information Data***
		objName = curObjNode.SelectSingleNode("@name").Value;
		Debug.Log("objName: " + objName);		
		fileName = curObjNode.SelectSingleNode("FileName").InnerText;
		Debug.Log("fileName: " + fileName);		
		texLocation = curObjNode.SelectSingleNode("TexLocation").InnerText;
		Debug.Log("texLocation: " + texLocation);	
			if(texLocation.Length > 3 && fromImport){
				impObj.TexImport();
			}
		contribUsr = curObjNode.SelectSingleNode("ContribName").InnerText; 
		contribDate = curObjNode.SelectSingleNode("ContribDate").InnerText;
		objDescript = curObjNode.SelectSingleNode("Description").InnerText;
		
		//***Model Information Data***
		var modelInfoNode = curObjNode.SelectSingleNode("ModelInfo");
			modelCreatorName = modelInfoNode.SelectSingleNode("ModelCreator").InnerText;
			modelCreateDate = modelInfoNode.SelectSingleNode("ModelCreateDate").InnerText;
			modelCreateType = modelInfoNode.SelectSingleNode("ModelCreateType").InnerText;
			modelScale = modelInfoNode.SelectSingleNode("ModelScale").InnerText; 	
			
			//***Photogram Information Data***
			if (modelCreateType == "Photogrammetric"){				
				var photogramInfoNode = modelInfoNode.SelectSingleNode("PhotogramInfo");
				//Photogram Location Data
				try {
					var photogramLocationNode = photogramInfoNode.SelectSingleNode("PhotogramLocation"); //attempts to select this photogram node
				} catch (err){ //if the photogram node doesn't exist
					CreatePhotogramNode(); // create it
				}
				if (photogramLocationNode != null)
				{
					photogramLocationName = photogramLocationNode.SelectSingleNode("PhotogramLocationName").InnerText;
				} 
				else if (photogramLocationNode == null)
				{
					photogramInfoNode = modelInfoNode.SelectSingleNode("PhotogramInfo");
					photogramLocationNode = photogramInfoNode.SelectSingleNode("PhotogramLocation");
					photogramLocationName = photogramLocationNode.SelectSingleNode("PhotogramLocationName").InnerText;
				}
			}
			//***Design Information Data*** 
			else if (modelCreateType == "Design")
			{
				var designInfoNode = modelInfoNode.SelectSingleNode("DesignInfo");
				//Design Create Data
				try {
					var designCreationNode = designInfoNode.SelectSingleNode("DesignCreation");
				} catch (err){ 
					Debug.Log("Node doesn't exist!");
					CreateDesignNode(); // create it
				}
				if (designCreationNode != null)
				{
					designCreateField = designCreationNode.SelectSingleNode("DesignCreationField").InnerText;
					Debug.Log("DesignCreationField = " + designCreateField);
				}
				else if (designCreationNode == null)
				{
					designInfoNode = modelInfoNode.SelectSingleNode("DesignInfo");
					designCreationNode = designInfoNode.SelectSingleNode("DesignCreation");
					designCreateField = designCreationNode.SelectSingleNode("DesignCreationField").InnerText;
				} 
			}
		
		//***Contextual Media Data***
		cntxtMediaNodes = curObjNode.SelectSingleNode("ContextualInfo").ChildNodes;
		fromImport = false;
		Debug.Log("LOADED existing node: " + objName);
	}
}




public function Save(){ //this could be changed to OnDisable for autosave

	//XML Method 02
	//object: change object data and save to xml
	
	var nodeName = curObjNode.SelectSingleNode("@name").Value;
	curObjNode = root.SelectSingleNode("MetaPipeObject[@name='"+ nodeName +"']");
	curObjNode.SelectSingleNode("@name").Value = objName;
	
	curObjNode.SelectSingleNode("FileName").InnerText = fileName;
	curObjNode.SelectSingleNode("TexLocation").InnerText = texLocation;
	curObjNode.SelectSingleNode("ContribName").InnerText = contribUsr;
	curObjNode.SelectSingleNode("Description").InnerText = objDescript;
	
	var modelInfoNode = curObjNode.SelectSingleNode("ModelInfo");
		modelInfoNode.SelectSingleNode("ModelCreator").InnerText = modelCreatorName;
		modelInfoNode.SelectSingleNode("ModelCreateDate").InnerText = modelCreateDate;
		modelInfoNode.SelectSingleNode("ModelCreateType").InnerText = modelCreateType;
		modelInfoNode.SelectSingleNode("ModelScale").InnerText = modelScale;
		
		//***Photogram Information Data***
		if (modelCreateType == "Photogrammetric"){
			var photogramInfoNode = modelInfoNode.SelectSingleNode("PhotogramInfo");
	
			try {
				var photogramLocationNode = photogramInfoNode.SelectSingleNode("PhotogramLocation"); //attempts to select this photogram node
			} catch (err){ //if the photogram node doesn't exist
				CreatePhotogramNode(); // create it
			}
			if (photogramLocationNode == null)
			{
				photogramInfoNode = modelInfoNode.SelectSingleNode("PhotogramInfo");
				photogramLocationNode = photogramInfoNode.SelectSingleNode("PhotogramLocation");
			}
			photogramLocationNode.SelectSingleNode("PhotogramLocationName").InnerText = photogramLocationName;		
		}
		//***Design Information Data***
		if (modelCreateType == "Design"){
			var designInfoNode = modelInfoNode.SelectSingleNode("DesignInfo");
			
			try {
				var designCreationNode = designInfoNode.SelectSingleNode("DesignCreation");
			} catch (err){ //if the photogram node doesn't exist
				CreateDesignNode();
			}
			if (designCreationNode == null)
			{
				designInfoNode = modelInfoNode.SelectSingleNode("DesignInfo");
				designCreationNode = designInfoNode.SelectSingleNode("DesignCreation");
			}
			designCreationNode.SelectSingleNode("DesignCreationField").InnerText = designCreateField;		
		}		
	
	doc.Save(Application.dataPath + "/Metapipe_ObjArchive.xml");
	
	Debug.Log("********TEST**********");
	Debug.Log("contribUsr: " + contribUsr);
	Debug.Log("ContribName.InnerText: " + curObjNode.SelectSingleNode("ContribName").InnerText);	
	if (doc == null)
		Debug.Log("doc == null");
	
	if (root == null)
		Debug.Log("root == null");
	nodeName = curObjNode.SelectSingleNode("@name").Value; 
	var nodeCheck = root.SelectSingleNode("MetaPipeObject[@name='"+ nodeName +"']");
	Debug.Log("ContribName.InnerText: " + nodeCheck.SelectSingleNode("ContribName").InnerText);
	Debug.Log("********/TEST**********");	
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
		//texLocationNode.InnerText = texLocation;
		texLocationNode.InnerText = "";
		newObjNode.AppendChild(texLocationNode);
		texLocation = texLocationNode.InnerText;			
	
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
			
		var modelScaleNode = doc.CreateElement("ModelScale");
			modelScaleNode.InnerText = "1";
			modelInfoNode.AppendChild(modelScaleNode);
			modelScale = modelScaleNode.InnerText;
		
	var contextualInfoNode = doc.CreateElement("ContextualInfo");
		newObjNode.AppendChild(contextualInfoNode);	
	
	root.InsertAfter(newObjNode, lastObjNode); //add to the bottom of the xml doc
	
	doc.Save(Application.dataPath + "/Metapipe_ObjArchive.xml");
	
	curObjNode = newObjNode;
	
	Debug.Log("CREATED new node: " + newObjNode.GetAttribute("name"));

}


public function CreateContextNode(mediaType : String){

	//called everytime contextual media is uploaded
	var nodeName = curObjNode.SelectSingleNode("@name").Value;
	Debug.Log("objName: " + objName); 
	curObjNode = root.SelectSingleNode("MetaPipeObject[@name='"+ nodeName +"']");
	
	var contextInfoNode = curObjNode.SelectSingleNode("ContextualInfo");
	
	var contextMedia = doc.CreateElement("ContextMedia");
	
	var firstContextNode = contextInfoNode.FirstChild; //inserts at top of list
	if (firstContextNode != null){
		Debug.Log("First context media name: " + firstContextNode.SelectSingleNode("MediaName").InnerText);
		contextInfoNode.InsertBefore(contextMedia, firstContextNode); //inserts at top of list
	} 
	else if (firstContextNode == null) //if no contextual media has been added and this is the first one
	{
		Debug.Log("firstContextNode == null");
		contextInfoNode.AppendChild(contextMedia);
	}
	
	
	
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

public function CreatePhotogramNode()
{
	Debug.Log("Creating photogram node");
	var mediaInfoNode = curObjNode.SelectSingleNode("ModelInfo");
	var modelCreateTypeNode = mediaInfoNode.SelectSingleNode("ModelCreateType");
	
	var photogramNode = doc.CreateElement("PhotogramInfo");
	
		var photogramLocationNode = doc.CreateElement("PhotogramLocation");
		photogramNode.AppendChild(photogramLocationNode);
		
			var photogramLocationNameNode = doc.CreateElement("PhotogramLocationName");
			photogramLocationNameNode.InnerText = "";
			photogramLocationNode.AppendChild(photogramLocationNameNode);
				
	mediaInfoNode.InsertAfter(photogramNode, modelCreateTypeNode);
	
	doc.Save(Application.dataPath + "/Metapipe_ObjArchive.xml");
}

public function CreateDesignNode()
{
	Debug.Log("Creating design node");
	var mediaInfoNode = curObjNode.SelectSingleNode("ModelInfo");
	var modelCreateTypeNode = mediaInfoNode.SelectSingleNode("ModelCreateType");
	
	var designNode = doc.CreateElement("DesignInfo");
	
		var designCreationNode = doc.CreateElement("DesignCreation");
		designNode.AppendChild(designCreationNode);
		
			var designCreationFieldNode = doc.CreateElement("DesignCreationField");
			designCreationFieldNode.InnerText = "";
			designCreationNode.AppendChild(designCreationFieldNode);
				
	mediaInfoNode.InsertAfter(designNode, modelCreateTypeNode);
	
	doc.Save(Application.dataPath + "/Metapipe_ObjArchive.xml");
}