#pragma strict


var movingVarPanel : GameObject;
	var curXposText : Text;
	//var curYposText : Text;
	var curZposText : Text;
	var newXposText : Text;
	//var newYposText : Text;
	var newZposText : Text;

var rotateVarPanel : GameObject;
	var curXrotText : Text;
	var curYrotText : Text;
	var curZrotText : Text;
	var newXrotText : Text;
	var newYrotText : Text;
	var newZrotText : Text;

var scaleVarPanel : GameObject;
	var curScaleText : Text;
	var curWidthText : Text;
	var curHeightText : Text;
	var curDepthText : Text;
	var newScaleText : Text;
	var newWidthText : Text;
	var newHeightText : Text;
	var newDepthText : Text;	

var rayDetectScript : CollectRayDetectObj;
var modifySelectObjName : Text;
var curObjName : String;
var prevObjName : String;


function Start()
{
	prevObjName = null;
}

function Update()
{
	curObjName = rayDetectScript.curObjName;
	if (curObjName != prevObjName)
	{
		modifySelectObjName.text = curObjName;
		prevObjName = curObjName;		
	}

	if (!Input.GetKey(KeyCode.E) && movingVarPanel.activeSelf)
	{
		movingVarPanel.SetActive(false);
	}
	
	if (!Input.GetKey(KeyCode.R) && rotateVarPanel.activeSelf)
	{
		rotateVarPanel.SetActive(false);
	}
	if (!Input.GetKey(KeyCode.T) && scaleVarPanel.activeSelf)
	{
		scaleVarPanel.SetActive(false);
	}
}

function activateMoveDisplay(curPos : Vector3, newPos : Vector3)
{
	if (!movingVarPanel.activeSelf)
	{
		movingVarPanel.SetActive(true);
	}
	
	curXposText.text = curPos.x.ToString("F2");
	curZposText.text = curPos.z.ToString("F2");
	
	newXposText.text = newPos.x.ToString("F2");
	newZposText.text = newPos.z.ToString("F2");	
}

function activateRotateDisplay(curRot : Quaternion, newRot : Quaternion)
{
	if (!rotateVarPanel.activeSelf)
	{
		rotateVarPanel.SetActive(true);
	}
	
	curXrotText.text = curRot.eulerAngles.x.ToString("F2");
	curYrotText.text = curRot.eulerAngles.y.ToString("F2");
	curZrotText.text = curRot.eulerAngles.z.ToString("F2");
	
	newXrotText.text = newRot.eulerAngles.x.ToString("F2");
	newYrotText.text = newRot.eulerAngles.y.ToString("F2");
	newZrotText.text = newRot.eulerAngles.z.ToString("F2");
}

function activateScaleDisplay(curScale : Vector3, curDimen : Vector3, newScale : Vector3, newDimen : Vector3)
{
	if (!scaleVarPanel.activeSelf)
	{
		scaleVarPanel.SetActive(true);
	}
	
	curScaleText.text = curScale.x.ToString("F2");
	curWidthText.text = curDimen.x.ToString("F2");   
	curHeightText.text = curDimen.y.ToString("F2");  
	curDepthText.text = curDimen.z.ToString("F2");   
	
	newScaleText.text = newScale.x.ToString("F2");  
	newWidthText.text = newDimen.x.ToString("F2");   
	newHeightText.text = newDimen.y.ToString("F2");  
	newDepthText.text = newDimen.z.ToString("F2");   

}