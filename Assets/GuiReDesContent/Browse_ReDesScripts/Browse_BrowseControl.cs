using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Browse_BrowseControl : MonoBehaviour {

	//Controls the instantiation of artefacts to Browse scene after user browse query

	public Transform instantParent;
	private Transform[] instantPoints;
	private GameObject[] importedObjects;
	//private Texture2D objTexture;


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
	public void ImportArtefacts(string[] browseIdentifiers)
	{
		importedObjects = new GameObject[browseIdentifiers.Length];
		for (int i = 0; i < browseIdentifiers.Length; i++) {
			string meshLocation = "file://" + Application.dataPath + "/../.." + DublinCoreReader.GetMeshLocationForArtefactWithIdentifier(browseIdentifiers [i]); //TODO change directory to reference Paths.js
			string texLocation = "file://" + Application.dataPath + "/../.." + DublinCoreReader.GetTextureLocationForArtefactWithIdentifier(browseIdentifiers [i]); //TODO change directory to reference Paths.js
			StartCoroutine (ImportModel (i, browseIdentifiers[i], meshLocation, texLocation));
		}
	}
		
	/// <summary>
	/// Imports mesh information using ObjReader
	/// </summary>
	/// <returns>Array containing gameobject</returns>
	/// <param name="meshLocation">Location of mesh information</param>
	IEnumerator ImportModel(int index, string browseIdentifier, string meshLocation, string texLocation)
	{

		// Download mesh
		ObjReader.ObjData objReader = ObjReader.use.ConvertFileAsync(meshLocation, false);
		while (!objReader.isDone) 
		{
			yield return null;
		}
		importedObjects[index] = objReader.gameObjects[0];


		// Create GameObject
		Texture2D objTexture = new Texture2D (512, 512);
		importedObjects[index].GetComponent<MeshRenderer> ().material.mainTexture = objTexture;
		importedObjects[index].name = browseIdentifier; //artefact gameobject will be identifier for ease of reference
		importedObjects[index].tag = "Active Model";
		importedObjects[index].AddComponent<BoxCollider> ();

		// Download texture
		WWW www = new WWW(texLocation);

		while (!www.isDone){
			yield return null;
		}
		www.LoadImageIntoTexture(objTexture);



		PlaceArtefact (index, importedObjects[index]);
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
		browseArtefact.AddComponent<Rigidbody> ();
	}
}


