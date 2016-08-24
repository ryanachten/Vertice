using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

public class DublinCoreWriter {

	XmlDocument xmlDocument;

	/// <summary>
	/// Initializes a new instance of the <see cref="DublinCoreWriter"/> class. This writer will create a new 
	/// XML file in the case where no file exists
	/// </summary>
	/// <param name="data">A nested dictionary representing the structure of a metadata record. 
	/// <param name="metadataRecordPath">The path to a metadata file. If no path is provided, and new 
	/// file will be created at Application.persistentDataPath</param>
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
	public DublinCoreWriter(Dictionary<string, object> data, string artefactIdentifier, string metadataRecordPath = null){

		try {
			LoadXmlFromFile (metadataRecordPath);
		} catch (FileNotFoundException fnfException) {
			Debug.Log (String.Format("Could not open an XML file at {0} -- will create new metadata file at that path", metadataRecordPath));
			EstablishNewDocument ();
		} catch (DirectoryNotFoundException dirException) {
			Debug.Log (String.Format ("Could not open directory\n\n {0}", dirException.Message));
			EstablishNewDocument ();
		} catch (ArgumentNullException nullException){
			Debug.Log (String.Format("Could not open an XML file at {0} -- will create new metadata file", metadataRecordPath));
			metadataRecordPath = String.Format ("{0}/{1}", Application.dataPath, "vertice_metadata.xml");
			EstablishNewDocument ();
		}

		try {
			UnpackDictionaries (data, GetArtefactRoot(artefactIdentifier));
			WriteXmlToFile(metadataRecordPath);
			Debug.Log(String.Format("Wrote metadata to {0}", metadataRecordPath));
		} catch (NullReferenceException nullReference) {
			Debug.LogError ("An error occurred converting a nested dictionary to XML -- you must pass in a nested dictionary (not null!)");
		} catch (ArgumentNullException nullArgument){
			Debug.LogError (String.Format("An error occurred writing the file to XML -- it's likely that no filePath was passed. Exception message:\n\n\n{0}", nullArgument.Message));
		}

	}

	/// <summary>
	/// Creates a new document with a <verticeMetadata> root node and XML preamble
	/// </summary>
	void EstablishNewDocument(){
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
	void UnpackDictionaries(Dictionary<string, object> metadataDictionary, XmlElement parentElement)	{


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
	
	}

	/// <summary>
	/// Gets the root node for an artefact record, given its unique identifier
	/// </summary>
	/// <returns>An empty XmlElement (with an id attribute representing the identifier for the artefact) and no child elements.</returns>
	/// <param name="identifier">The identifier for the artefact</param>
	XmlElement GetArtefactRoot(string identifier){
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
	void UnpackList(string elementName, string[] elementValues, XmlElement parentElement){
		Debug.Log ("Unpacking list: " + elementName);
		foreach (string value in elementValues) {
			Debug.Log ("Adding " + elementName + " node to " + parentElement.LocalName + " with value " + value);
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
	void LoadXmlFromFile(string filePath) {
		
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
	void WriteXmlToFile(string filePath){

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
	public XmlElement GetRootElement(){
		return (XmlElement) xmlDocument.SelectSingleNode("/verticeMetadata");
	}

	/// <summary>
	/// Getter for the XML document that this class mutates
	/// </summary>
	/// <returns>The XML document</returns>
	public XmlDocument GetDocument(){
		return xmlDocument;
	}

}