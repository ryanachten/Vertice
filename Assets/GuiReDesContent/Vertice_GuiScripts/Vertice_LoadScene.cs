using UnityEngine;
using System.Collections;

public class Vertice_LoadScene : MonoBehaviour {



	public void loadImport()
	{
		Application.LoadLevel("MetaPipe_ImportScene");	
	}
		
	public void loadBrowse()
	{	
		Application.LoadLevel("MetaPipe_BrowseScene");
	}


	public void loadCollection()
	{	
		Application.LoadLevel("MetaPipe_CollectionScene");
	}
}
