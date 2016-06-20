#pragma strict

var objInfoCont : ObjInfoControl;

private var modelCreateType : String;

var photoToggle : Toggle;
var designToggle : Toggle;

var photogramPanel : GameObject;
var designPanel : GameObject;

var prevType : String;

function Start()
{
	if (objInfoCont == null)
	{
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
	prevType = "";
}



function OnGUI()
{
	if (objInfoCont.control.modelCreateType.Length > 3 && objInfoCont.control.modelCreateType != prevType)
	{
		LoadType();
		prevType = objInfoCont.control.modelCreateType;
	}
	else if (objInfoCont.control.modelCreateType.Length < 1) //***NEW***
	{
		if (photogramPanel.activeSelf == true)
			photogramPanel.SetActive(false);
		
		if (designPanel.activeSelf == true)
			designPanel.SetActive(false);
	}
}



function LoadType(){
	
	if (objInfoCont == null) //***NEW***
	{
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
	
	modelCreateType = objInfoCont.control.modelCreateType;
	
	if (modelCreateType.Length > 1){
	
		if(modelCreateType == "Photogrammetric"){
			photoToggle.isOn = true;
		}
		else if(modelCreateType == "Design"){
			designToggle.isOn = true;
		}		
		activateTypePanels();
	} 
//	activateTypePanels();
}


function ChangeType(){
	
	if (objInfoCont == null)
	{
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
	
	if(photoToggle.isOn){
		objInfoCont.control.modelCreateType = "Photogrammetric";
		objInfoCont.Save(); //***HERE***
//		Debug.Log("CHNG TYPE PHOTO modelCreateType: " + objInfoCont.control.modelCreateType);
	}
	else if(designToggle.isOn){
		objInfoCont.control.modelCreateType = "Design";
		objInfoCont.Save(); //***HERE***
//		Debug.Log("CHNG TYPE DES modelCreateType: " + objInfoCont.control.modelCreateType);
	}
}

function activateTypePanels()
{
	if (objInfoCont.control.modelCreateType == "Photogrammetric")
	{
		photogramPanel.SetActive(true);
	} else {
		photogramPanel.SetActive(false);
	}
	if (objInfoCont.control.modelCreateType == "Design")
	{
		designPanel.SetActive(true);
	} else {
		designPanel.SetActive(false);
	}
}