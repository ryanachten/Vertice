#pragma strict

//function used to sort imported objs by their sort field and position them in groups

import System.Linq;
import System.Xml;
import System.Collections.Generic;


var infoCont : ObjInfoControl;

//xml declarations
var xmlRoot : XmlNode;

//gui dependancies
var photogramLocationToggle : Toggle;
var floorPlane : GameObject; //floorplane objects sit on

//transform declarations
/*@HideInInspector
public var prevOrigin : Vector3;
private var newOrigin : Vector3;
@HideInInspector
public var prevObjDepth : float;
//private var curObjDepth : float;
public var objSpaceGap : float;*/

var curPosObjects : List.<GameObject>;
var groupingsOrigin = Dictionary.<String, float>();



function sortMode(curBrowseObjects : List.<GameObject>) //determines what sort of sorting is requried
{
	curPosObjects = curBrowseObjects;

	if (photogramLocationToggle.isOn)
	{
		getPhotogramLocations(curPosObjects);	
	}	
}


//Photogram etc might need their own script
function getPhotogramLocations (curPosObjects : List.<GameObject>) //used to determine number of locations in browse results
{
	xmlRoot = infoCont.control.root;

	var curObjName : String;

	var objectPhotogramLocations : List.<String>  = new List.<String>(); //will be used to count up number of locations in browse results
	
	var objectLocationsDict = new Dictionary.<GameObject, String>();
	
	Debug.Log("Positioning now");
	for (var object : Object in curPosObjects)
	{
	
		var curObject = object as GameObject;
		curObjName = curObject.name;
		Debug.Log("Positioning: " + curObjName);
		
		var curObjNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
		var curLocationName = curObjNode.SelectSingleNode("./ModelInfo/PhotogramInfo/PhotogramLocation/PhotogramLocationName").InnerText;
		Debug.Log(curObjName + " location: " + curLocationName);	
		
		objectLocationsDict.Add(curObject, curLocationName);
		
		if (!objectPhotogramLocations.Contains(curLocationName)) //will only add location name to list if it doesn't already exist
		{
			objectPhotogramLocations.Add(curLocationName);
		}
	}
	
	//sort the photogram groups reverse alphabetically to match origin calculations
	objectPhotogramLocations = objectPhotogramLocations.OrderByDescending(function(curObjName) curObjName).ToList();
	
	var photogramLocationCount : int = objectPhotogramLocations.Count;
	Debug.Log("Total number of locations: " + photogramLocationCount);
	
	findXOrigin(photogramLocationCount, objectPhotogramLocations); //find floor allocations for the individual groupings
	
	positionObjects(objectLocationsDict); //move the objects into position
}



function findXOrigin(numberOfGroupings : int, groupings : List.<String>)
{
	var floorPlaneCol = floorPlane.GetComponent(BoxCollider);	
	
	var floorSizeX = floorPlaneCol.bounds.size.x;
	var floorOriginX = 0 - (floorSizeX /2); //note that this only works with the plane's centre @ 0,0,0
	
	Debug.Log("floorSizeX: " + floorSizeX);
	Debug.Log("floorOriginX: " + floorOriginX);
	
	var floorSubdivisionSize = floorSizeX / numberOfGroupings;
	Debug.Log("floorSubdivisionSize: " + floorSubdivisionSize);
	
	groupingsOrigin = new Dictionary.<String, float>();
	
	for (var group : String in groupings)
	{	
		var subDivLength = floorOriginX + floorSubdivisionSize;
		floorOriginX += floorSubdivisionSize; //removes accounted for section from the overall size
		var subDivXOrigin = subDivLength - (floorSubdivisionSize /2);
		groupingsOrigin.Add(group, subDivXOrigin);
	}
	
	//Accessing values example
	for (var group : KeyValuePair.<String, float> in groupingsOrigin)
	{
		var groupOrigin = groupingsOrigin.Values;
		Debug.Log("Dictionary entry= " + " Group Name: " +group.Key + " " + " Group X Origin: " + group.Value);
	
	}
}



function positionObjects(objectSortDict : Dictionary.<GameObject, String>)
{
	//assigns rb/coll components and moves objects into position

	for (var browseObj : KeyValuePair.<GameObject, String> in objectSortDict)
	{
		var curObj : GameObject = browseObj.Key;
		var curSortField : String = browseObj.Value;
		Debug.Log("Dictionary entry= " + " Object Name: " + curObj.name + " " + " Location: " + curSortField);
		
		var curObjXPos = groupingsOrigin[curSortField]; //value of objSort dict matches the key of the groupingsOrigin dict
		var curObjXPosDebug : String = curObjXPos.ToString();
	
		curObj.transform.position.x = curObjXPos;
	}

}




/* //Old Code
	
	//from Start function
	
	prevOrigin = Vector3(0,0,0);
	prevObjDepth = 0;

	********

	//from ImportList function

	//iterates through list, finds node and times import seq
	prevOrigin = Vector3(0,0,0);
	prevObjDepth = 0;
	
	********
	
	//from ImportObj function
	
	//Add RigidBody
		var rb = curObj.AddComponent(Rigidbody); //add rigibody to currentModel to use physics and raycasting
		rb.useGravity = false; //make mass 0 to stop it falling (doesn't work for some reason)
		rb.isKinematic = true; //make mass 0 to stop it falling
		
	//Use BoxCol to find placement
		var curObjBoxCol = curObj.AddComponent(BoxCollider);	//add box collider to find scale etc **REVISE**
		
	moveToPosition(curObj);
	
	rb.useGravity = true; //make mass 0 to stop it falling (doesn't work for some reason)
	rb.isKinematic = false;
	
	********
	
	function moveToPosition(curObj : GameObject)
{
	//used to move the imported object to correct position
	var curObjBoxCol = curObj.GetComponent(BoxCollider);
	var curObjDepth = curObjBoxCol.size.z;
	
	newOrigin.z = prevOrigin.z + prevObjDepth/2 + objSpaceGap + curObjDepth/2;
	
	curObj.transform.position = newOrigin;
	
	prevObjDepth = curObjDepth;
	prevOrigin = newOrigin;

}

	
*/
