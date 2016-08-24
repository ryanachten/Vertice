﻿using UnityEngine;
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

	public static string GetMeshLocationForArtefactWithIdentifier(string identifier){
		return "";
	}

	public static string GetTexLocationForArtefactWithIdentifier(string identifier){
		return "";
	}

	public static Dictionary<string, Dictionary<string, string>>[] GetContextualMediaForArtefactWithIdentifier(string identifier){
		return new Dictionary<string, Dictionary<string, string>>[]{};
	}

	public static Dictionary<string, Dictionary<string, string>>[] GetContextualMediaOfTypeForArtefactWithIdentifier(string type, string identifier){
		return new Dictionary<string, Dictionary<string, string>>[]{};
	}
}