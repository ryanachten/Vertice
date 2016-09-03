using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Browse_BrowseControl : MonoBehaviour {

	//Controls the instantiation of artefacts to Browse scene after user browse query

	public Transform instantParent;
	private Transform[] instantPoints;
	private GameObject[] importedObjects;
	private Texture2D objTexture;


	void Start()
	{
		GetInstantPoints(); //updates points once per execution
	}
		

	/// <summary>
	/// Gets transforms from editor mockup to assign to instantiated artefacts
	/// </summary>
	void GetInstantPoints ()
	{
		instantPoints = new Transform[instantParent.childCount];

		for (int i = 0; i < instantPoints.Length; i++) {
			instantPoints[i] = instantParent.GetChild(i);
//			Debug.Log("instantPoint " + i + ": " + instantPoints[i].position);
		}
	}


	/// <summary>
	/// Imports browse artefact's mesh and texture, assigns object info
	/// </summary>
	/// <param name="browseIdentifiers">array of identifiers to browse</param>
	public void ImportArtefacts(List<string> browseIdentifiers)
	{
		for (int i = 0; i < browseIdentifiers.Count; i++) {
			Dictionary<string, Dictionary<string, string[]>> curArtefactDict = DublinCoreReader.GetArtefactWithIdentifier (browseIdentifiers [i]);

			string meshLocation = curArtefactDict["relatedAssets"]["MeshLocation"][0]; //TODO check that there isn't more than one meshLocation
			StartCoroutine (ImportModel(meshLocation));
			GameObject curArtefact = importedObjects [0];

			objTexture = new Texture2D (512, 512);
			string texLocation = curArtefactDict["relatedAssets"]["TexLocation"][0]; //TODO check that there isn't more than one texLocation
			StartCoroutine(ImportTexture(texLocation));
			curArtefact.GetComponent<MeshRenderer> ().material.mainTexture = objTexture;

			curArtefact.name = browseIdentifiers [i]; //artefact gameobject will be identifier for ease of reference
			curArtefact.tag = "Active Model";
			curArtefact.AddComponent<BoxCollider> ();


			PlaceArtefact (i, curArtefact);
		}
	}
		
	/// <summary>
	/// Imports mesh information using ObjReader
	/// </summary>
	/// <returns>Array containing gameobject</returns>
	/// <param name="meshLocation">Location of mesh information</param>
	IEnumerator ImportModel(string meshLocation)
	{
		ObjReader.ObjData objReader = ObjReader.use.ConvertFileAsync(meshLocation, false);
		while (!objReader.isDone) 
		{
			yield return null;
		}
		importedObjects = objReader.gameObjects;
	}
		
	/// <summary>
	/// Imports texture information
	/// </summary>
	/// <returns>Model 2D texture</returns>
	/// <param name="texLocation">Location of texture</param>
	IEnumerator ImportTexture(string texLocation)
	{
		string wwwDirectory = texLocation;
		WWW www = new WWW(wwwDirectory);

		while (!www.isDone){
			yield return null;
		}
		www.LoadImageIntoTexture(objTexture);
	}


	/// <summary>
	/// Places artefact at one of the instance points defined in Editor
	/// </summary>
	/// <param name="instantNumber">Instantiation number to place artefact at</param>
	/// <param name="browseArtefact">Artefact to place</param>
	private void PlaceArtefact(int instantNumber, GameObject browseArtefact)
	{
		Vector3 artefactPosition = instantPoints [instantNumber].position;
		browseArtefact.transform.position = artefactPosition;
	}
}


