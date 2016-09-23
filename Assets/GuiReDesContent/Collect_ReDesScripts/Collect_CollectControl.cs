using UnityEngine;
using System.Collections;

public class Collect_CollectControl : MonoBehaviour {

	public void GetCollectionArtefacts(string collectId)
	{
		string[] collectArtefactIds = CollectionReader.GetIdentifiersForArtefactsInCollectionWithIdentifier(collectId);
		for (int i = 0; i < collectArtefactIds.Length; i++) {
			Debug.Log("collectArtefactIds[i]: " + collectArtefactIds[i]);

			//TODO Need to create a new Collection XML document to properly test using institution artefacts
		}

	}

}
