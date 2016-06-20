#pragma strict


var rayDetectScript : CollectRayDetectObj;
@HideInInspector
var curObj : GameObject;
@HideInInspector
var curObjName : String;
@HideInInspector
var prevObjName : String;

var modifyHeight : float; //***NEW*** used to prevent model falling through terrain when modifying

var objPosFieldX : Text;
	var objPosTextX : Text;
var objPosFieldZ : Text;
	var objPosTextZ : Text;

var objRotateFieldX : Text;
	var objRotateTextX : Text;
var objRotateFieldY : Text;
	var objRotateTextY : Text;
var objRotateFieldZ : Text;
	var objRotateTextZ : Text;

var objScaleField : Text;
	var objScaleText : Text;

var objDimenWidthField : Text;
	var objDimenWidthText : Text;
var objDimenHeightField : Text;
	var objDimenHeightText : Text;
var objDimenDepthField : Text;
	var objDimenDepthText : Text;

var dimenIncrementVal : float = 0.05; 
		

function Start()
{
	prevObjName = null;
	//UpdateGUI();

}


function OnGUI()
{
	curObjName = rayDetectScript.curObjName;
	
	if (curObjName != prevObjName)
	{
		UpdateGUI();
		prevObjName = curObjName;		
	}
}


function UpdateGUI()
{
//	Debug.Log("Updating GUI");
	curObj = rayDetectScript.curObj;

	objPosTextX.text = curObj.transform.position.x.ToString("F2");
	objPosTextZ.text = curObj.transform.position.z.ToString("F2");
	
	objRotateTextX.text = curObj.transform.rotation.x.ToString("F2");
	objRotateTextY.text = curObj.transform.rotation.y.ToString("F2");
	objRotateTextZ.text = curObj.transform.rotation.z.ToString("F2");
	
	objScaleText.text = curObj.transform.localScale.x.ToString("F2");
	
	var rendSize : Vector3 = curObj.GetComponent.<Renderer>().bounds.size;
	objDimenWidthText.text = rendSize.x.ToString("F2");
	objDimenHeightText.text = rendSize.y.ToString("F2");
	objDimenDepthText.text = rendSize.z.ToString("F2");
}


function modXPos()
{
	curObj = rayDetectScript.curObj;
	curObj.transform.position.x = parseFloat(objPosFieldX.text);
	curObj.transform.position.y = modifyHeight; 
	UpdateGUI();
}

function modZPos()
{
	curObj = rayDetectScript.curObj;
	curObj.transform.position.z = parseFloat(objPosFieldZ.text);
	curObj.transform.position.y = modifyHeight;
	UpdateGUI();
}

function modXRotate()
{			
	curObj = rayDetectScript.curObj;

	var rotHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2;	//***NEW***
	curObj.transform.position.y = curObj.transform.position.y + rotHeight; //***NEW*** prevents object falling through terrain
	
	curObj.transform.rotation.x = parseFloat(objRotateFieldX.text);
	UpdateGUI();
}

function modYRotate()
{
	curObj = rayDetectScript.curObj;

	var rotHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2;	//***NEW***
	curObj.transform.position.y = curObj.transform.position.y + rotHeight; //***NEW*** prevents object falling through terrain
	
	curObj.transform.rotation.y = parseFloat(objRotateFieldY.text);
	UpdateGUI();
}

function modZRotate()
{
	curObj = rayDetectScript.curObj;

	var rotHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2;	//***NEW***
	curObj.transform.position.y = curObj.transform.position.y + rotHeight; //***NEW*** prevents object falling through terrain
	
	curObj.transform.rotation.z = parseFloat(objRotateFieldZ.text);
	UpdateGUI();
}

function modScale()
{
	curObj = rayDetectScript.curObj;
	
	var scaleFactor = parseFloat(objScaleField.text); //***NEW***
	var objHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2; //***NEW***
	curObj.transform.position.y = objHeight * scaleFactor; //***NEW*** prevents object falling through terrain
	
	curObj.transform.localScale.x = scaleFactor;
	curObj.transform.localScale.y = scaleFactor;
	curObj.transform.localScale.z = scaleFactor; 
	UpdateGUI();
}

function modWidth()
{
	curObj = rayDetectScript.curObj;
	var newWidth = parseFloat(objDimenWidthField.text);
	var curWidth = curObj.GetComponent.<Renderer>().bounds.size.x;
	
	var scaleFactor = newWidth/curWidth; //***NEW***
	var objHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2; //***NEW***
	curObj.transform.position.y = objHeight * scaleFactor; //***NEW*** prevents object falling through terrain
	
	if (curWidth < newWidth) //if the user size is larger than current size
	{ 
		while (curObj.GetComponent.<Renderer>().bounds.size.x < newWidth)
		{
			curObj.transform.localScale.x += dimenIncrementVal;
			curObj.transform.localScale.y += dimenIncrementVal;
			curObj.transform.localScale.z += dimenIncrementVal;
		}
	} else if (curWidth > newWidth) //if the user size is smaller than current size
	{
		while (curObj.GetComponent.<Renderer>().bounds.size.x > newWidth)
		{
			curObj.transform.localScale.x -= dimenIncrementVal;
			curObj.transform.localScale.y -= dimenIncrementVal;
			curObj.transform.localScale.z -= dimenIncrementVal;
		}	
	}

	UpdateGUI();
}


function modHeight()
{
	curObj = rayDetectScript.curObj;
	var newHeight = parseFloat(objDimenHeightField.text);
	var curHeight = curObj.GetComponent.<Renderer>().bounds.size.y;
	
	var scaleFactor = newHeight/curHeight; //***NEW***
	var objHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2; //***NEW***
	curObj.transform.position.y = objHeight * scaleFactor; //***NEW*** prevents object falling through terrain
	
	if (curHeight < newHeight) //if the user size is larger than current size
	{ 
		while (curObj.GetComponent.<Renderer>().bounds.size.y < newHeight)
		{
			curObj.transform.localScale.x += dimenIncrementVal;
			curObj.transform.localScale.y += dimenIncrementVal;
			curObj.transform.localScale.z += dimenIncrementVal;
		}
	} else if (curHeight > newHeight) //if the user size is smaller than current size
	{
		while (curObj.GetComponent.<Renderer>().bounds.size.y > newHeight)
		{
			curObj.transform.localScale.x -= dimenIncrementVal;
			curObj.transform.localScale.y -= dimenIncrementVal;
			curObj.transform.localScale.z -= dimenIncrementVal;
		}	
	}

	UpdateGUI();
}


function modDepth()
{
	curObj = rayDetectScript.curObj;
	var newDepth = parseFloat(objDimenDepthField.text);
	var curDepth = curObj.GetComponent.<Renderer>().bounds.size.z;
	
	var scaleFactor = newDepth/curDepth; //***NEW***
	var objHeight = curObj.GetComponent(BoxCollider).bounds.size.y / 2; //***NEW***
	curObj.transform.position.y = objHeight * scaleFactor; //***NEW*** prevents object falling through terrain
	
	if (curDepth < newDepth) //if the user size is larger than current size
	{ 
		while (curObj.GetComponent.<Renderer>().bounds.size.z < newDepth)
		{
			curObj.transform.localScale.x += dimenIncrementVal;
			curObj.transform.localScale.y += dimenIncrementVal;
			curObj.transform.localScale.z += dimenIncrementVal;
		}
	} else if (curDepth > newDepth) //if the user size is smaller than current size
	{
		while (curObj.GetComponent.<Renderer>().bounds.size.z > newDepth)
		{
			curObj.transform.localScale.x -= dimenIncrementVal;
			curObj.transform.localScale.y -= dimenIncrementVal;
			curObj.transform.localScale.z -= dimenIncrementVal;
		}	
	}

	UpdateGUI();
}