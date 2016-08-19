using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class DublinCoreWriter : MonoBehaviour {

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
	public DublinCoreWriter(Dictionary<string, object> data, string metadataRecordPath){
		LoadXmlFromFile (metadataRecordPath);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DublinCoreWriter"/> class. This writer will create a new 
	/// XML file
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
	public DublinCoreWriter(Dictionary<string, object> data){
		xmlDocument = new XmlDocument();

		// Add the fundaments of an XML document: the xml processing instruction and a root element
		XmlProcessingInstruction declareVersionAndEncoding = xmlDocument.CreateProcessingInstruction ("xml", "version='1.0' encoding='UTF-8'");
		XmlElement rootElement = xmlDocument.CreateElement ("verticeMetadata");
		xmlDocument.AppendChild (declareVersionAndEncoding);
		xmlDocument.AppendChild (rootElement);
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
	public XmlNode GetRootElement(){
		return xmlDocument.SelectSingleNode("/verticeMetadata");
	}
}