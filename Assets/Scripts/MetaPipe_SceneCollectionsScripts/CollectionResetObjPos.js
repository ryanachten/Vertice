#pragma strict

import System.Xml;


var collectionCont : ObjCollectionControl;
var curCollectionNode : XmlNode;
var modObjScript : CollectModifyObjGUI;

@HideInInspector
var curObjName : String;
@HideInInspector
var curObj : GameObject;


function resetCurObjPos()
{
	//Get Cur List
	var curCollectionNode : XmlNode = collectionCont.curCollectionListNode;
	
	//Get Cur Obj + name
	curObj = modObjScript.curObj;
	curObjName = curObj.name;
	
	//Select CurObjNode
	var curObjNode = curCollectionNode.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");

	//Get Info
	var curObjPos : Vector3 = new Vector3();
	var existPosNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjPosition");
		var existPosX = existPosNode.SelectSingleNode("./ObjPosX").InnerText;
		var existPosY = existPosNode.SelectSingleNode("./ObjPosY").InnerText;
		var existPosZ = existPosNode.SelectSingleNode("./ObjPosZ").InnerText;

	var existRotateNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjRotation");				
		var existRotateX = existRotateNode.SelectSingleNode("./ObjRotateX").InnerText;
		var existRotateY = existRotateNode.SelectSingleNode("./ObjRotateY").InnerText;
		var existRotateZ = existRotateNode.SelectSingleNode("./ObjRotateZ").InnerText;
		
	var existScaleNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjScale");				
		var existScaleX = existScaleNode.SelectSingleNode("./ObjScaleX").InnerText;
		var existScaleY = existScaleNode.SelectSingleNode("./ObjScaleY").InnerText;
		var existScaleZ = existScaleNode.SelectSingleNode("./ObjScaleZ").InnerText;

	if (existPosX == "" && existPosY == "" && existPosY == "")
	{
		Debug.Log("No pos information stored for reset");
	
	}
	else
	{
		Debug.Log("Resetting to prev pos");
		
		curObjPos.x = parseFloat(existPosX);
		curObjPos.z = parseFloat(existPosZ);
		curObj.transform.position = curObjPos;
		
		var curObjRotate : Quaternion = new Quaternion();
		curObjRotate.x = parseFloat(existRotateX);
		curObjRotate.y = parseFloat(existRotateY);
		curObjRotate.z = parseFloat(existRotateZ);
		curObj.transform.rotation = curObjRotate;
		
		var curObjScale : Vector3 = new Vector3();
		curObjScale.x = parseFloat(existScaleX);
		curObjScale.y = parseFloat(existScaleY);
		curObjScale.z = parseFloat(existScaleZ);
		curObj.transform.localScale = curObjScale;
		
		modObjScript.UpdateGUI();
	}

}