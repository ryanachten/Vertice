#pragma strict

var loadScript : MetaPipe_LoadScene;


//function OnTriggerEnter()
function OnTriggerEnter( otherCol : Collider)
{
	if (otherCol.tag == "Player")
	{
//		Debug.Log("Collider object: " + otherCol.gameObject.name);
		//	Debug.Log("Load Import Scene");
		loadScript.loadBrowseScene();
	}
}