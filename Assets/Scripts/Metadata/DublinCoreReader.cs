using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

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

	public static Dictionary<string, Dictionary<string, string[]>> GetArtefactWithIdentifier(string identifier){
		Dictionary<string, Dictionary<string, string[]>> retVal = new Dictionary<string, Dictionary<string, string[]>> ();
//		XmlNode artefact = Xml().SelectSingleNode(String.Format("//artefact[@id='{0}'", identifier));
//		XmlNode descriptive = artefact.SelectSingleNode ("/descriptive");
//		XmlNode structural = artefact.SelectSingleNode ("/structural");

		// RETURN DUMMY DATA
		retVal.Add("descriptive", new Dictionary<string, string[]>());
		retVal.Add("structural", new Dictionary<string, string[]>());
		retVal["descriptive"].Add ("title", new string[] {
			"Another title for another original resource",
			"Another title for that original resource, maybe in some other language?",
			"Yet another title, for that original resource"
		});
		retVal["descriptive"].Add ("creator", new string[] {"Some Guy", "Some Guy's Friend"});
		retVal["descriptive"].Add ("description", new string[] {"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce blandit blandit massa vitae ultrices. Fusce ligula augue, ullamcorper non bibendum in, sollicitudin et risus. Maecenas eget elementum massa, vitae sagittis urna. Mauris vestibulum erat et volutpat fermentum. Nulla rutrum nisi enim, eget rhoncus ante condimentum ac. Phasellus nec eleifend eros. Vestibulum vitae molestie nisl, vitae porttitor mi. Pellentesque convallis justo a suscipit scelerisque. Pellentesque in luctus turpis, eu pretium nunc. Vivamus libero massa, ultrices in tempor ac, varius a diam. Sed nec urna quis nisi viverra volutpat sed quis arcu. Donec pharetra ex vel fringilla viverra. Cras quis congue mi."});
		retVal["descriptive"].Add ("format", new string[] { "Statue" });
		retVal["descriptive"].Add ("identifier", new string[] {"ID1234/19/A"});
		retVal["descriptive"].Add ("relation", new string[] {"http://themuseum.org/catalogue/ID123419A"});


		retVal["structural"].Add ("creator", new string[] { "Digitisation Guy" });
		retVal["structural"].Add ("created", new string[] { "2015-01-25" });
		retVal["structural"].Add ("identifier", new string[] { "ID1234.19.A.obj", "ID1234.19.A.png" });
		retVal["structural"].Add ("isVersionOf", new string[] { "http://themuseum.org/repository/ID123419A.zip" });

		return retVal;


	}
}