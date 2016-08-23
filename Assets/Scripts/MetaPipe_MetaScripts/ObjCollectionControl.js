#pragma strict

import System.Runtime.Serialization.Formatters.Binary;
import System.IO;
import System.Xml;
import System.Xml.XmlWriter;

//XML Declarations
public var collectionDoc : XmlDocument; 
public var root : XmlNode;
static public var collectionList : XmlNodeList;
private var curObjNode : XmlNode;

static var curCollectionListNode : XmlNode; //used for GUI content set via buttons / other col import methods

function Start(){
	//Automatically loads XML doc for save etc
	collectionDoc = new XmlDocument();



	#if UNITY_WEBGL
	Debug.Log("Loading UserCollections XML data from AWS");
	var url = Paths.Remote + "/Metadata/Metapipe_UserCollections.xml";
	var xmlWWW = new WWW(url);

	// Note that there is a race condition here, so it isn't a good idea to yield. If this code yields in the while 
	// loop, then the class that loads the list of "Collections Available" (c.f. Collection UI) has no data to work 
	// with and will throw an exception
	while(!xmlWWW.isDone){
		yield;
	}
	collectionDoc.LoadXml(xmlWWW.text);
	#else
	collectionDoc.Load(Application.dataPath + "/Metapipe_UserCollections.xml");
	#endif

	root = collectionDoc.DocumentElement;

	yield;
	collectionList = root.SelectNodes("MetaPipeCollection");
	//Debug.Log("Number of MetaPipe Collections: " + collectionList.Count); //returns total number of MPObjs
}

function addObjToCollection (collectionNode : XmlNode, objName : String)
{
	var listName = collectionNode.SelectSingleNode("@name").Value;	

	Debug.Log("Adding " + objName + " to " + listName);
		
	var curObjNode = collectionDoc.CreateElement("MetaPipeObject");
		curObjNode.SetAttribute("name", objName);
		
		var objTransNode = collectionDoc.CreateElement("ObjTransformInfo");
			curObjNode.AppendChild(objTransNode);
			
			var objPosNode = collectionDoc.CreateElement("ObjPosition");
				objTransNode.AppendChild(objPosNode);
					
				var objPosXNode = collectionDoc.CreateElement("ObjPosX");
				objPosNode.AppendChild(objPosXNode);
				
				var objPosYNode = collectionDoc.CreateElement("ObjPosY");
				objPosNode.AppendChild(objPosYNode);
				
				var objPosZNode = collectionDoc.CreateElement("ObjPosZ");
				objPosNode.AppendChild(objPosZNode);
				
			var objRotateNode = collectionDoc.CreateElement("ObjRotation");
				objTransNode.AppendChild(objRotateNode);
				
				var objRotateXNode = collectionDoc.CreateElement("ObjRotateX");
				objRotateNode.AppendChild(objRotateXNode);
				
				var objRotateYNode = collectionDoc.CreateElement("ObjRotateY");
				objRotateNode.AppendChild(objRotateYNode);
				
				var objRotateZNode = collectionDoc.CreateElement("ObjRotateZ");
				objRotateNode.AppendChild(objRotateZNode);
				
			var objScaleNode = collectionDoc.CreateElement("ObjScale");
				objTransNode.AppendChild(objScaleNode);
				
				var objScaleXNode = collectionDoc.CreateElement("ObjScaleX");
				objScaleNode.AppendChild(objScaleXNode);
				
				var objScaleYNode = collectionDoc.CreateElement("ObjScaleY");
				objScaleNode.AppendChild(objScaleYNode);
				
				var objScaleZNode = collectionDoc.CreateElement("ObjScaleZ");
				objScaleNode.AppendChild(objScaleZNode);		
				
			var lastObjNode : XmlNode;	
			try {
				var existObjNodes = collectionNode.SelectNodes("MetaPipeObject");
				if (existObjNodes.Count > 0)
				{
					lastObjNode = collectionNode.SelectSingleNode("MetaPipeObject[last()]");	
				}
				else 
				{
					lastObjNode = collectionNode.SelectSingleNode("CollectionInfo");
				}
			}
			catch(err)
			{
					lastObjNode = collectionNode.SelectSingleNode("CollectionInfo");
			}						
																																																																		
			collectionNode.InsertAfter(curObjNode, lastObjNode);	
			
			collectionDoc.Save(Application.dataPath + "/Metapipe_UserCollections.xml");				
}


function addNewCollection(collectionName : String)
{
	Debug.Log("New Collection Name: " + collectionName);
	
	if (collectionDoc == null)
	{
		Debug.Log("collectionDoc == null - reStart()ing");
		Start();
	}

	var newCollectionNode = collectionDoc.CreateElement("MetaPipeCollection");
		newCollectionNode.SetAttribute("name", collectionName);
		
		var collectionInfoNode = collectionDoc.CreateElement("CollectionInfo");
		newCollectionNode.AppendChild(collectionInfoNode);
		
			var collectionCreatorNode = collectionDoc.CreateElement("CollectionCreator");
			collectionInfoNode.InnerText = "";
			collectionInfoNode.AppendChild(collectionCreatorNode);
			
			var collectionDateNode = collectionDoc.CreateElement("CollectionDate");
			collectionDateNode.InnerText = System.DateTime.Now.ToString("dd/MM/yyyy");
			collectionInfoNode.AppendChild(collectionDateNode);	
			
			var collectionDescriptNode = collectionDoc.CreateElement("CollectionDescription");
			collectionDescriptNode.InnerText = "";
			collectionInfoNode.AppendChild(collectionDescriptNode);
								
//	var lastCollectionNode = root.SelectSingleNode("MetaPipeCollection[last()]");	
//	root.InsertAfter(newCollectionNode, lastCollectionNode);

	var firstCollectionNode = root.FirstChild; //***NEW***
		Debug.Log("FirstCollect name: " + firstCollectionNode.SelectSingleNode("@name").Value);
	root.InsertBefore(newCollectionNode, firstCollectionNode);
		
	
	collectionDoc.Save(Application.dataPath + "/Metapipe_UserCollections.xml");	
	
	collectionList = root.SelectNodes("MetaPipeCollection");
}