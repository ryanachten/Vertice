﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collect_CollectControl : MonoBehaviour {


	private GameObject[] importedObjects;
	public Transform collectArtefactParent;
	public string collectionId; //used to check whether collection present and to get trasnform info
	public BoxCollider loadPlaneBoxCol;
	public Object particleLocator;


	/// <summary>
	/// Imports collection artefact's mesh and texture, assigns object info
	/// </summary>
	/// <param name="collectionIdentifiers">array of identifiers belonging to collection</param>
	public void ImportArtefacts(string collectId)
	{

		collectionId = collectId;

		DublinCoreReader.LoadXml("file://" + Application.dataPath + "/Scripts/Metadata/TestAssets/Vertice_ArtefactInformation.xml");
//		ResetInstances(); //TODO reset function

		string[] collectionIdentifiers = CollectionReader.GetIdentifiersForArtefactsInCollectionWithIdentifier(collectId);
		importedObjects = new GameObject[collectionIdentifiers.Length];

		for (int i = 0; i < collectionIdentifiers.Length; i++) {

			string meshLocation = "file://" + Application.dataPath + "/../.." + DublinCoreReader.GetMeshLocationForArtefactWithIdentifier(collectionIdentifiers [i]); //TODO change directory to reference Paths.js
			string texLocation = "file://" + Application.dataPath + "/../.." + DublinCoreReader.GetTextureLocationForArtefactWithIdentifier(collectionIdentifiers [i]); //TODO change directory to reference Paths.js
			StartCoroutine (ImportModel (i, collectionIdentifiers[i], meshLocation, texLocation));
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
		importedObjects[index] = objReader.gameObjects[0];


		// Create GameObject
		Texture2D objTexture = new Texture2D (512, 512);
		importedObjects[index].GetComponent<MeshRenderer> ().material.mainTexture = objTexture;
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

	}
}
