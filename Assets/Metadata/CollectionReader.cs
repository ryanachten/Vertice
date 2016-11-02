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

	/// <summary>
	/// Loads XML data using the passed in text
	/// </summary>
	/// <param name="text">The XML-formatted text</param>
	public static void LoadXmlFromText(string text){
		_xmlDocument = new XmlDocument ();
		_xmlDocument.LoadXml (text);
	}

	/// <summary>
	/// Loads XML data from a given filepath
	/// </summary>
	/// <param name="filePath">File path.</param>
	public static void LoadXmlFromFile(string filePath){
		_xmlDocument = new XmlDocument ();
		_xmlDocument.Load (filePath);
	}

	/// <summary>
	/// Lazily read in XML data to the local _xmlDocument variable
	/// </summary>
	/// <returns>The XML document representing collections in the system</returns>
	static XmlDocument Xml(){
		if (_xmlDocument == null) {
			// TODO: Replace with an appropriate exception and handler
			Debug.LogError ("Attempting to access a Collection XML document that has not been properly loaded");
			return new XmlDocument ();
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
	/// Gets a list of element values for a given element in a given <descriptive> node
	/// </summary>
	/// <returns>An array of strings representing the values associated with a given element</returns>
	/// <param name="descriptiveNode">The <descriptive> node for a particular <verticeCollection></param>
	/// <param name="elementName">The name of the element (e.g. title, description, etc.)</param>
	private static string[] GetValuesForElementsWithNameInDescriptiveNode(XmlNode descriptiveNode, string elementName){
	
		XmlNodeList elements = descriptiveNode.SelectNodes (String.Format ("./{0}", elementName));
		string[] values = new string[elements.Count];
		for (int i = 0; i < elements.Count; i++) {
//			Debug.Log (elements [i].InnerXml);
			values [i] = elements [i].InnerXml;
		}

		return values;
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

		Dictionary<string, string[]> retVal = new Dictionary<string, string[]> ();
		HashSet<string> elements = new HashSet<string> ();
		XmlNode descriptionElements = Xml ().SelectSingleNode (String.Format ("//verticeCollections/verticeCollection[@id='{0}']/descriptive", collectionIdentifier));
		foreach (XmlNode element in descriptionElements.ChildNodes) {
			elements.Add (element.Name);
		}

		foreach (string element in elements) {
			retVal.Add(element, GetValuesForElementsWithNameInDescriptiveNode(descriptionElements, element));
		}

		int numberOfArtefacts = Xml ().SelectNodes (String.Format ("//verticeCollections/verticeCollection[@id='{0}']/structural/artefact", collectionIdentifier)).Count;
		string numberOfArtefactsString = numberOfArtefacts.ToString ();
		retVal.Add ("extent", new string[]{ numberOfArtefactsString });
		retVal.Add ("identifier", new string[]{ collectionIdentifier });
		return retVal;

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
