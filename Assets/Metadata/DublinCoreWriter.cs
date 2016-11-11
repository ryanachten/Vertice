using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

public static class DublinCoreWriter {

	static XmlDocument xmlDocument;

	/// <summary>
	/// Write a metadata record to XML
	/// </summary>
	/// <param name="identifier">The identifier for the artefact</param>
	/// <param name="data">A nested dictionary representing the structure of a metadata record. 
	/// 
	/// 	{ 
	/// 		"descriptive" : {
	/// 			"title" : ["A title", "A subtitle"],
	/// 			...
	/// 			}
	///			"structural" : {
	/// 			"identifier" : ["12345.obj"],
	/// 			...
	/// 			}
	/// 		}
	/// 	}
	/// 
	/// </param>
	public static void WriteDataForArtefactWithIdentifier(string identifier, Dictionary<string, object> metadata, string meshLocation, string texLocation, Dictionary<string, string>[] contextualMedia){

		try {
			LoadXmlFromFile (Paths.ArtefactMetadata);
		} catch (FileNotFoundException fnfException) {
			Debug.Log (String.Format("Could not open an XML file at {0} -- will create new metadata file at that path", Paths.ArtefactMetadata));
			EstablishNewDocument ();
		} catch (DirectoryNotFoundException dirException) {
			Debug.Log (String.Format ("Could not open directory\n\n {0}", dirException.Message));
			EstablishNewDocument ();
		} 

		try {
			XmlElement artefactRoot = GetArtefactRoot(identifier);
			UnpackDictionaries (metadata, artefactRoot);
			Debug.Log("Unpacked dictionaries");
			AddRelatedAssets (meshLocation, texLocation, contextualMedia, artefactRoot);
			Debug.Log("Added related assets");
			WriteXmlToFile(Paths.ArtefactMetadata);
			Debug.Log(String.Format("Wrote metadata to {0}", Paths.ArtefactMetadata));
		} catch (NullReferenceException nullReference) {
			Debug.LogError ("An error occurred converting a nested dictionary to XML -- you must pass in a nested dictionary (not null!)");
		} catch (ArgumentNullException nullArgument){
			Debug.LogError (String.Format("An error occurred writing the file to XML -- it's likely that no filePath was passed. Exception message:\n\n\n{0}", nullArgument.Message));
		}

	}

	static void AddRelatedAssets(string mesh, string tex, Dictionary<string, string>[] contextualMedia, XmlElement artefactRoot) {
		XmlNode relatedAssets = xmlDocument.CreateElement ("relatedAssets");

		// Add Mesh and Texture locations to <relatedAssets>
		XmlElement meshLocation = xmlDocument.CreateElement ("MeshLocation");
		XmlElement texLocation = xmlDocument.CreateElement ("TexLocation");
		meshLocation.InnerText = mesh;
		texLocation.InnerText = tex;
		relatedAssets.AppendChild (meshLocation);
		relatedAssets.AppendChild (texLocation);

		// Loop over contextual media and add
		foreach (Dictionary<string, string> c in contextualMedia) {
			try {
				XmlElement contextMedia = xmlDocument.CreateElement ("ContextMedia");
				XmlElement mediaName = xmlDocument.CreateElement ("MediaName");
				XmlElement mediaType = xmlDocument.CreateElement ("MediaType");
				XmlElement mediaLocation = xmlDocument.CreateElement ("MediaLocation");

				mediaName.InnerText = c ["MediaName"];
				mediaType.InnerText = c ["MediaType"];
				mediaLocation.InnerText = c ["MediaLocation"];

				contextMedia.AppendChild (mediaName);
				contextMedia.AppendChild (mediaType);
				contextMedia.AppendChild (mediaLocation);

				relatedAssets.AppendChild (contextMedia);
			} catch {
				Debug.Log ("Couldn't unpack contextual media attributes -- is there a missing attribute?");
			}
		} 

		artefactRoot.AppendChild (relatedAssets);
		
	}

	/// <summary>
	/// Creates a new document with a <verticeMetadata> root node and XML preamble
	/// </summary>
	static void EstablishNewDocument(){
		xmlDocument = new XmlDocument ();
		// Add the fundaments of an XML document: the xml processing instruction and a root element
		XmlProcessingInstruction declareVersionAndEncoding = xmlDocument.CreateProcessingInstruction ("xml", "version='1.0' encoding='UTF-8'");
		XmlElement rootElement = xmlDocument.CreateElement ("verticeMetadata");
		xmlDocument.AppendChild (declareVersionAndEncoding);
		xmlDocument.AppendChild (rootElement);
	}

	/// <summary>
	/// Recursively the Dictionary passed in to the constructor and mimics the nested structure in the 
	/// XML document, as a child of the passed in element <verticeMetadata>
	/// </summary>
	/// <param name="metadataDictionary">Metadata dictionary.</param>
	/// <param name="rootElement">The parent element for this nested metadata dictionary</param>
	/*static void UnpackDictionaries(Dictionary<string, object> metadataDictionary, XmlElement parentElement)	{


		foreach (string key in metadataDictionary.Keys) {
			if (metadataDictionary[key].GetType() == typeof(string[])) {
				UnpackList (key, (string[]) metadataDictionary [key], parentElement);
			}
			else {
				XmlElement newElement = xmlDocument.CreateElement ((string)(object)key);
				parentElement.AppendChild (newElement);
				Debug.Log ("Unpacking dictionary: " + key);
				UnpackDictionaries((Dictionary<string, object>) metadataDictionary [key], newElement);
			}
		}
	
	}*/

	static void UnpackDictionaries(Dictionary<string, object> metadataDictionary, XmlElement parentElement)    {


		foreach (string key in metadataDictionary.Keys) {
			if (metadataDictionary [key].GetType () == typeof(string[])) {
				UnpackList (key, (string[])metadataDictionary [key], parentElement);
			} else if (metadataDictionary [key].GetType () == typeof(Dictionary<string, object>)) {
				XmlElement newElement = xmlDocument.CreateElement ((string)(object)key);
				parentElement.AppendChild (newElement);
				Debug.Log ("Unpacking dictionary: " + key);
				UnpackDictionaries ((Dictionary<string, object>)metadataDictionary [key], newElement);
			} else {
				Debug.Log ("The value associated with metadataDictionary [" + key + "] violates the nested dictionary structure. It has the type: " + metadataDictionary [key].GetType ());
			}
		}

	}




	/// <summary>
	/// Gets the root node for an artefact record, given its unique identifier
	/// </summary>
	/// <returns>An empty XmlElement (with an id attribute representing the identifier for the artefact) and no child elements.</returns>
	/// <param name="identifier">The identifier for the artefact</param>
	static XmlElement GetArtefactRoot(string identifier){
		XmlNode artefactRootNode = xmlDocument.SelectSingleNode (String.Format ("/verticeMetadata/artefact[@id='{0}']", identifier));
		if (artefactRootNode != null) {
			xmlDocument.SelectSingleNode ("/verticeMetadata").RemoveChild (artefactRootNode);
		}
		XmlElement artefactRoot = xmlDocument.CreateElement ("artefact");
		artefactRoot.SetAttribute ("id", identifier);
		xmlDocument.SelectSingleNode ("/verticeMetadata").AppendChild (artefactRoot);
		return artefactRoot;

	}

	/// <summary>
	/// Unpacks the leaves of the nested dictionary (i.e. the values for a field, expressed in an array). That is, given the 
	/// following dictionary:
	/// 
	/// { 
	/// 		"descriptive" : {
	/// 			"title" : ["A title", "A subtitle"],
	/// 			...
	/// 			}
	///			"structural" : {
	/// 			"identifier" : ["12345.obj"],
	/// 			...
	/// 			}
	/// 		}
	/// 	}
	/// 
	/// this method will unpack, e.g., the "title" and "identfier" arrays
	/// </summary>
	/// <param name="elementName">The name for the new element</param>
	/// <param name="elementValues">The array of values to be associated with this element</param>
	/// <param name="parentElement">The parent element that the newly created element(s) will be added to</param>
	static void UnpackList(string elementName, string[] elementValues, XmlElement parentElement){
//		Debug.Log ("Unpacking list: " + elementName);
		foreach (string value in elementValues) {
//			Debug.Log ("Adding " + elementName + " node to " + parentElement.LocalName + " with value " + value);
			XmlElement newElement = xmlDocument.CreateElement (elementName);
			newElement.InnerText = value;
			parentElement.AppendChild (newElement);
		}

	}
		


	/// <summary>
	/// Loads XML data from a specified file
	/// </summary>
	/// <param name="filePath">The absolute or relative (i.e. relative to the project) path to a file</param>
	/// <exception cref="fnfException">Throws FileNotFound to caller if the file cannot be opened</exception>
	static void LoadXmlFromFile(string filePath) {
		
		XmlReader reader = null;
		try
		{
			Debug.Log(filePath);
			reader = XmlReader.Create(filePath);
			xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
		}
		finally
		{
			if (reader != null)
				reader.Close();
		}
	}

	/// <summary>
	/// Writes the built XML document to a file
	/// </summary>
	/// <param name="filePath">The file path to write to</param>
	/// <exception cref="FileNotFoundException">Thrown if the file cannot </exception>
	static void WriteXmlToFile(string filePath){

		XmlWriter writer = null;
		XmlWriterSettings settings = new XmlWriterSettings ();
		settings.Indent = true;
		try
		{
			writer = XmlWriter.Create(filePath, settings);
			xmlDocument.WriteTo(writer);
			writer.Flush();
		}
		finally
		{
			if (writer != null)
				writer.Close();
		}
		
	}


	/// <summary>
	/// Extracts the root element from the XML document that this writer will write to
	/// </summary>
	/// <returns>The root element.</returns>
	public static XmlElement GetRootElement(){
		return (XmlElement) xmlDocument.SelectSingleNode("/verticeMetadata");
	}

	/// <summary>
	/// Getter for the XML document that this class mutates
	/// </summary>
	/// <returns>The XML document</returns>
	public static XmlDocument GetDocument(){
		return xmlDocument;
	}

}