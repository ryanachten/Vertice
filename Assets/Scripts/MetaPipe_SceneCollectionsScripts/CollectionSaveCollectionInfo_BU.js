#pragma strict

import System.Xml;

var collectionCont : ObjCollectionControl;
var collectionTitle : Text;


function Start()
{
	if (collectionCont == null)
	{
		Debug.Log("collectionCont == null - finding");
		GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjCollectionControl>();
	}
}

function SaveColllectionInfo()
{

	if (collectionCont == null)
	{
		Debug.Log("collectionCont == null - finding");
		GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjCollectionControl>();
	}
	
	var root : XmlNode = collectionCont.root;
	
	var collectDoc : XmlDocument = collectionCont.collectionDoc;
	var curCollectionNode : XmlNode = collectionCont.curCollectionListNode;
	try {
			Debug.Log("SaveCollectName: " + curCollectionNode.SelectSingleNode("@name").Value);	
		}
	catch(err)
		{
			Debug.Log("curCollectionNode == null - finding"); //here the issue remains
			var collectionName = GameObject.Find("Collection Information Panel").GetComponent.<CollectionGUIcontrol>().collectionTitle.text;
			collectionCont.curCollectionListNode = collectionCont.root.SelectSingleNode("MetaPipeCollection[@name='"+ collectionName +"']");
			curCollectionNode = collectionCont.curCollectionListNode;
			
			Debug.Log("SaveCollectName: " + curCollectionNode.SelectSingleNode("@name").Value);	
		}
			
	
	var curActiveModels : GameObject[] = GameObject.FindGameObjectsWithTag("Active Model");
	
	for ( model in curActiveModels)
	{
		var curObj : GameObject = model as GameObject;
		var curObjName = curObj.name;
		

		var curObjNode : XmlNode = curCollectionNode.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']"); 

		
		var curObjPos : Vector3 = curObj.transform.position;
		var posNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjPosition");
			posNode.SelectSingleNode("./ObjPosX").InnerText = curObjPos.x.ToString();
			posNode.SelectSingleNode("./ObjPosY").InnerText = curObjPos.y.ToString();
			posNode.SelectSingleNode("./ObjPosZ").InnerText = curObjPos.z.ToString();
		
		var curObjRotation : Quaternion = curObj.transform.rotation;
		var rotateNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjRotation");
			rotateNode.SelectSingleNode("./ObjRotateX").InnerText = curObjRotation.x.ToString();
			rotateNode.SelectSingleNode("./ObjRotateY").InnerText = curObjRotation.y.ToString();
			rotateNode.SelectSingleNode("./ObjRotateZ").InnerText = curObjRotation.z.ToString();
		
		var curObjScale : Vector3 = curObj.transform.localScale;
		var scaleNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjScale");
			scaleNode.SelectSingleNode("./ObjScaleX").InnerText = curObjScale.x.ToString();
			scaleNode.SelectSingleNode("./ObjScaleY").InnerText = curObjScale.y.ToString();
			scaleNode.SelectSingleNode("./ObjScaleZ").InnerText = curObjScale.z.ToString();
			
		collectDoc.Save(Application.dataPath + "/Metapipe_UserCollections.xml");
	}
	
	curCollectionNode.SelectSingleNode("@name").Value = collectionTitle.text;
	
	collectionCont.curCollectionListNode = curCollectionNode;
}