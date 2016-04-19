#pragma strict

import UI;

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

function changeScale()
{	
	if (curObj == null)
		curObj = GameObject.FindGameObjectWithTag("Current Model");

	var scale = scaleSlider.value;

	if (curObj != null && scale != 0f)
	{
		curObj.transform.localScale = new Vector3(scale,scale,scale);
		updateScaleVals();
	}
}