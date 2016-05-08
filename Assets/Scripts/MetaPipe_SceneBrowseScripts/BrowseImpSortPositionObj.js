#pragma strict

//function used to sort imported objs by their sort field and position them in groups

import System.Linq;
import System.Xml;
import System.Collections.Generic;


var infoCont : ObjInfoControl;
var browseObjTypeXmlScript : BrowseObjTypeXml;

//xml declarations
var xmlRoot : XmlNode;

//gui dependancies
var photogramLocationToggle : Toggle;
var designFieldToggle : Toggle;
var loadPlane : GameObject; //floorplane objects sit on

var particleLocate : GameObject;

//transform declarations
var objZOriginStartPos : float = 10;
var objSpaceGap : float = 10;

var curPosObjects : List.<GameObject>;
var groupingsOrigin = Dictionary.<GameObject, Vector3>();



function sortMode(curBrowseObjects : List.<GameObject>) //determines what sort of sorting is requried
{
	var browseType : String = browseObjTypeXmlScript.browseObjType;
	
	curPosObjects = new List.<GameObject>();
	curPosObjects = curBrowseObjects;

	if (photogramLocationToggle.isOn && browseType == "Photogrammetric")
	{
		getPhotogramLocations(curPosObjects);	
	}
	else if (designFieldToggle.isOn && browseType == "Design")
	{
		getDesignFields(curPosObjects);
	}
	else if (browseType == "Search")
	{
		getSearchTypes(curPosObjects);
	}	
}


//Photogram etc will need their own script when this gets bigger
function getPhotogramLocations (curPosObjects : List.<GameObject>) //used to determine number of locations in browse results
{
	xmlRoot = infoCont.control.root;

	var curObjName : String;

	var photogramLocations : List.<String>  = new List.<String>(); //will be used to count up number of locations in browse results
	
	var objectLocationsDict = new Dictionary.<GameObject, String>();

	for (var object : Object in curPosObjects)
	{
	
		var curObject = object as GameObject;
		curObjName = curObject.name;
		
		var curObjNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
		var curLocationName = curObjNode.SelectSingleNode("./ModelInfo/PhotogramInfo/PhotogramLocation/PhotogramLocationName").InnerText;
		
//		Debug.Log("curLocationName: " + curLocationName);
		
		objectLocationsDict.Add(curObject, curLocationName);
		
		if (!photogramLocations.Contains(curLocationName)) //will only add location name to list if it doesn't already exist
		{
			photogramLocations.Add(curLocationName);
		}
	}
	
	//sort the photogram groups reverse alphabetically to match origin calculations
	photogramLocations = photogramLocations.OrderByDescending(function(curObjName) curObjName).ToList();
	
	findOrigin(photogramLocations, objectLocationsDict); //find floor allocations for the individual groupings
	positionObjects(groupingsOrigin); //move the objects into position
}


function getDesignFields (curPosObjects : List.<GameObject>) //used to determine number of locations in browse results
{
	xmlRoot = infoCont.control.root;

	var curObjName : String;

	var designFields : List.<String>  = new List.<String>(); //will be used to count up number of design fields in browse results
	
	var objectFieldsDict = new Dictionary.<GameObject, String>();

	for (var object : Object in curPosObjects)
	{
	
		var curObject = object as GameObject;
		curObjName = curObject.name;
		
		var curObjNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
		var curDesignField = curObjNode.SelectSingleNode("./ModelInfo/DesignInfo/DesignCreation/DesignCreationField").InnerText;
		
		objectFieldsDict.Add(curObject, curDesignField);
		
		if (!designFields.Contains(curDesignField)) //will only add location name to list if it doesn't already exist
		{
			designFields.Add(curDesignField);
		}
	}
	
	//sort the design fields groups reverse alphabetically to match origin calculations
	designFields = designFields.OrderByDescending(function(curObjName) curObjName).ToList();
	
	findOrigin(designFields, objectFieldsDict); //find floor allocations for the individual groupings
	
	positionObjects(groupingsOrigin); //move the objects into position
}



function getSearchTypes(curPosObjects : List.<GameObject>)
{
	xmlRoot = infoCont.control.root;

	var curObjName : String;

	var createTypes : List.<String>  = new List.<String>(); //will be used to count up number of design fields in browse results
	
	var objectTypesDict = new Dictionary.<GameObject, String>();

	for (var object : Object in curPosObjects)
	{
	
		var curObject = object as GameObject;
		curObjName = curObject.name;
		
		var curObjNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
		var curCreateType = curObjNode.SelectSingleNode("./ModelInfo/ModelCreateType").InnerText;
		
		objectTypesDict.Add(curObject, curCreateType);
		
		if (!createTypes.Contains(curCreateType)) //will only add location name to list if it doesn't already exist
		{
			createTypes.Add(curCreateType);
		}
	}
	
	//sort the design fields groups reverse alphabetically to match origin calculations
	createTypes = createTypes.OrderByDescending(function(curObjName) curObjName).ToList();
	
	findOrigin(createTypes, objectTypesDict); //find floor allocations for the individual groupings
	
	positionObjects(groupingsOrigin); //move the objects into position


}




function findOrigin( groupings : List.<String>, objectSortDict : Dictionary.<GameObject, String>)
{
//	Debug.Log("****** DEBUG findOrigin test *******");
	var numberOfGroupings = groupings.Count;
	
	var floorPlaneCol = loadPlane.GetComponent(BoxCollider);	
	
	var floorSizeX = floorPlaneCol.bounds.size.x;
	var floorOriginX = 0 - (floorSizeX /2); //note that this only works with the plane's centre @ 0,0,0
	
//	Debug.Log("floorSizeX: " + floorSizeX);
//	Debug.Log("floorOriginX: " + floorOriginX);
//	Debug.Log("numberOfGroupings: " + numberOfGroupings);
	
	var floorSubdivisionSize = floorSizeX / numberOfGroupings;
//	Debug.Log("floorSubdivisionSize: " + floorSubdivisionSize);
	
	var groupingsXOrigin = new Dictionary.<String, float>();
	
	for (var group : String in groupings)
	{	
		var subDivLength = floorOriginX + floorSubdivisionSize;
		floorOriginX += floorSubdivisionSize; //removes accounted for section from the overall size
		var subDivXOrigin = subDivLength - (floorSubdivisionSize /2);
		groupingsXOrigin.Add(group, subDivXOrigin);
		
//		Debug.Log("subDivXOrigin: " + subDivXOrigin); // **HERE** - for browse group titles
	}
	
	
	
	//Dictionary containing gameobject and vector3 

	groupingsOrigin = new Dictionary.<GameObject, Vector3>();
	
	for (var browseObj : KeyValuePair.<GameObject, String> in objectSortDict)
	{
		var curObj : GameObject = browseObj.Key;
		var curSortField : String = browseObj.Value;
		//Debug.Log("Dictionary entry= " + " Object Name: " + curObj.name + " " + " Location: " + curSortField);
		
		var curObjXPos = groupingsXOrigin[curSortField]; //value of objSort dict matches the key of the groupingsOrigin dict
	
		var curXVector = new Vector3(curObjXPos, 0, 0);
		groupingsOrigin.Add(curObj, curXVector);
	}

	
	//Z coordinate code
	for (var group : String in groupings) //for each of the groups being sorted
	{
		var tempGroupObjects = new List.<GameObject>(); //tempList containing objects for eeach group
		
		//Create Temp List of Group Objects
		for (var browseObj : KeyValuePair.<GameObject, String> in objectSortDict) //cycle throuhg the browse objects
		{
			if (browseObj.Value == group) //if the grouping value of the browse object matches the current group being sorted
			{
				var curBrowseObj = browseObj.Key;
				
				//Use BoxCol to find placement
				var curObjBoxCol = curBrowseObj.AddComponent(BoxCollider);
				var curPartLocate : GameObject = Instantiate(particleLocate, curBrowseObj.GetComponent.<BoxCollider>().bounds.center, Quaternion.identity);//***NEW***
				curPartLocate.transform.parent = curBrowseObj.transform;
				//curPartLocate.transform.rotation = Quaternion.EulerAngles(-90,0,0);
				
				tempGroupObjects.Add(curBrowseObj);  
			}		
		}
		
		//once objects for each group have been collected together find their pos
		var prevZOrigin : float = 0; //used to reset for each group
		var prevObjDepth = 0;
		
		for (var groupObj : GameObject in tempGroupObjects) //cycle through objects in group list
		{
			var curVector : Vector3 = groupingsOrigin[groupObj];
			
			if (prevZOrigin == 0) //default positioning for starting objects in the list
			{
				curVector.z = objZOriginStartPos;
				groupingsOrigin[groupObj] = curVector; // need to reassign vector
				
				prevObjDepth = groupObj.GetComponent(BoxCollider).bounds.size.z;
				prevZOrigin = objZOriginStartPos;
				
			} 
			else //if anything but the first starting obj
			{
				var curObjDepth = groupObj.GetComponent(BoxCollider).bounds.size.z;
				
				var newZOrigin = prevZOrigin + prevObjDepth /2 + objSpaceGap + curObjDepth/2;
				curVector.z = newZOrigin;
				groupingsOrigin[groupObj] = curVector; // need to reassign vector
			
				prevZOrigin = newZOrigin;
				prevObjDepth = curObjDepth;
			}
		}
	}
//	Debug.Log("****** /DEBUG findOrigin test *******");	
}



function positionObjects(objectPosDict : Dictionary.<GameObject, Vector3>)
{
	//assigns rb/coll components and moves objects into position

	for (var browseObj : KeyValuePair.<GameObject, Vector3> in objectPosDict)
	{
		var curPosObj : GameObject = browseObj.Key;
		var curObjPosition : Vector3 = browseObj.Value;
		//Debug.Log("Return Object Name: " + curPosObj.name + " " + " Location: " + curObjPosition.ToString());
	
		curPosObj.transform.position = curObjPosition;
	}
	
	for (var browseObj : KeyValuePair.<GameObject, Vector3> in objectPosDict)
	{
		var curRbObj : GameObject = browseObj.Key;
		var rb = curRbObj.AddComponent(Rigidbody);
		rb.useGravity = true;
		rb.isKinematic = false;
	}
}


