#pragma strict

import System.Linq;
import System.Xml;
import System.Collections.Generic;

var usrTxt : Text;
var infoCont : ObjInfoControl;
var xmlDoc : XmlDocument;
var xmlRoot : XmlNode;

private var usrTxtInput : String;

//Refine search toggles
var objNameToggle : Toggle;
var persNameToggle : Toggle;
var dateToggle : Toggle;

//Import Obj / List stuff
var browseImportObj : BrowseImportObj;
var browseObjXml : BrowseObjTypeXml; //set browse type to search

var curObjName : String;
//public var browseResults : List.<String>;
public var sortResults : List.<String>;



function Start ()
{
	xmlRoot = infoCont.control.root;	
}


function searchXml()
{
	
	usrTxtInput = usrTxt.text;
	if (usrTxtInput.Length > 2)
	{
		browseObjXml.browseObjType = "Search";
		
		if(objNameToggle.isOn)
		{
			Debug.Log("Search Object Name");
			searchObjectNameXml();
		} 
		else if(persNameToggle.isOn)
		{
			Debug.Log("Search Person Name");
			searchPersonNameXml();
		} 
		else if(dateToggle.isOn)
		{
			Debug.Log("Search Date");
			searchDateXml();
		} 
		else if (!objNameToggle.isOn && !persNameToggle.isOn && !dateToggle.isOn)
		{
			Debug.Log("General Search");
			searchWholeXml();
		}
	}
}



function searchWholeXml()
{
//	var mpo = xmlRoot.SelectNodes("MetaPipeObject//*[contains(text(), 'Logs')]");
//	
//	Debug.Log("MPO Count: " + mpo[1].SelectSingleNode("../MetaPipeObject/@name"));

	var mpo = xmlRoot.SelectNodes("MetaPipeObject");
	var browseResults = new List.<String>();
	var searchResults : boolean = false;
		
	Debug.Log("Search query for: " + usrTxtInput);				
	for (var i = 0; i < mpo.Count; i++)
	{
		var curNode = mpo[i];
		var nodeCheck = curNode.SelectNodes(".//*[contains(text(), '" + usrTxtInput + "')]");
		
		if (nodeCheck.Count >= 1)
		{
			//Debug.Log("nodeCheck: " + nodeCheck.Count);
						
			//***List Test***
			curObjName = curNode.SelectSingleNode("@name").Value;
			browseResults.Add(curObjName);
			
			searchResults = true;
		}
	}
		
	if (searchResults == false)
	{
		Debug.Log("No matches to query");
	}
	
	//***List Test***
	sortResults = browseResults.OrderBy(function(curObjName) curObjName).ToList();
	browseImportObj.importList(sortResults);
}


function searchObjectNameXml()
{
	var mpo = xmlRoot.SelectNodes("MetaPipeObject"); // /ContextualInfo/ContextMedia
	
	var browseResults = new List.<String>();
	var searchResults : boolean = false;
	Debug.Log("Search query for: " + usrTxtInput);	
				
	for (var i = 0; i < mpo.Count; i++){
	
		var curNode = mpo[i];

		var cntxtnodeCheck = curNode.SelectNodes("./ContextualInfo/ContextMedia/MediaName[contains(.,'" + usrTxtInput + "')]");
		
//		var objNameNodeCheck = curNode.SelectNodes("./@name[contains(.,'translate(" + usrTxtInput + ",'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')')]");
//		var objNameNodeCheck = curNode.SelectNodes("./@name[contains(lower-case(.),'" + usrTxtInput + "')]");

		var objNameNodeCheck = curNode.SelectNodes("./@name[contains(.,'" + usrTxtInput + "')]");
		
		if (cntxtnodeCheck.Count >= 1 || objNameNodeCheck.Count >= 1) //cntxtnodeCheck.Count -> cntxtnodeCheck.Count >= 1 
		{	
			//***List Test***
			curObjName = curNode.SelectSingleNode("@name").Value;
			browseResults.Add(curObjName);
			
			searchResults = true;
		}
	}				
	if (searchResults == false)
	{
		Debug.Log("No matches to query");
	}
	
	//***List Test***
	sortResults = browseResults.OrderBy(function(curObjName) curObjName).ToList();
	browseImportObj.importList(sortResults);
}


function searchPersonNameXml()
{
	var mpo = xmlRoot.SelectNodes("MetaPipeObject"); // /ContextualInfo/ContextMedia
	
	var browseResults = new List.<String>();
	var searchResults : boolean = false;
	Debug.Log("Search query for: " + usrTxtInput);	
				
	for (var i = 0; i < mpo.Count; i++){
	
		var curNode = mpo[i];
		
		var contribNameNodeCheck = curNode.SelectNodes("./ContribName[contains(.,'" + usrTxtInput + "')]");
		var createNameNodeCheck = curNode.SelectNodes("./ModelInfo/ModelCreator[contains(.,'" + usrTxtInput + "')]");

		if (contribNameNodeCheck.Count || createNameNodeCheck.Count >= 1)
		{
		
			//***List Test***
			curObjName = curNode.SelectSingleNode("@name").Value;
			browseResults.Add(curObjName);
			
			searchResults = true;
		}
	}				
	if (searchResults == false)
	{
		Debug.Log("No matches to query");
	}
	
	//***List Test***
	sortResults = browseResults.OrderBy(function(curObjName) curObjName).ToList();
	browseImportObj.importList(sortResults);
}


function searchDateXml()
{
	var mpo = xmlRoot.SelectNodes("MetaPipeObject"); // /ContextualInfo/ContextMedia
	
	var browseResults = new List.<String>();
	var searchResults : boolean = false;
	Debug.Log("Search query for: " + usrTxtInput);	
				
	for (var i = 0; i < mpo.Count; i++){
	
		var curNode = mpo[i];
		
		var contribDateNodeCheck = curNode.SelectNodes("./ContribDate[contains(.,'" + usrTxtInput + "')]");
		var modelCreateDateNodeCheck = curNode.SelectNodes("./ModelInfo/ModelCreateDate[contains(.,'" + usrTxtInput + "')]");

		if (contribDateNodeCheck.Count || modelCreateDateNodeCheck.Count >= 1) //|| createNameNodeCheck.Count
		{
			
			//***List Test***
			curObjName = curNode.SelectSingleNode("@name").Value;
			browseResults.Add(curObjName);
			
			searchResults = true;
		}
	}				
	if (searchResults == false)
	{
		Debug.Log("No matches to query");
	}
	
	//***List Test***
	sortResults = browseResults.OrderBy(function(curObjName) curObjName).ToList();
	browseImportObj.importList(sortResults);
}


