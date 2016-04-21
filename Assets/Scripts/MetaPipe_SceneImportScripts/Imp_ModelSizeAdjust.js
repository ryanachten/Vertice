#pragma strict

import UI;

var objInfoCont : ObjInfoControl;

var curObj : GameObject;

var widthValText : Text;
var heightValText : Text;
var depthValText : Text;

var scaleValText : Text;
var scaleSlider : Slider;

//externally triggered via ObjReport script 
function updateScaleVals()
{
//	Debug.Log("Updating scale fields");

	if (curObj == null)
		curObj = GameObject.FindGameObjectWithTag("Current Model");

	var meshBounds : Bounds = curObj.GetComponent.<MeshRenderer>().bounds;
	
	var meshWidth = meshBounds.size.x;
		widthValText.text = meshWidth.ToString("F2");
	var meshHeight = meshBounds.size.y;
		heightValText.text = meshHeight.ToString("F2");
	var meshDepth = meshBounds.size.z;
		depthValText.text = meshDepth.ToString("F2");
		
	var modelScale = curObj.transform.localScale;
		scaleValText.text = modelScale.x.ToString("F2");
}

function changeScaleSlider()
{
	var scale = scaleSlider.value;
	changeScale(scale);
}


function changeScale( scale : float) //this should really be placed in objInfoAdjust or something to allow for other scenes
{	
	if (curObj == null)
		curObj = GameObject.FindGameObjectWithTag("Current Model");

	if (curObj != null && scale != 0f)
	{
		curObj.transform.localScale = new Vector3(scale,scale,scale);
		updateScaleVals();
	}
	
	if (objInfoCont == null)
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
		
	objInfoCont.control.modelScale = scale.ToString();
	objInfoCont.Save();
}