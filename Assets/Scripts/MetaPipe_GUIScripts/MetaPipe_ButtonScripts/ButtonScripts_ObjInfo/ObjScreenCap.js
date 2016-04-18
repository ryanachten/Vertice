#pragma strict

import System.IO;

var objCont : ObjInfoControl;




function TakeScreenCap(){

	ScreenCap();
}


function ScreenCap(){
	
	var objName = objCont.control.objName;
	
	yield WaitForEndOfFrame();
	
	var width = Screen.width;
	var height = Screen.height;
	var tex = new Texture2D(height, height, TextureFormat.RGB24, false);
	
	tex.ReadPixels(Rect(Screen.width/2 - height/2, 0, height, height), 0, 0);
	tex.Apply();
	
	var bytes = tex.EncodeToPNG();
	Destroy(tex);
	
	var screenCapLocation = Application.dataPath + "/" + objName + "_ScreenShot.png";
	File.WriteAllBytes(screenCapLocation, bytes);
	
//	objCont.control.objImgCap = screenCapLocation;
	Debug.Log("screenCapLocation: " + screenCapLocation);


}
