#pragma strict

//positioning information removed from this version

import System.Xml;

var infoCont : ObjInfoControl;
var xmlDoc : XmlDocument;
var xmlRoot : XmlNode;

var curObjNode : XmlNode;

//import declarations
//var objImp : ObjImporter;
private var curObj : GameObject;
var impTex : Material;
private var objTex : MeshRenderer;
//@HideInInspector
public var texImportComplete : boolean;
//@HideInInspector
public var objImportComplete : boolean;
var impLimitSlider : Slider;
@HideInInspector
var impLimitRangeMin : int; //used as start point for the import limit range
@HideInInspector
var impLimitRangeMax : int;
var totalResultsCount : int; // total number of results from query provided by list.count

var progressBar : GameObject;
var progressBarScript : Browse_ProgressBar;
var camMoveScript : BrowseCamMovement;


//Obj Reader Declarations
@HideInInspector
var standardMaterial : Material;
@HideInInspector
var transparentMaterial : Material;
@HideInInspector
var curSearch : List.<String>;
@HideInInspector
var sameSearch : boolean;
var displayResultsPanel : GameObject;

var sortPositionScript : BrowseImpSortPositionObj;
var curBrowseObjects : List.<GameObject>;

var groupLabelParent : Transform;

function Start()
{
	sameSearch = false;
	displayResultsPanel.SetActive(false);
	
	objImportComplete = true;
	texImportComplete = true;
	
}


function ShowMoreObj()
{
	sameSearch = true; //prevents min/max being overwritten

	impLimitRangeMin = impLimitRangeMax + 1;
	impLimitRangeMax = impLimitRangeMax + impLimitSlider.value + 1;
	
	if (impLimitRangeMax > totalResultsCount)
	{
		impLimitRangeMax = totalResultsCount; //ensures this won't go over the maximum avilable objects	
	}
	
	importList(curSearch);
}



function importList(sortResults : List.<String>)
{
	Debug.Log("****** DEBUG importList test *******");
	curBrowseObjects = new List.<GameObject>();

	xmlRoot = infoCont.control.root;
	
	if (!displayResultsPanel.activeSelf) //activates diplay results panel if not already active
	{
		displayResultsPanel.SetActive(true);	
	}
	
	//Check to see if existing search results are present
	var existSearchResults  = GameObject.FindGameObjectsWithTag("Active Model");
	if (existSearchResults.Length > 0)
	{
		for (var go : GameObject in existSearchResults)
		{
			Destroy(go);
		}
		
		if (groupLabelParent.childCount != 0) //***NEW*** used to delete group labels from previous browse
		{
//			Debug.Log("Deleting group labels");
			for (var childLabel : Transform in groupLabelParent.transform)
			{
				Destroy(childLabel.gameObject);
			}
		}
	}

	//import limits code	
	curSearch = sortResults;

	var impLimitSliderVal = impLimitSlider.value;
	totalResultsCount = sortResults.Count;
			
	if (!sameSearch)
	{
		impLimitRangeMin = 0;
		impLimitRangeMax = impLimitSliderVal;
		if (impLimitRangeMax > totalResultsCount)
		{
			impLimitRangeMax = totalResultsCount; // prevents going beyond index range		
		}
	}
	
	camMoveScript.navLocked = true;
	progressBar.SetActive(true); 
	progressBarScript.setMaxVal(impLimitRangeMax);

	var curObjName : String;

	for ( var i = impLimitRangeMin; i <= impLimitRangeMax -1; i++) //-1 added to account for list index beginning at 0
	{
		curObjName = sortResults[i];
		progressBarScript.AddTask( curObjName);

		if (objImportComplete && texImportComplete)
		{
			var curNode = xmlRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']");
			Debug.Log("IMPORT curNode: " + curObjName);
						
			importObj(curNode);
		}
		if (!objImportComplete || !texImportComplete) 
		{
			yield; //operation must wait for the current download to finish before beginning new one
//			Debug.Log("IMPORT curNode: yielding"); 
		}
	}
	
	if (sameSearch) //resets same search setting
	{
		sameSearch = false;
	}
	
	sortPositionScript.sortMode(curBrowseObjects); //sends objects to be positioned
	
	camMoveScript.navLocked = false;
	progressBar.SetActive(false); 
	Debug.Log("****** /DEBUG importList test *******");
}



function importObj(curObjNode : XmlNode)
{

	objImportComplete = false;
	texImportComplete = false;
	
	var meshLocation : String = curObjNode.SelectSingleNode("./MeshLocation").InnerText;
	var objName : String = curObjNode.SelectSingleNode("@name").Value;
	var modelScale : float = parseFloat(curObjNode.SelectSingleNode("./ModelInfo/ModelScale").InnerText); //***NEW***
//	Debug.Log("Model Scale: " + modelScale);
	
	var importModels : GameObject[] = [];

	#if UNITY_STANDALONE
	// TODO: This code expects an absolute path to a file, but paths will be relative. A BASE_URL should be declared for WebGL and 
	// the "Archive Folder Location" should be used to alter a BASE_PATH for standalone builds
	var importer : ObjReader.ObjData = ObjReader.use.ConvertFileAsync("file://" + meshLocation, false, standardMaterial);

	#elif UNITY_EDITOR || UNITY_WEBGL
	var BASE_URL = "http://s3-ap-southeast-2.amazonaws.com/vertice-dev";
	var fileLocation = BASE_URL + meshLocation;
	Debug.Log("fileLocation: " + fileLocation);
	var importer : ObjReader.ObjData = ObjReader.use.ConvertFileAsync(fileLocation, false, standardMaterial);
	#endif
	while (!importer.isDone){
		yield;
	}
	importModels = importer.gameObjects;

	for (var model : GameObject in importModels)
	{
		curObj = model;
		
		//Add Tag
		curObj.tag = "Active Model"; 
		curObj.name = objName;
		
		objTex = curObj.GetComponent(MeshRenderer);		
	
		importTex(curObjNode);
		
		while (!texImportComplete)
		{
			yield;
		}
		
		model.transform.localScale = new Vector3(modelScale,modelScale,modelScale); //***NEW***
		
		curBrowseObjects.Add(model);
		
		objImportComplete = true;
		//Debug.Log("Done Downloading Obj: " + objName);
	}
}

function importTex(curObjNode : XmlNode)
{

	#if UNITY_STANDALONE
	var texLocation = curObjNode.SelectSingleNode("./TexLocation").InnerText;
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**

	#elif UNITY_WEBGL || UNITY_EDITOR
	var texLocation = curObjNode.SelectSingleNode("./TexLocation").InnerText;
	var wwwDirectory = "http://s3-ap-southeast-2.amazonaws.com/vertice-dev" + texLocation;

	#endif
	
	objTex.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT1, false);
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);
		
		yield www;
	
		www.LoadImageIntoTexture(curObj.GetComponent.<Renderer>().material.mainTexture);
		
		if (www.isDone){
			texImportComplete = true;
			//Debug.Log("Done Downloading Texture");
			break; //if done downloading image break loop
		}
	}
}
