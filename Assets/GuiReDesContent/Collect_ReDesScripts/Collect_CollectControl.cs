using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Collect_CollectControl : MonoBehaviour {


	private GameObject[] importedObjects;
	public Transform collectArtefactParent;
	public string collectionId; //used to check whether collection present and to get trasnform info
	public BoxCollider loadPlaneBoxCol;
	public Object particleLocator;

	public GameObject progressBar;
	public LoadProgressBar ProgressBarCont;


	/// <summary>
	/// Imports collection artefact's mesh and texture, assigns object info
	/// </summary>
	/// <param name="collectionIdentifiers">array of identifiers belonging to collection</param>
	public void ImportArtefacts(string collectId)
	{
		ResetInstances();
		#if UNITY_WEBGL
		StartCoroutine(DownloadArtefactXmlAndImportArtefacts(collectId));
		#elif UNITY_STANDALONE
		DublinCoreReader.LoadXmlFromFile(Paths.ArtefactMetadata);
		GetIdentifiers(collectId);
		#endif
	}


	/// <summary>
	/// Asynchronous, private implementation for the public-facing ImportArtefacts(...) method. Allows the caller to 
	/// ignore any implementation details (i.e. they do not need to call StartCoroutine, etc.
	/// 
	/// Checks to see if the DublinCoreReader has data available, otherwise it downloads it in a non-blocking fashion.
	/// 
	/// When data is present, it is used to import meshes, textures, and metadata, and instantiate objects
	/// </summary>
	/// <param name="collectId">Collection identifier.</param>
	IEnumerator DownloadArtefactXmlAndImportArtefacts(string collectId){
		if (!DublinCoreReader.HasXml ()) {
			UnityWebRequest www = UnityWebRequest.Get (Paths.ArtefactMetadata); //Paths.Remote + "/Metadata/Vertice_ArtefactInformation.xml"
			yield return www.Send ();

			if (www.isError) {
				// TODO: Echo the error condition to the user
				Debug.Log ("Couldn't download XML file" + www.error);
			} else {
				DublinCoreReader.LoadXmlFromText (www.downloadHandler.text);
				Debug.Log("Downloaded some XML");
			}
		}
		GetIdentifiers(collectId);
	}


	private void GetIdentifiers(string collectId)
	{
		collectionId = collectId;

		string[] collectionIdentifiers = CollectionReader.GetIdentifiersForArtefactsInCollectionWithIdentifier(collectId);
		importedObjects = new GameObject[collectionIdentifiers.Length];

		progressBar.SetActive(true); 
		ProgressBarCont.SetMaxVal(collectionIdentifiers.Length *2);

		for (int i = 0; i < collectionIdentifiers.Length; i++) {

			string meshLocation = Paths.PathToFile(DublinCoreReader.GetMeshLocationForArtefactWithIdentifier(collectionIdentifiers [i]));
			string texLocation = Paths.PathToFile(DublinCoreReader.GetTextureLocationForArtefactWithIdentifier(collectionIdentifiers [i])); 
			StartCoroutine (ImportModel (i, collectionIdentifiers[i], meshLocation, texLocation));
		}
	}



	/// <summary>
	/// Removes artefacts between browse queries
	/// </summary>
	private void ResetInstances()
	{
		for (int i = 0; i < collectArtefactParent.transform.childCount; i++) {
			Destroy(collectArtefactParent.GetChild(i).gameObject);
		}
	}


	/// <summary>
	/// Imports mesh information using ObjReader
	/// </summary>
	/// <returns>Array containing gameobject</returns>
	/// <param name="meshLocation">Location of mesh information</param>
	IEnumerator ImportModel(int index, string collectArtefactIdentifier, string meshLocation, string texLocation)
	{

		// Download mesh
		ObjReader.ObjData objReader = ObjReader.use.ConvertFileAsync(meshLocation, false);
		while (!objReader.isDone) 
		{
			yield return null;
		}
		ProgressBarCont.AddTask("Importing " + collectArtefactIdentifier);

		importedObjects[index] = objReader.gameObjects[0];


		// Create GameObject
		Texture2D objTexture = new Texture2D (512, 512);
		importedObjects[index].GetComponent<MeshRenderer> ().material.mainTexture = objTexture;
		importedObjects[index].GetComponent<MeshRenderer>().enabled = false; //turns off the mesh before placement to avoid clustering
		importedObjects[index].name = collectArtefactIdentifier; //artefact gameobject will be identifier for ease of reference
		importedObjects[index].tag = "Active Model";
		importedObjects[index].AddComponent<BoxCollider> ();
		importedObjects[index].transform.SetParent(collectArtefactParent);

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
	private void PlaceArtefact(int instantNumber, GameObject collectArtefact)
	{
		Dictionary<string, Dictionary<string, float>> transInfo;
		VerticeTransform VertTrans;

		try {
			transInfo = CollectionReader.GetTransformForArtefactWithIdentifierInCollection(collectionId, collectArtefact.name);
			VertTrans = new VerticeTransform(transInfo);	
		} 
		catch (System.Exception ex) 
		{
			transInfo = null;

			VertTrans = new VerticeTransform(loadPlaneBoxCol.bounds.min.x, loadPlaneBoxCol.bounds.max.x, 
												loadPlaneBoxCol.bounds.min.z, loadPlaneBoxCol.bounds.max.z);
			Debug.Log("No pos info available, random assignment");
			Debug.Log("Random pos: " + VertTrans.position.x + " " + VertTrans.position.y + " " + VertTrans.position.z);
		}	
		Instantiate(particleLocator, collectArtefact.transform);

		collectArtefact.transform.position = VertTrans.position;;
		Rigidbody rb = collectArtefact.AddComponent<Rigidbody> ();
		rb.mass = 3;
		collectArtefact.GetComponent<MeshRenderer>().enabled = true;

		ProgressBarCont.AddTask("Placing " + collectArtefact.name);

	}
}
