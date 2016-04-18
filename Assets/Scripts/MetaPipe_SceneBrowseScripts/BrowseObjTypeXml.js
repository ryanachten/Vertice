#pragma strict

import System.Linq;
import System.Xml;
import System.Collections.Generic;


var infoCont : ObjInfoControl;
var xmlDoc : XmlDocument;
var xmlRoot : XmlNode;

var photogramObjButton : Button;
var designObjButton : Button;
var archiObjButton : Button;
var newMediaObjButton : Button;

var browseImportObj : BrowseImportObj;

public var sortResults : List.<String>;

public var browseObjType : String;

function browseXml(browseType : String)
{
	browseObjType = browseType;

	xmlRoot = infoCont.control.root;
	var mpo = xmlRoot.SelectNodes("MetaPipeObject"); // /ContextualInfo/ContextMedia
	
	var searchResults : boolean = false;
	Debug.Log("Search query for: " + browseType);	

	var curObjName : String;
	var browseResults = new List.<String>();
	
//	Debug.Log("****** DEBUG browseXml test *******");
	
	for (var i = 0; i < mpo.Count; i++){
	
		var curNode = mpo[i];
		var createTypeNodeCheck = curNode.SelectNodes("./ModelInfo/ModelCreateType[contains(.,'" + browseType + "')]");

		if (createTypeNodeCheck.Count >= 1)
		{
			searchResults = true;
			
			//***List Test***
			curObjName = curNode.SelectSingleNode("@name").Value;
			browseResults.Add(curObjName);
//			Debug.Log("BROWXML curObjName:" + curObjName);
		}
	}				
	if (searchResults == false)
	{
//		Debug.Log("No matches to query");
	}
	
	//***List Test***
	sortResults = browseResults.OrderBy(function(curObjName) curObjName).ToList();

	browseImportObj.importList(sortResults); //sends all relevant objects to import
	
//	Debug.Log("****** /DEBUG browseXml test *******");
}