using UnityEngine;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;

/// <summary>
/// The CollectionWriter is responsible for transcoding between a dictionary (for internal use) and XML (for persistent data storage) representation of 
/// metadata pertaining to Vertice Collections. The data has the following general format:
/// 
/// <verticeCollections>
///		<verticeCollection id="[String]">
///			<descriptive>
///				<title>[String]</title>
///				<creator>[String]</creator>
///				<date>[Formatted string, e.g. YYYY-MM-DD]</date>
///				<description>[String]</description>
///				<subject>[String]</subject>
///				<coverage>[String]</coverage>
/// 			...
///			</descriptive>
///			<structural>
///				<artefact id="[String]">
///					<transform>
///						<position>
///							<x>[Float]</x>
///							<y>[Float]</y>
///							<z>[Float]</z>
///						</position>
///						<rotation>
///							<x>[Float]</x>
///							<y>[Float]</y>
///							<z>[Float]</z>
///							<w>[Float]</w>
///						</rotation>
///						<scale>
///							<x>[Float]</x>
///							<y>[Float]y>
///							<z>[Float]</z>
///						</scale>
///					</transform>
///			</artefact>
///			<artefact id="[String]">
///				...
/// 		</artefact>
/// 	</verticeCollection>
/// 	...
/// </verticeCollections>
/// 
/// As such, a Collection is identified by a collection identifier, and comprises exactly one <descriptive> container consisting of Dublin Core elements,
/// and exactly one <structural> container consisting of one or more artefacts (identified by their artefact ID, which acts, essentially, as a foreign key that is consistent 
/// with the artefact XML file that the DublinCoreReader/Writer uses) and their transforms within a Collection scene.
/// 
/// </summary>
public static class CollectionWriter {

	private static string _xmlFilePath = Paths.CollectionMetadata;
	private static XmlDocument _xmlDocument;

	/// <summary>
	/// Writes the passed in collection information to the persistent XML file. The typical use case for this method will be via the Collection scene controller; in pseudocode:
	/// 
	/// string collectionId = "some-identifier"
	/// Dictionary<string, string[]> descriptiveMetadata = {title: "The title", creator: "The creator", ...}
	/// Dictionary<string, VerticeTransform> artefacts = {}
	/// for artefact in scene.artefacts
	/// 	transform = VerticeTransform(artefact.position, artefact.rotation, artefact.scale)
	/// 	artefacts.Add(artefact.identifier, transform)
	/// CollectionWriter.WriteCollectionWithIdentifier(collectionId, descriptiveMetadata, artefacts)
	/// 
	/// Note that the editing semantic will be 'overwrite'. That is, if a collection already exists with a given identifier, it will be removed from the XML file and replaced with this 
	/// new data.
	///
	/// </summary>
	/// <param name="collectionIdentifier">Collection identifier.</param>
	/// <param name="descriptiveMetadata">A dictionary of string-string[] pairs that map Dublin Core element names to an array of values that describe the collection as a whole</param>
	/// <param name="aretefactTransforms">A dictionary of string-VerticeTransform pairs that maps artefact identifiers to the transform information that gives their position in the Vertice collection scene</param>
	public static void WriteCollectionWithIdentifer(string collectionIdentifier, Dictionary<string, string[]> descriptiveMetadata, Dictionary<string, VerticeTransform> artefactTransforms) {

		if (_xmlDocument == null) {
			LoadXml ();
		}

		PruneExistingCollectionWithIdentifier (collectionIdentifier);
		XmlNode collectionNode = CreateCollectionNodeForCollectionWithIdentifier (collectionIdentifier);
		AddDescriptiveMetadataToCollectionNode (collectionNode, descriptiveMetadata);
		AddArtefactsToCollectionNode (collectionNode, artefactTransforms);
		WriteXmlToFile ();



	}

	static XmlNode CreateCollectionNodeForCollectionWithIdentifier(string collectionIdentifier){
		XmlElement collectionRoot = _xmlDocument.CreateElement ("verticeCollection");
		collectionRoot.SetAttribute ("id", collectionIdentifier);
		Debug.Log (_xmlDocument.SelectSingleNode ("/verticeCollections"));
		return _xmlDocument.SelectSingleNode ("/verticeCollections").AppendChild (collectionRoot);
	}

	static void AddDescriptiveMetadataToCollectionNode(XmlNode collectionNode, Dictionary<string, string[]> descriptiveMetadata) {

		XmlElement descriptiveElement = _xmlDocument.CreateElement ("descriptive");
		XmlNode descriptiveNode = collectionNode.AppendChild (descriptiveElement);

		foreach (string key in descriptiveMetadata.Keys) {
			foreach (string value in descriptiveMetadata[key]) {
				XmlElement metadataElement = _xmlDocument.CreateElement (key);
				metadataElement.InnerText = value;
				descriptiveNode.AppendChild (metadataElement);

			}
		}
	}

	static void AddArtefactsToCollectionNode(XmlNode collectionNode, Dictionary<string, VerticeTransform> artefactTransforms) {

		XmlElement structuralElement = _xmlDocument.CreateElement ("structural");
		XmlNode structuralNode = collectionNode.AppendChild (structuralElement);

		foreach (string key in artefactTransforms.Keys) {
			XmlElement artefactElement = _xmlDocument.CreateElement ("artefact");
			artefactElement.SetAttribute ("id", key);
			XmlElement transformElement = _xmlDocument.CreateElement ("transform");
			XmlNode artefactNode = structuralNode.AppendChild (artefactElement);

			VerticeTransform transform = artefactTransforms [key];

			XmlElement positionElement = _xmlDocument.CreateElement ("position");
			XmlElement rotationElement = _xmlDocument.CreateElement ("rotation");
			XmlElement scaleElement = _xmlDocument.CreateElement ("scale");

			addVector3ToNode (positionElement, artefactTransforms [key].position);
			addQuaternionToNode (rotationElement, artefactTransforms [key].rotation);
			addVector3ToNode (scaleElement, artefactTransforms [key].scale);

			transformElement.AppendChild (positionElement);
			transformElement.AppendChild (rotationElement);
			transformElement.AppendChild (scaleElement);

			artefactNode.AppendChild (transformElement);
			structuralNode.AppendChild (artefactNode);
		}
		
	}

	static void addVector3ToNode(XmlNode node, Vector3 vector) {

		XmlElement x = _xmlDocument.CreateElement ("x");
		XmlElement y = _xmlDocument.CreateElement ("y");
		XmlElement z = _xmlDocument.CreateElement ("z");

		x.InnerText = Convert.ToString(vector.x);
		y.InnerText = Convert.ToString(vector.y);
		z.InnerText = Convert.ToString(vector.z);

		node.AppendChild (x);
		node.AppendChild (y);
		node.AppendChild (z);
	
	}

	static void addQuaternionToNode (XmlNode node, Quaternion vector) {

		XmlElement x = _xmlDocument.CreateElement ("x");
		XmlElement y = _xmlDocument.CreateElement ("y");
		XmlElement z = _xmlDocument.CreateElement ("z");
		XmlElement w = _xmlDocument.CreateElement ("w");

		x.InnerText = Convert.ToString(vector.x);
		y.InnerText = Convert.ToString(vector.y);
		z.InnerText = Convert.ToString(vector.z);
		w.InnerText = Convert.ToString(vector.w);

		node.AppendChild (x);
		node.AppendChild (y);
		node.AppendChild (z);
		node.AppendChild (w);
		
	}

	/// <summary>
	/// If the collection to be written already exists in the XML file it is removed in order to preserve overwrite semantics
	/// </summary>
	/// <param name="collectionIdentifier">The identifier of the collection to search for and remove</param>
	static void PruneExistingCollectionWithIdentifier(string collectionIdentifier) {
		XmlNode collectionRoot = _xmlDocument.SelectSingleNode (String.Format ("/verticeCollections/verticeCollection[@id='{0}']", collectionIdentifier));
		if (collectionRoot != null) {
			_xmlDocument.SelectSingleNode ("/verticeCollections").RemoveChild (collectionRoot);
		}
	}

	/// <summary>
	/// Sets the output file path
	/// </summary>
	/// <param name="newPath">New path.</param>
	public static void SetOutputFile(string newPath) {
		_xmlFilePath = newPath;
	}

	/// <summary>
	/// Loads XML data from the designated (i.e either default, or via SetOutputFile()) XML file
	/// </summary>
	static void LoadXml() {

		XmlReader reader = null;
		try
		{
			reader = XmlReader.Create(_xmlFilePath);
			_xmlDocument = new XmlDocument();
			_xmlDocument.Load(reader);
		}
		catch (FileNotFoundException fnf) {
			Debug.Log ("File doesn't exist; creating an empty file");
			FileStream fs = File.Create (_xmlFilePath);
			EstablishNewDocument ();
			fs.Close ();
		}
		finally
		{
			if (reader != null)
				reader.Close();
		}
	}

	/// <summary>
	/// Creates a new document with a <verticeMetadata> root node and XML preamble
	/// </summary>
	static void EstablishNewDocument(){
		_xmlDocument = new XmlDocument ();
		// Add the fundaments of an XML document: the xml processing instruction and a root element
		XmlProcessingInstruction declareVersionAndEncoding = _xmlDocument.CreateProcessingInstruction ("xml", "version='1.0' encoding='UTF-8'");
		XmlElement rootElement = _xmlDocument.CreateElement ("verticeCollections");
		_xmlDocument.AppendChild (declareVersionAndEncoding);
		_xmlDocument.AppendChild (rootElement);
	}

	/// <summary>
	/// Writes the built XML document to a file
	/// </summary>
	/// <param name="filePath">The file path to write to</param>
	static void WriteXmlToFile(){

		XmlWriter writer = null;
		XmlWriterSettings settings = new XmlWriterSettings ();
		settings.Indent = true;
		try
		{
			writer = XmlWriter.Create(_xmlFilePath, settings);
			_xmlDocument.WriteTo(writer);
			writer.Flush();
		}
		finally
		{
			if (writer != null)
				writer.Close();
		}

	}


}
