using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Browse_BrowseControl : MonoBehaviour {

	//Controls the instantiation of artefacts to Browse scene after user browse query

	public Transform instantParent;
	private Transform[] instantPoints;
	private GameObject[] importedObjects;
	public Transform browseArtefactParent;
	public Object particleLocator;

	public GameObject progressBar;
	public LoadProgressBar ProgressBarCont;


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
		}
	}
		

	/// <summary>
	/// Imports browse artefact's mesh and texture, assigns object info
	/// </summary>
	/// <param name="browseIdentifiers">array of identifiers to browse</param>
	public void ImportArtefacts(string[] browseIdentifiers)
	{
		ResetInstances();
		importedObjects = new GameObject[browseIdentifiers.Length];

		progressBar.SetActive(true); 
		ProgressBarCont.SetMaxVal(browseIdentifiers.Length *2);


		for (int i = 0; i < browseIdentifiers.Length; i++) {
			string meshLocation = Paths.Remote + DublinCoreReader.GetMeshLocationForArtefactWithIdentifier(browseIdentifiers [i]);
			string texLocation = Paths.Remote + DublinCoreReader.GetTextureLocationForArtefactWithIdentifier(browseIdentifiers [i]);
			StartCoroutine (ImportModel (i, browseIdentifiers[i], meshLocation, texLocation));
		}
	}


	/// <summary>
	/// Removes artefacts between browse queries
	/// </summary>
	private void ResetInstances()
	{
		for (int i = 0; i < browseArtefactParent.transform.childCount; i++) {
			Destroy(browseArtefactParent.GetChild(i).gameObject);
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

		ProgressBarCont.AddTask("Importing " + browseIdentifier);

		importedObjects[index] = objReader.gameObjects[0];

		// Create GameObject
		Texture2D objTexture = new Texture2D (512, 512);
		importedObjects[index].GetComponent<MeshRenderer> ().material.mainTexture = objTexture;
		importedObjects[index].GetComponent<MeshRenderer>().enabled = false; //turns off the mesh before placement to avoid clustering
		importedObjects[index].name = browseIdentifier; //artefact gameobject will be identifier for ease of reference
		importedObjects[index].tag = "Active Model";
		importedObjects[index].AddComponent<BoxCollider> ();
		importedObjects[index].transform.SetParent(browseArtefactParent);

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
		Instantiate(particleLocator, browseArtefact.transform);

		Vector3 artefactPosition = instantPoints [instantNumber].position;
		browseArtefact.transform.position = artefactPosition;
		Rigidbody rb = browseArtefact.AddComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		browseArtefact.GetComponent<MeshRenderer>().enabled = true;

		ProgressBarCont.AddTask("Placing " + browseArtefact.name);
	}
}


