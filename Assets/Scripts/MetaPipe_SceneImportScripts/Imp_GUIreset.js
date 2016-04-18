#pragma strict

var objInfoCont : ObjInfoControl;


function resetTextFields()
{
	if (objInfoCont == null)
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	
	objInfoCont.control.objName = "";
			
}


/*
var artifactName : Text;
var fileName : Text;
var contribDate : Text;
var contribName : Text;
var artifactDescript : Text;


function resetTextFields()
{
	Debug.Log("Reset GUI");
	artifactName.text = "Upload a model";
	fileName.text = "";
	contribDate.text = "";
	contribName.text = "";
	artifactDescript.text = "";
}*/