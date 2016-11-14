using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Vertice_LoadScene : MonoBehaviour {

	public GameObject importButtonObject;
	public Button importButton;
	public Button browseButton;
	public Button collectButton;

	void Start()
	{
		#if UNITY_WEBGL
		importButtonObject.SetActive(false);
		#endif

		OnLevelWasLoaded(Application.loadedLevel);
	}


	public void loadImport()
	{
		Application.LoadLevel("MetaPipe_ImportScene");
		ArtefactSaveData.ClearSaveData();
	}
		
	public void loadBrowse()
	{	
		Application.LoadLevel("MetaPipe_BrowseScene");
		browseButton.interactable = false;
	}
		
	public void loadCollection()
	{	
		Application.LoadLevel("MetaPipe_CollectionScene");
		collectButton.interactable = false;
		CollectDataHost.ResetSaveData();
	}


	void OnLevelWasLoaded(int level)
	{
		if (level == 0) //browse scene
		{
			browseButton.interactable = false;
			Cursor.visible = false;
		}else
		{
			browseButton.interactable = true;
		}


		if (level == 1) //import scene
		{
			importButton.interactable = false;
			Cursor.visible = true;
		}else
		{
			importButton.interactable = true;
		}

		if (level == 2) //collect scene
		{
			collectButton.interactable = false;
			Cursor.visible = false;
		}else
		{
			collectButton.interactable = true;
		}
	}
}
