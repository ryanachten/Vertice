using UnityEngine;
using System.Collections;

public class Import_ImportArtefact : MonoBehaviour {

	public Transform ImportParent;
	public Material defaultMaterial;

	void Start()
	{
//		ArtefactSaveData SaveHost = new ArtefactSaveData();
//		ArtefactSaveData.MeshLocation = "Testing Save Host";
//		Debug.Log("MeshLocation: " + ArtefactSaveData.MeshLocation);
	}



	public void OpenDialogue(string openMode){

		//TODO add default path directory for dialogue window

		string[] fileExtensions = new string[1]; //Filter Files
		if (openMode == "model")
		{
			fileExtensions[0] = "obj";
			UniFileBrowser.use.SetFileExtensions(fileExtensions);
			UniFileBrowser.use.OpenFileWindow(OpenModel);
		}
		else if (openMode == "texture")
		{
			fileExtensions[0] = "jpg";
			UniFileBrowser.use.SetFileExtensions(fileExtensions);
			UniFileBrowser.use.OpenFileWindow(OpenTexture);
		}
	}


	private void OpenModel(string pathToModel)
	{
		if (pathToModel.Length > 0)
		{
			StartCoroutine(ImportModel("file://" + pathToModel));
			ArtefactSaveData.ClearSaveData();
		}
	}


	private void OpenTexture(string pathToTex)
	{
		if (pathToTex.Length > 0)
		{
			StartCoroutine(ImportTexture("file://" + pathToTex));
		}
	}


	/// <summary>
	/// Imports mesh information using ObjReader
	/// </summary>
	/// <returns>Array containing gameobject</returns>
	/// <param name="meshLocation">Location of mesh information</param>
	IEnumerator ImportModel(string meshLocation)
	{
		ObjReader.ObjData objReader = ObjReader.use.ConvertFileAsync(meshLocation, false, defaultMaterial);
		while (!objReader.isDone) 
		{
			yield return null;
		}
			
		GameObject curArtefact = objReader.gameObjects[0];
		curArtefact.tag = "Current Model";
		curArtefact.transform.SetParent(ImportParent);
//		curArtefact.name = browseIdentifier; //TODO assign default name to gameobject

		ArtefactSaveData.MeshLocation = meshLocation;
//		Debug.Log("MeshLocation: " + ArtefactSaveData.MeshLocation);
	}


	IEnumerator ImportTexture( string texLocation)
	{
		GameObject curArtefact = ImportParent.GetChild(0).gameObject;
		Texture2D objTexture = new Texture2D (512, 512);
		curArtefact.GetComponent<MeshRenderer> ().material.mainTexture = objTexture;

		// Download texture
		WWW www = new WWW(texLocation);

		while (!www.isDone){
			yield return null;
		}
		www.LoadImageIntoTexture(objTexture);

		ArtefactSaveData.TexLocation = texLocation;
//		Debug.Log("TexLocation: " + ArtefactSaveData.TexLocation);
	}
}
