#pragma strict


public var vertText : GameObject;
public var triText : GameObject;
public var triTextField : Text;
public var vertTextField : Text;

public var curObj : GameObject;

public static var triCount : int;
public static var vertCount : int;

public var triSlide : TriSliderVal; //holds objReport script 

public var modelInfoPanelScript : PanelModelInfo;
private var modelInfoPanelActive : boolean;


function Awake(){


}


public function ObjReport(){ //reports vert info - called on import
	
	modelInfoPanelActive = modelInfoPanelScript.panelActive;
	var curObj : GameObject = GameObject.FindGameObjectWithTag("Current Model");
	
	
	if (modelInfoPanelActive == true && curObj != null){
		//cache all the button etc that need reports
		//execute as part of the import chain -> assign vars to the xml vars
		
		vertTextField = vertText.GetComponentInChildren(Text);
		triTextField = triText.GetComponentInChildren(Text);
		
		//Vert Report
		var meshVert : Vector3 [] = curObj.GetComponent(MeshFilter).mesh.vertices;
		vertCount = meshVert.length;
		
		if (curObj == null)
			Debug.Log("curObj == null");

		if (vertTextField == null)
			Debug.Log("vertTextField == null");
						
		vertTextField.text = "" + vertCount;
		
		
		//Tri Report
		var meshTri : int[] = curObj.GetComponent(MeshFilter).mesh.triangles;
		triCount = meshTri.length /3; //by itself returns indices in tri /3 to get tri count
		triTextField.text = "" + triCount;
		
		triSlide.triSlideUpdate();
		
	}
}
