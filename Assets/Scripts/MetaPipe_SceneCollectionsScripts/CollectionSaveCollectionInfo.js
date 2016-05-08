#pragma strict

import System.Xml;

var collectionCont : ObjCollectionControl;

var collectDoc : XmlDocument;
var curCollectionNode : XmlNode;

var collectionTitle : Text;
var collectionCreatorText : Text;
var collectionDescriptionText : Text;

var saveCollectFeedback : FeedbackScript; //***NEW***


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
	
	collectDoc = collectionCont.collectionDoc;
	curCollectionNode = collectionCont.curCollectionListNode;
	
	SavePositionInfo();
	SaveGuiInfo();
	
	saveCollectFeedback.Feedback();
}

function SavePositionInfo()
{
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
	}
}

	
function SaveGuiInfo()
{
	
	collectDoc = collectionCont.collectionDoc;
	curCollectionNode = collectionCont.curCollectionListNode;
	
	Debug.Log(curCollectionNode.SelectSingleNode("@name").Value + " -> " + collectionTitle.text);
	
	curCollectionNode.SelectSingleNode("@name").Value = collectionTitle.text;
	
	curCollectionNode.SelectSingleNode("./CollectionInfo/CollectionCreator").InnerText = collectionCreatorText.text;
	
	curCollectionNode.SelectSingleNode("./CollectionInfo/CollectionDescription").InnerText = collectionDescriptionText.text;
	
	collectDoc.Save(Application.dataPath + "/Metapipe_UserCollections.xml");
}	
			