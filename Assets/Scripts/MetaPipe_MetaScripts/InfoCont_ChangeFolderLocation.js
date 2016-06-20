#pragma strict

var objInfoCont : ObjInfoControl;


public function OpenFolder(){ //**REVISE** this function is pretty redundant
	
	UniFileBrowser.use.OpenFolderWindow(false, SelectFolder);
}

public function SelectFolder(pathToFolder : String){

	var folderLocation = pathToFolder; //assigned to public var - reference script to access
//	Debug.Log("folderLocation: " + folderLocation);
	
	var verticePath : String = "/VerticeArchive/"; //used to identify substring index
	
	var pathSplitIndex = folderLocation.IndexOf(verticePath);
//	Debug.Log("pathSplitIndex: " + pathSplitIndex);
	
	var splitFolderDirect = folderLocation.Substring(0, pathSplitIndex);
//	Debug.Log("splitFolderDirect: " + splitFolderDirect);
	
	if (objInfoCont == null)
	{
		objInfoCont = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjInfoControl>();
	}
	objInfoCont.ChangeFolderLocation(splitFolderDirect, folderLocation);
}