using UnityEngine;
using System.Collections;

public class Browse_BrowseControl : MonoBehaviour {


	/// <summary>
	/// Uses DCReader to return a list of artefacts relevant to user search query
	/// </summary>
	/// <param name="browseMode">Parameter user wants to browse</param>
	public void FindArtefacts(string browseMode)
	{
		//TODO need a DublinCoreReader function for accessing a singular DC attribute for all of the artefacts
		// i.e. Browse by title -> returns something like a dictionary listing all of the titles in the XML
		//alternatively, could just have a function to expose a list of all the artefacts in the XML and I can write logic to test against this

	}

}
