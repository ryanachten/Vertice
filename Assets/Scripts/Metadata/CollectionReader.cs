using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

public static class CollectionReader {

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
	/// Synchronise the system's copy of the collection library with the XML file stored on disk
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
	/// <returns>The XML document representing collections in the system</returns>
	static XmlDocument Xml(){
		if (_xmlDocument == null) {
			Refresh ();
		}
		return _xmlDocument;
	}

	public static string[] GetIdentifiersForCollections() {
		XmlNodeList collections = Xml().SelectNodes("//verticeCollection/@id");
		string[] collectionIdentifiers = new string[collections.Count];

		for (int i = 0; i < collections.Count; i++) {
			collectionIdentifiers [i] = collections [i].InnerXml;
		}

		return collectionIdentifiers;
	}

	public static Dictionary<string, Dictionary<string, string[]>> GetCollectionMetadataWithIdentifier(string collectionIdentifier){
		return null;
	}

	public static string[] GetIdentifiersForArtefactsInCollectionWithIdentifier(string collectionIdentifier){
		return null;
	}

	public static Dictionary<string, Dictionary<string, float>> GetTransformForArtefactWithIdentifierInCollection(string artefactIdentifier, string collectionIdentifier) {
		return null;
	}
}
