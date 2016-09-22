using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class NoSuchCollectionException : Exception
{
	public NoSuchCollectionException ()
	{}

	public NoSuchCollectionException (string message) 
		: base(message)
	{}

	public NoSuchCollectionException (string message, Exception innerException)
		: base (message, innerException)
	{}    
}


[Serializable]
public class NoSuchArtefactInCollectionException : Exception
{
	public NoSuchArtefactInCollectionException ()
	{}

	public NoSuchArtefactInCollectionException (string message) 
		: base(message)
	{}

	public NoSuchArtefactInCollectionException (string message, Exception innerException)
		: base (message, innerException)
	{}    
}

[Serializable]
public class MalformedTransformCoordinateException : Exception
{
	public MalformedTransformCoordinateException ()
	{}

	public MalformedTransformCoordinateException (string message) 
		: base(message)
	{}

	public MalformedTransformCoordinateException (string message, Exception innerException)
		: base (message, innerException)
	{}    
}

public static class CollectionReader {

	static XmlDocument _xmlDocument;
	static string _uri;

	/// <summary>
	/// Loads XML data from a given filepath or URL
	/// </summary>
	/// <param name="filePath">File path.</param>
	public static void LoadXml(string uri){
		_uri = uri;
		Refresh ();
	}

	/// <summary>
	/// Synchronise the system's copy of the collection library with the XML file stored on disk
	/// </summary>
	/// <exception cref="FileNotFoundException">Throws an exception if the the URI for the XML file was never set</exception>
	public static void Refresh(){

		if (_uri == null) {
			Debug.Log ("The URI for XML data is not set. Did you forget to call LoadXml with a URI before calls to RefreshXML?");
			throw new FileNotFoundException ();
		}

		// FIXME Loading in XML data will block until the data is returned. Using coroutines isn't a solution, since DublinCoreReader is not
		// a subclass of MonoBehvaiour
		WWW www = new WWW (_uri);
		while (!www.isDone) {
		}
		_xmlDocument = new XmlDocument ();
		_xmlDocument.LoadXml (www.text);

	}

	/// <summary>
	/// Lazily read in XML data to the local _xmlDocument variable
	/// </summary>
	/// <returns>The XML document representing collections in the system</returns>
	static XmlDocument Xml(){
		if (_xmlDocument == null) {
			Refresh ();
		}
		return _xmlDocument;
	}

	/// <summary>
	/// Get a list of identifiers for collections in an XML file
	/// </summary>
	/// <returns>A string array containing the identifiers for each collection in the XML file</returns>
	public static string[] GetIdentifiersForCollections() {
		XmlNodeList collections = Xml().SelectNodes("//verticeCollection/@id");
		string[] collectionIdentifiers = new string[collections.Count];

		for (int i = 0; i < collections.Count; i++) {
			collectionIdentifiers [i] = collections [i].InnerXml;
		}

		return collectionIdentifiers;
	}

	/// <summary>
	/// Retrieves the descriptive metadata associated with a collection
	/// </summary>
	/// <returns>The descriptive metadata in a nested dictionary having the form: 
	/// 
	/// {
	/// 	"title": ["Title A", "Some other title"],
	/// 	"description": ["This is a description"],
	/// 	"creator": ["Creator A", "Some other creator"]
	/// 	...
	/// }
	/// 
	/// No guarantees are provided about the contents of the dictionary; it is possible that some fields might be omitted. However, if a 
	/// field is included, it will contain data. As such, callers need only check whether the field that they expect actually exists. E.g. 
	/// 
	/// try {
	/// 	string[] creators = metadataDictionary["description"] 
	/// 	foreach (string creator in creators) {
	/// 		// Do something
	/// 	}
	/// } catch (KeyNotFoundException) {
	/// 	...
	/// }
	/// </returns>
	/// <param name="collectionIdentifier">The identifier of the collection for which to get metadata</param>
	public static Dictionary<string, string[]> GetCollectionMetadataWithIdentifier(string collectionIdentifier){

		// TODO Replace the MOCK_DICTIONARY with a real implementation
		Dictionary<string, string[]> MOCK_DICTIONARY = new Dictionary<string, string[]> ();

		string[] identifier = new string[1];
		string[] title = new string[1];
		string[] creator = new string[1];
		string[] date = new string[1];
		string[] coverage = new string[2];
		string[] subject = new string[1];
		string[] description = new string[1];
		string[] extent = new string[1];


		identifier [0] = "P14C3H01D3R-00";
		title [0] = "Photogrammetry Test Scans";
		creator [0] = "Ryan Achten";
		date [0] = "2015-11-29";
		coverage [0] = "Evan's Bay";
		coverage [1] = "Basin Reserve";
		subject [0] = "Photogrammetry";
		description[0] = "A museum is distinguished by a collection of often unique objects that forms the core of its activities for exhibitions, education, research, etc.";

		string[] artefactIds = GetIdentifiersForArtefactsInCollectionWithIdentifier ("P14C3H01D3R-00");
		string extentString = String.Format ("{0} artefacts", artefactIds.Length);
		extent [0] = extentString;

			
		MOCK_DICTIONARY.Add("title", title);
		MOCK_DICTIONARY.Add("identifier", identifier);
		MOCK_DICTIONARY.Add("creator", creator);
		MOCK_DICTIONARY.Add("date", date);
		MOCK_DICTIONARY.Add("coverage", coverage);
		MOCK_DICTIONARY.Add("subject", subject);
		MOCK_DICTIONARY.Add("description", description);
		MOCK_DICTIONARY.Add("extent", extent);

		return MOCK_DICTIONARY;

	}

	/// <summary>
	/// Gets the identifiers for artefacts in a collection with a particular collection ID
	/// </summary>
	/// <returns>A string array of artefact identifiers; these should be compatible with the DublinCoreReader, for example.</returns>
	/// <param name="collectionIdentifier">The collection identifier, as discovered by, e.g., GetIdentifiersForCollection()</param>
	/// <exception cref="NoSuchCollectionException">Thows NoSuchCollectionException if the collectionIdentifier does not exist</exception>
	public static string[] GetIdentifiersForArtefactsInCollectionWithIdentifier(string collectionIdentifier) {

		if (Xml().SelectNodes(String.Format("//verticeCollection[@id='{0}']", collectionIdentifier)).Count == 0){
			throw new NoSuchCollectionException(String.Format("The collection with identifier {0} does not exist", collectionIdentifier));
		}
		
		XmlNodeList artefacts = Xml().SelectNodes(String.Format("//verticeCollection[@id='{0}']/structural/artefact/@id", collectionIdentifier));
		string[] artefactIdentifiers = new string[artefacts.Count];

		for (int i = 0; i < artefacts.Count; i++) {
			artefactIdentifiers [i] = artefacts [i].InnerXml;
		}

		return artefactIdentifiers;
	}


	/// <summary>
	/// Produces a nested dictionary containing the transform coordinates for position, rotation, and scale of a specified artefact in a given collection
	/// </summary>
	/// <returns>A nested dictionary of the form: 
	/// 	{
	/// 		'position': {
	/// 						'x': #.#f
	/// 						'y': #.#f
	/// 						'z': #.#f
	/// 		}
	/// 		'rotation': {
	/// 						'x': #.#f
	/// 						'y': #.#f
	/// 						'z': #.#f
	/// 						'w': #.#f
	/// 		}
	/// 		'scale': {
	/// 						'x': #.#f
	/// 						'y': #.#f
	/// 						'z': #.#f
	/// 		}
	/// 
	/// 	}
	/// 
	/// </returns>
	/// <param name="artefactIdentifier">The identifier for the artefact, probably determined with GetIdentifiersForArtefactsInCollectionWithIdentifier(...)</param>
	/// <param name="collectionIdentifier">The identifier for the collection, probably determined with GetIdentifiersForCollections(...)</param>
	/// <exception cref="NoSuchArtefactException">Throws NoSuchArtefactException if the given artefact does not exist in the given collection (this will also be thrown if -- more generally -- the collection doesn't exist)</exception>
	/// <exception cref="MalformedTransformCoordinateException">Throws MalformedTransformCoordinateException if at least one coordinate is not recorded in the XML file</exception>
	public static Dictionary<string, Dictionary<string, float>> GetTransformForArtefactWithIdentifierInCollection(string collectionIdentifier, string artefactIdentifier) {

		// Declare position coordinate values
		float posX;
		float posY;
		float posZ;

		// Declare rotation coordinate values
		float rotX;
		float rotY;
		float rotZ;
		float rotW;

		// Declare scale coordinate values
		float scaleX;
		float scaleY;
		float scaleZ;

		// Ensure that the given artefact exists in the given collection
		if (Xml().SelectNodes(String.Format("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']", collectionIdentifier, artefactIdentifier)).Count == 0){
			throw new NoSuchArtefactInCollectionException(String.Format("The artefact with identifier {0} does not exist in collection with identifier {1}", artefactIdentifier, collectionIdentifier));
		}

		try {

			// Extract transform coordinates for position
			string _posX = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/position/x", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _posY = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/position/y", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _posZ = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/position/z", collectionIdentifier, artefactIdentifier)).InnerXml;

			// Extract transform coordinates for rotation
			string _rotX = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/rotation/x", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _rotY = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/rotation/y", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _rotZ = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/rotation/z", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _rotW = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/rotation/w", collectionIdentifier, artefactIdentifier)).InnerXml;

			// Extract transform coordinates for scale
			string _scaleX = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/scale/x", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _scaleY = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/scale/y", collectionIdentifier, artefactIdentifier)).InnerXml;
			string _scaleZ = Xml ().SelectSingleNode (String.Format ("//verticeCollection[@id='{0}']/structural/artefact[@id='{1}']/transform/scale/z", collectionIdentifier, artefactIdentifier)).InnerXml;

			// Parse coordinates for position
			if (!Single.TryParse (_posX, out posX)) {
				Debug.Log ("No valid coordinate was supplied for the x position -- setting to 0.0");
			}

			if (!Single.TryParse (_posY, out posY)) {
				Debug.Log ("No valid coordinate was supplied for the y position -- setting to 0.0");
			}

			if (!Single.TryParse (_posZ, out posZ)) {
				Debug.Log ("No valid coordinate was supplied for the z position -- setting to 0.0");
			}

			// Parse coordinates for rotation
			if (!Single.TryParse (_rotX, out rotX)) {
				Debug.Log ("No valid coordinate was supplied for the x rotation -- setting to 0.0");
			}

			if (!Single.TryParse (_rotY, out rotY)) {
				Debug.Log ("No valid coordinate was supplied for the y rotation -- setting to 0.0");
			}

			if (!Single.TryParse (_rotZ, out rotZ)) {
				Debug.Log ("No valid coordinate was supplied for the z rotation -- setting to 0.0");
			}

			if (!Single.TryParse (_rotW, out rotW)) {
				Debug.Log ("No valid coordinate was supplied for the w rotation -- setting to 0.0");
			}

			// Parse coordinates for scale
			if (!Single.TryParse (_scaleX, out scaleX)) {
				Debug.Log ("No valid coordinate was supplied for the x scale -- setting to 0.0");
			}

			if (!Single.TryParse (_scaleY, out scaleY)) {
				Debug.Log ("No valid coordinate was supplied for the y scale -- setting to 0.0");
			}

			if (!Single.TryParse (_scaleZ, out scaleZ)) {
				Debug.Log ("No valid coordinate was supplied for the z scale -- setting to 0.0");
			}

		} catch (NullReferenceException e) {
			Debug.Log (e.Message);
			throw new MalformedTransformCoordinateException (
				String.Format ("One or more transform coordinates are not present in the XML file for the artefact with identifier {0} in collection with identifier {1}", artefactIdentifier, collectionIdentifier)
			);
		}
			
		// Initialise the dictionaries
		Dictionary<string, Dictionary<string, float>> transformDictionary = new Dictionary<string, Dictionary<string, float>> ();
		Dictionary<string, float> positionDictionary = new Dictionary<string, float> ();
		Dictionary<string, float> rotationDictionary = new Dictionary<string, float> ();
		Dictionary<string, float> scaleDictionary = new Dictionary<string, float> ();

		// Add position coordinats to the dictionary
		positionDictionary.Add ("x", posX);
		positionDictionary.Add ("y", posY);
		positionDictionary.Add ("z", posZ);

		// Add rotation coordinates to the dictionary
		rotationDictionary.Add ("x", rotX);
		rotationDictionary.Add ("y", rotY);
		rotationDictionary.Add ("z", rotZ);
		rotationDictionary.Add ("w", rotW);

		// Add scale coordinates to the dictionary
		scaleDictionary.Add ("x", scaleX);
		scaleDictionary.Add ("y", scaleY);
		scaleDictionary.Add ("z", scaleZ);

		// Finally, add the nested dictionaries to the nesting dictionary
		transformDictionary.Add("position", positionDictionary);
		transformDictionary.Add ("rotation", rotationDictionary);
		transformDictionary.Add ("scale", scaleDictionary);

		return transformDictionary;


	}
}
