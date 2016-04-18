#pragma strict

//used to get curObj from Import scene to browse (after browse -> imp edit)

var infoCont : ObjInfoControl;

//tag change method
function loadCollectionList()
{
	var curObj = GameObject.FindGameObjectWithTag("Current Model");
	
	if (curObj != null)
	{
		Destroy(curObj); //destroy curObj as it will be loaded in the list 
		infoCont.control.isImpBrowse = true;
		
		Application.LoadLevel("MetaPipe_CollectionScene");
	}
}
