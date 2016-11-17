using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class VerticeArchivePreferences : MonoBehaviour {

	public GameObject preferencesPanel;
	public Text locationText;

	void Start()
	{
		#if UNITY_WEBGL
			gameObject.SetActive(false);
		#elif UNITY_STANDALONE
			SetVerticeArchiveLocation();
		#endif
	}

	void SetVerticeArchiveLocation()
	{
		if(Paths.VerticeArchive == null)
		{
//			Debug.Log("Getting Player Prefs");
			string verticeArchiveLocation = PlayerPrefs.GetString("VerticeArchive Location");

			if(verticeArchiveLocation.Length == 0)
			{
				Debug.Log("Player Prefs Empty - need to assign");
				preferencesPanel.SetActive(true);
			}
			else
			{
				Paths.VerticeArchive = verticeArchiveLocation;
				Debug.Log("Paths.VerticeArchive set to: " + Paths.VerticeArchive);
				locationText.text = Paths.VerticeArchive as string;  //TODO Update location string
			}
		}
	}

	public void TogglePrefsPanel()
	{
		if (!preferencesPanel.activeSelf)
		{
			preferencesPanel.SetActive(true);
		}else
		{
			preferencesPanel.SetActive(false);
		}
	}

	public void OpenDialogue()
	{
		UniFileBrowser.use.OpenFolderWindow(true, SetPlayerPrefs);
	}


	private void SetPlayerPrefs(string pathToVerticeArchiveFolder)
	{
		if (pathToVerticeArchiveFolder.EndsWith("VerticeArchive/"))
		{
			int directoryIndex = pathToVerticeArchiveFolder.IndexOf("/VerticeArchive");
//			Debug.Log("directoryIndex: " + directoryIndex);

			string directorySubstring = pathToVerticeArchiveFolder.Substring(0, directoryIndex);
//			Debug.Log("directorySubstring: " + directorySubstring);

//			string verticeArchiveLocation = "file://" + directorySubstring; //TODO this "file://" prob needs to change for Windows
			string verticeArchiveLocation = directorySubstring;
//			Debug.Log("verticeArchiveLocation: " + verticeArchiveLocation);

			PlayerPrefs.SetString("VerticeArchive Location", verticeArchiveLocation);
			SetVerticeArchiveLocation();

			preferencesPanel.SetActive(false);
		}
		else
		{
			locationText.text = "The folder selected is not a VerticeArchive folder";
		}
	}

	public void ResetPlayerPrefs()
	{
		PlayerPrefs.SetString("VerticeArchive Location", "");
		Debug.Log("VerticeArchive Location: " + PlayerPrefs.GetString("VerticeArchive Location"));
	}
}
