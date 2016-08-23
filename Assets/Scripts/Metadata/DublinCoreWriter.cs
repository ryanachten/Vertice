using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class DublinCoreWriter {

	XmlDocument xmlDocument;

	/// <summary>
	/// Initializes a new instance of the <see cref="DublinCoreWriter"/> class. This writer will 
	/// update an existing XML file.
	/// </summary>
	/// <param name="data">A nested dictionary representing the structure of a metadata record. 
	/// 	{ 
	/// 		"descriptive" : {
	/// 			"title" : "A title",
	/// 			...
	/// 			}
	///			"structural" : {
	/// 			"identifier" : "12345.obj",
	/// 			...
	/// 			}
	/// 		}
	/// 	}
	/// 
	/// </param>
	/// <param name="metadataRecordPath">A path to an existing metadata record that should be updated</param>
	public DublinCoreWriter(Dictionary<string, System.Object> data, string metadataRecordPath){
		LoadXmlFromFile (metadataRecordPath);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DublinCoreWriter"/> class. This writer will create a new 
	/// XML file
	/// </summary>
	/// <param name="data">A nested dictionary representing the structure of a metadata record. 
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
	public DublinCoreWriter(Dictionary<string, object> data){
		xmlDocument = new XmlDocument();

		// Add the fundaments of an XML document: the xml processing instruction and a root element
		XmlProcessingInstruction declareVersionAndEncoding = xmlDocument.CreateProcessingInstruction ("xml", "version='1.0' encoding='UTF-8'");
		XmlElement rootElement = xmlDocument.CreateElement ("verticeMetadata");
		xmlDocument.AppendChild (declareVersionAndEncoding);
		xmlDocument.AppendChild (rootElement);

		if (data != null) {
			UnpackDictionaries (data, rootElement);
		}

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
			newElement.InnerText = elementName;
			parentElement.AppendChild (newElement);
		}

	}
		


	/// <summary>
	/// Loads XML data from a specified file
	/// </summary>
	/// <param name="filePath">The absolute or relative (i.e. relative to the project) path to a file</param>
	void LoadXmlFromFile(string filePath){
		
		XmlReader reader = null;
		try
		{
			
			reader = XmlReader.Create(filePath);
			xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
		}
		catch (FileNotFoundException fnfException) {
			Debug.Log (fnfException);
		}
		finally
		{
			if (reader != null)
				reader.Close();
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