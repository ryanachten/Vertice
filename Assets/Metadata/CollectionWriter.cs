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

		PruneExistingCollectionWithIdentifier (collectionIdentifier); // Remove the collection node that will be overwritten
		XmlNode collectionNode = CreateCollectionNodeForCollectionWithIdentifier (collectionIdentifier); // Create a new empty <verticeCollection id="..."> node for this collection
		AddDescriptiveMetadataToCollectionNode (collectionNode, descriptiveMetadata); // Add in the descriptive metadata
		AddArtefactsToCollectionNode (collectionNode, artefactTransforms); // Add in the structural metadata
		WriteXmlToFile ();



	}

	/// <summary>
	/// Adds the passed in artefact to an existing collection in a Vertice XML and persists the data to the 
	/// collection XML file
	/// </summary>
	/// <param name="collectionIdentifier">The identifier for the collection in which to add the artefact</param>
	/// <param name="newArtefactIdentifier">The identifier for the new artefact (which should correspond with the artefact XML file)</param>
	/// <param name="newArtefactTransform">The VerticeTransform to use as the default coordinate for the new artefact</param>
	/// <exception cref="NoSuchCollectionException>If the collection identifier does not correspond to a collection in the XML file, NoSuchCollectionException is thrown and no data is written</exception>
	public static void AddArtefactToCollectionWithIdentifier(string collectionIdentifier, string newArtefactIdentifier, VerticeTransform newArtefactTransform) {

		if (_xmlDocument == null) {
			LoadXml ();
		}

		XmlNode collectionNode = _xmlDocument.SelectSingleNode (String.Format("/verticeCollections/verticeCollection[@id='{0}']", collectionIdentifier));
			
		if (collectionNode == null) {
			throw new NoSuchCollectionException ();
		} else {
			Dictionary<string, VerticeTransform> newArtefact = new Dictionary<string, VerticeTransform> ();
			newArtefact.Add (newArtefactIdentifier, newArtefactTransform);
			AddArtefactsToCollectionNode (collectionNode, newArtefact);
			WriteXmlToFile ();
		}
	}

	/// <summary>
	/// Creates the collection node for the collection to be written
	/// </summary>
	/// <returns>The new node</returns>
	/// <param name="collectionIdentifier">Collection identifier.</param>
	static XmlNode CreateCollectionNodeForCollectionWithIdentifier(string collectionIdentifier){
		XmlElement collectionRoot = _xmlDocument.CreateElement ("verticeCollection");
		collectionRoot.SetAttribute ("id", collectionIdentifier);
		Debug.Log (_xmlDocument.SelectSingleNode ("/verticeCollections"));
		return _xmlDocument.SelectSingleNode ("/verticeCollections").AppendChild (collectionRoot);
	}

	/// <summary>
	/// Adds descriptive metadata to the <descriptive> child of the main <verticeCollection id="..."> node
	/// </summary>
	/// <param name="collectionNode">The parent <verticeCollection id="..."> node</param>
	/// <param name="descriptiveMetadata">A Dictionary<string, string[]> mapping element names to lists of values</param>
	static void AddDescriptiveMetadataToCollectionNode(XmlNode collectionNode, Dictionary<string, string[]> descriptiveMetadata) {

		XmlElement descriptiveElement = _xmlDocument.CreateElement ("descriptive");
		XmlNode descriptiveNode = collectionNode.AppendChild (descriptiveElement);

		// Iterate through the elements and their values, and write that data in to <element>value</element> children of the <descriptive> node
		foreach (string key in descriptiveMetadata.Keys) {
			foreach (string value in descriptiveMetadata[key]) {
				XmlElement metadataElement = _xmlDocument.CreateElement (key);
				metadataElement.InnerText = value;
				descriptiveNode.AppendChild (metadataElement);

			}
		}
	}

	/// <summary>
	/// Adds the artefacts belonging to a collection and their transforms to the <structural> child of a <verticeCollection> element
	/// </summary>
	/// <param name="collectionNode">The <verticeCollection> node</param>
	/// <param name="artefactTransforms">A dictionary mapping artefact identifiers to their VerticeTransform transforms</param>
	static void AddArtefactsToCollectionNode(XmlNode collectionNode, Dictionary<string, VerticeTransform> artefactTransforms) {

		XmlNode structuralNode = collectionNode.SelectSingleNode ("structural");
		if (structuralNode == null) {
			XmlElement structuralElement = _xmlDocument.CreateElement ("structural");
			structuralNode = collectionNode.AppendChild (structuralElement);
		}

		// Iterate through each artefact and pull out its identifier and transform, then add 
		// a subtree to the <structural> element of the form:
		//		<artefact id="...">
		//			<transform>
		//				<position>
		//					<x>...</x>
		//					<y>...</y>
		//					<z>...</z>
		//				</position>
		//				<rotation>
		//					<x>...</x>
		//					<y>...</y>
		//					<z>...</z>
		//					<w>...</w>
		//				</rotation>
		//				<scale>
		//					<x>...</x>
		//					<y>...</y>
		//					<z>...</z>
		//				</scale>
		//			</transform>
		//		</artefact>
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

	/// <summary>
	/// Helper function for unpacking a Vector3 in to XML nodes
	/// </summary>
	/// <param name="node">The XML node to add this Vector3 to</param>
	/// <param name="vector">The Vector3 to unpack</param>
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

	/// <summary>
	/// Helper function for unpacking a Quaternion in to XML nodes
	/// </summary>
	/// <param name="node">The XML node to add this Quaternion to</param>
	/// <param name="vector">The Quaternion to unpack</param>
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
	/// Loads XML data from the designated (i.e either default, or via SetOutputFile()) XML file
	/// </summary>
	static void LoadXml() {

		XmlReader reader = null;
		try
		{
			reader = XmlReader.Create(Paths.CollectionMetadata);
			_xmlDocument = new XmlDocument();
			_xmlDocument.Load(reader);
		}
		catch (FileNotFoundException fnf) {
			Debug.Log ("File doesn't exist; creating an empty file");
			FileStream fs = File.Create (Paths.CollectionMetadata);
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
	public static void EstablishNewDocument(){
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
			writer = XmlWriter.Create(Paths.CollectionMetadata, settings);
			_xmlDocument.WriteTo(writer);
			writer.Flush();

		}
		finally
		{
			if (writer != null) {
				writer.Close ();
				CollectionReader.LoadXmlFromFile (Paths.CollectionMetadata);
			}
		}

	}


}
