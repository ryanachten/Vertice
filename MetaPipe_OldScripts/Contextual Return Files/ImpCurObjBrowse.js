#pragma strict

//used to get curObj from Import scene to browse (after browse -> imp edit)

var infoCont : ObjInfoControl;

//tag change method
function loadBrowseCurObj()
{
	var curObj = GameObject.FindGameObjectWithTag("Current Model");
	
	if (curObj != null)
	{
		var curObjPrevPos : Vector3 = infoCont.control.curObjPrevPos;
		curObj.transform.position = curObjPrevPos; //load previous pos before edit
		
		var rb = curObj.GetComponent(Rigidbody);
		rb.isKinematic = false;
		 
		curObj.tag = "Active Model";
		infoCont.control.isImpBrowse = true;
		
		Application.LoadLevel("MetaPipe_BrowseScene");
	}
}
