using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class NoSuchArtefactException : Exception
{
	public NoSuchArtefactException ()
	{}

	public NoSuchArtefactException (string message) 
		: base(message)
	{}

	public NoSuchArtefactException (string message, Exception innerException)
		: base (message, innerException)
	{}    
}

[Serializable]
public class NoModelInformationException : Exception
{
	public NoModelInformationException ()
	{}

	public NoModelInformationException (string message) 
		: base(message)
	{}

	public NoModelInformationException (string message, Exception innerException)
		: base (message, innerException)
	{}    
}

[Serializable]
public class NoContextualMediaException : Exception
{
	public NoContextualMediaException ()
	{}

	public NoContextualMediaException (string message) 
		: base(message)
	{}

	public NoContextualMediaException (string message, Exception innerException)
		: base (message, innerException)
	{}    
}

/// <summary>
/// The DublinCoreReader reads in XML data from a specified file. The pattern of 
/// use for this class is as follows:
/// 
/// DublinCoreReader.LoadXml("https://www.example.com/metadata/metadata_file.xml"); // You MUST associate a metadata file before doing ANYTHING else
/// ...
/// ...
/// DublinCoreReader.Refresh() // Call this if you want to check that the metadata file is up to date
/// </summary>
public static class DublinCoreReader {

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
	/// Synchronise the system's copy of the artefact library with the XML file stored on disk
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
	/// <returns>The XML document representing artefacts in the system</returns>
	static XmlDocument Xml(){
		if (_xmlDocument == null) {
			Refresh ();
		}
		return _xmlDocument;
	}

	/// <summary>
	/// Takes a subtree of the DublinCore XML document and converts it to a Dictionary representation. Subtrees must be of 
	/// the form (e.g. for the <descriptive> subtree)
	/// 
	/// <subtree-root>
	/// 	<child>
	/// 	<child>
	/// 	...
	/// </subtree-root>
	/// 
	/// </summary>
	/// <returns>A Dictionary<string, string[]> mapping DublinCore element names to their (possibly repeated) values</returns>
	/// <param name="subtree">A subtree of the XML document.</param>
	static Dictionary<string, string[]> UnpackSubtree(XmlNode subtree){
		Dictionary<string, List<string>> subtreeDictionary = new Dictionary<string, List<string>>();
		Dictionary<string, string[]> retVal = new Dictionary<string, string[]> ();

		// If there is no data to unpack, return an empty Dictionary<string, string[]>
		if (subtree == null) {
			return retVal;
		}

		// Iterate through children of the subtree and add 'leaves' to subtreeDictionary
		foreach (XmlNode node in subtree.ChildNodes) {

			// If this element is already represented in the dictionary, add its value to the array, 
			// otherwise, create a new string array and add it to the dictionary using the element 
			// name as a key
			if (subtreeDictionary.ContainsKey (node.LocalName)) {
				subtreeDictionary [node.LocalName].Add (node.InnerText);
			} else {
				subtreeDictionary [node.LocalName] = new List<string> ();
				subtreeDictionary [node.LocalName].Add (node.InnerText);
			}
		}

		// Convert the Dictionary<string, List<string>> to a Dictionary<string, string[]>
		foreach (string key in subtreeDictionary.Keys){
			retVal [key] = subtreeDictionary [key].ToArray ();
		}

		return retVal;
	}

	/// <summary>
	/// Queries the XML file containing metadata for artefacts represented in the system and retrieves information 
	/// for the artefact with a given identifier
	/// </summary>
	/// <returns>A nested dictionary representation of the data. Data -- if it exists -- can be accessed using 
	/// a combination of dictionary and array accessors: data["descriptive"]["title"][0] // returns the first 
	/// title (or throws an exception if no <title> elements existed.
	/// </returns>
	/// <param name="identifier">The identifier of the artefact to retrieve</param>
	/// <exception cref="NoSuchArtefactException">Throws NoSuchArtefactException if the artefact cannot be found</exception>
	public static Dictionary<string, Dictionary<string, string[]>> GetArtefactWithIdentifier(string identifier){
		Dictionary<string, Dictionary<string, string[]>> retVal = new Dictionary<string, Dictionary<string, string[]>> ();
		XmlNode artefact = Xml().SelectSingleNode(String.Format("//artefact[@id='{0}']", identifier));

		// If there is no match on the identifier, return an empty dictionary
		if (artefact == null) {
			throw new NoSuchArtefactException (String.Format ("Artefact with identifier {0} does not exist", identifier));
		}
			
		XmlNode descriptive = artefact.SelectSingleNode ("./descriptive");
		XmlNode structural = artefact.SelectSingleNode ("./structural");

		retVal ["descriptive"] = UnpackSubtree (descriptive);
		retVal ["structural"] = UnpackSubtree (structural);

		return retVal;
	}

	/// <summary>
	/// Gets the mesh location for an artefact with a given identifier.
	/// </summary>
	/// <returns>A string representing the relative path to the identifier. This path should be prepended 
	/// with the appropriate base path</returns>
	/// <param name="identifier">The identifier of the artefact</param>
	/// <exception cref="NoSuchArtefactException">Throws NoSuchArtefactException if the artefact cannot be found</exception>
	/// <exception cref="NoModelInformationException">Thrown if the artefact is not associated with any model information</exception>
	public static string GetMeshLocationForArtefactWithIdentifier(string identifier){
		XmlNode artefact = Xml().SelectSingleNode(String.Format("//artefact[@id='{0}']", identifier));
		if (artefact == null) {
			throw new NoSuchArtefactException (String.Format ("Artefact with identifier {0} does not exist", identifier));
		}

		XmlNode meshLocationNode = artefact.SelectSingleNode(String.Format("./relatedAssets/MeshLocation"));
		if (meshLocationNode == null) {
			throw new NoModelInformationException (String.Format ("Artefact with identifier {0} is not associated with a model", identifier));
		}

		return meshLocationNode.InnerText;
	}

	/// <summary>
	/// Gets the texture location for an artefact with a given identifier.
	/// </summary>
	/// <returns>A string representing the relative path to the identifier. This path should be prepended 
	/// with the appropriate base path</returns>
	/// <param name="identifier">The identifier of the artefact</param>
	/// <exception cref="NoSuchArtefactException">Throws NoSuchArtefactException if the artefact cannot be found</exception>
	/// <exception cref="NoModelInformationException">Thrown if the artefact is not associated with any model information</exception>
	public static string GetTextureLocationForArtefactWithIdentifier(string identifier){
		XmlNode artefact = Xml().SelectSingleNode(String.Format("//artefact[@id='{0}']", identifier));
		if (artefact == null) {
			throw new NoSuchArtefactException (String.Format ("Artefact with identifier {0} does not exist", identifier));
		}

		XmlNode texLocationNode = artefact.SelectSingleNode("./relatedAssets/TexLocation");
		if (texLocationNode == null) {
			throw new NoModelInformationException (String.Format ("Artefact with identifier {0} is not associated with a model", identifier));
		}

		return texLocationNode.InnerText;
	}

	/// <summary>
	/// Gets the contextual media for an artefact with a given identifier.
	/// </summary>
	/// <returns>An array of dictionaries, each comprising key-value pairs pertaining the contextual media. E.g
	/// 
	/// // Access the media location for the first contextual media asset associated with artefact having identifier the-identifier
	/// Dictionary<string, string>[] contextInformation = DublinCoreReader.GetContextualMediaForArtefactWithIdentifier("the-identifier");
	/// string mediaLocation = contextInformation[0]["MediaLocation"];  
	/// </returns>
	/// <param name="identifier">The identifier of the artefact</param>
	public static Dictionary<string, string>[] GetContextualMediaForArtefactWithIdentifier(string identifier){
		XmlNode artefact = Xml().SelectSingleNode(String.Format("//artefact[@id='{0}']", identifier));
		if (artefact == null) {
			throw new NoSuchArtefactException (String.Format ("Artefact with identifier {0} does not exist", identifier));
		}

		XmlNodeList contextualMediaNodes = artefact.SelectNodes("./relatedAssets/ContextMedia");
		if (contextualMediaNodes == null) {
			throw new NoContextualMediaException (String.Format ("Artefact with identifier {0} is not associated with any contextual information", identifier));
		}

		Dictionary<string, string>[] retVal = new Dictionary<string, string>[contextualMediaNodes.Count];
		for (int i = 0; i < contextualMediaNodes.Count; i++) {
			Dictionary<string, string> contextInformation = new Dictionary<string, string> ();
			foreach (XmlNode field in contextualMediaNodes[i]) {
				Debug.Log(String.Format("Trying to add an element to slot {0}", i));
				Debug.Log (String.Format ("Trying to do something with field: {0} having value: {1}", field.LocalName, field.InnerText));
				contextInformation.Add(field.LocalName, field.InnerText);
			}
			retVal [i] = contextInformation;
		}
		return retVal;
	}

	/// <summary>
	/// Gets contextual media of a specified type for an artefact with a given identifier
	/// </summary>
	/// <returns>An array of dictionaries, each comprising key-value pairs pertaining the contextual media. E.g
	/// 
	/// // Access the media location for the first contextual media asset associated with artefact having identifier the-identifier
	/// Dictionary<string, string>[] contextInformation = DublinCoreReader.GetContextualMediaForArtefactWithIdentifier("the-identifier");
	/// string mediaLocation = contextInformation[0]["MediaLocation"];  
	/// </returns>
	/// <param name="identifier">The identifier of the artefact</param>
	/// <param name="type">The media type (e.g. Image, Audio, Video) to filter on</param>
	public static Dictionary<string, string>[] GetContextualMediaArtefactWithIdentifierAndType(string identifier, string type){
		XmlNode artefact = Xml().SelectSingleNode(String.Format("//artefact[@id='{0}']", identifier));
		if (artefact == null) {
			throw new NoSuchArtefactException (String.Format ("Artefact with identifier {0} does not exist", identifier));
		}

		XmlNodeList contextualMediaNodes = artefact.SelectNodes(String.Format("./relatedAssets/ContextMedia/MediaType[text()='{0}']/parent::ContextMedia", type));
		if (contextualMediaNodes == null || contextualMediaNodes.Count == 0) {
			throw new NoContextualMediaException (String.Format ("Artefact with identifier {0} is not associated with any contextual information of type {1}", identifier, type));
		}

		Dictionary<string, string>[] retVal = new Dictionary<string, string>[contextualMediaNodes.Count];
		Debug.Log (String.Format ("retVal has {0} slots", retVal.Length));
		for (int i = 0; i < contextualMediaNodes.Count; i++) {
			Dictionary<string, string> contextInformation = new Dictionary<string, string> ();
			foreach (XmlNode field in contextualMediaNodes[i]) {
				Debug.Log(String.Format("Trying to add an element to slot {0}", i));
				Debug.Log (String.Format ("Trying to do something with field: {0} having value: {1}", field.LocalName, field.InnerText));
				contextInformation.Add(field.LocalName, field.InnerText);
			}
			retVal [i] = contextInformation;
		}
		return retVal;
	}

	private static string[] nodeListToSortedArrayWithSetSemantics(XmlNodeList nodes){
		HashSet<string> returnSet = new HashSet<string>();
		foreach (XmlNode node in nodes) {
			returnSet.Add (node.InnerXml);
		}

		string[] returnArray = new string[returnSet.Count];
		returnSet.CopyTo (returnArray);
		Array.Sort (returnArray);
		return returnArray;
	}

	private static string[] GetIdentifiersForValuesInElement(string[] values, string dcElement){
		HashSet<string> identifierSet = new HashSet<string> ();
		string xPath = String.Format ("/verticeMetadata/artefact/descriptive/{0}", dcElement);
		XmlNodeList nodes = Xml ().SelectNodes (xPath);
		foreach (XmlNode node in nodes) {
			if (Array.Exists (values, element => element == node.InnerXml)) {
				identifierSet.Add(node.SelectSingleNode ("../../@id").InnerXml);
			}
		}
		string[] returnArray = new string[identifierSet.Count];
		identifierSet.CopyTo (returnArray);
		Array.Sort (returnArray);
		return returnArray;
	}

	public static string[] GetValuesForCreator(){
		XmlNodeList results = Xml ().SelectNodes ("/verticeMetadata/artefact/descriptive/creator");
		return nodeListToSortedArrayWithSetSemantics (results);
	}

	public static string[] GetIdentifiersForCreators(string[] creatorNames){
		return GetIdentifiersForValuesInElement (creatorNames, "creator");
		
	}

	public static string[] GetValuesForContributor(){
		XmlNodeList results = Xml ().SelectNodes ("/verticeMetadata/artefact/descriptive/contributor");
		return nodeListToSortedArrayWithSetSemantics (results);
	}

	public static string[] GetIdentifiersForContributors(string[] contributorNames){
		return GetIdentifiersForValuesInElement (contributorNames, "contributor");
	}

	public static string[] GetIdentifiersForDateRange(DateTime start, DateTime end){
		HashSet<string> identifiers = new HashSet<string> ();
		XmlNodeList dates = Xml ().SelectNodes ("/verticeMetadata/artefact/descriptive/date");
		foreach (XmlNode date in dates) {
			DateTime artefactDateTime = DateTime.Parse (date.InnerXml);
			if (artefactDateTime >= start && artefactDateTime <= end) {
				identifiers.Add (date.SelectSingleNode ("../../@id").InnerXml);
			}
		}
		string[] returnArray = new string[identifiers.Count];
		identifiers.CopyTo (returnArray);
		Array.Sort (returnArray);
		return returnArray;
	}

	public static string[] GetAllYears(bool asc = false){
		XmlNodeList results = Xml ().SelectNodes ("/verticeMetadata/artefact/descriptive/date");
		HashSet<string> years = new HashSet<string> ();
		foreach (XmlNode node in results){
			years.Add(DateTime.Parse(node.InnerXml).Year.ToString());
		}
		string[] yearsArray = new string[years.Count];
		years.CopyTo (yearsArray);
		Array.Sort (yearsArray);
		if (!asc){
			Array.Reverse(yearsArray);
		}

		return yearsArray;

	}

	public static string[] GetIdentifiersForYears(string[] years){
		List<string> returnList = new List<string>();
		foreach (string year in years){
			DateTime firstDayOfYear = new DateTime(int.Parse(year), 1, 1);
			DateTime lastDayOfYear = new DateTime(int.Parse(year), 12, 31);
			returnList.AddRange(GetIdentifiersForDateRange(firstDayOfYear, lastDayOfYear));
		}
		string[] returnArray = new string[returnList.Count];
		returnList.CopyTo (returnArray);
		Array.Sort (returnArray);
		return returnArray;

	}

	public static string[] GetValuesForSubject(){
		XmlNodeList results = Xml ().SelectNodes ("/verticeMetadata/artefact/descriptive/subject");
		return nodeListToSortedArrayWithSetSemantics (results);
	}

	public static string[] GetIdentifiersForSubjects(string[] subjects){
		return GetIdentifiersForValuesInElement (subjects, "subject");
	}

}