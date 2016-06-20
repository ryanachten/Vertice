#pragma strict

var loadScript : MetaPipe_LoadScene;


function OnTriggerEnter( otherCol : Collider)
{
	if (otherCol.tag == "Player")
	{
	//	Debug.Log("Load Import Scene");
		loadScript.loadImportScene();
	}
}