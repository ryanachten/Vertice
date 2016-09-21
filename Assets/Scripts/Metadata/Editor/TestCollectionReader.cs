using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class TestCollectionReader {

	[Test]
	/// <summary>
	/// Indirectly test that a specified XML file can be read in and assigned to the internal _xmlDocument property
	/// </summary>
	public void TestLazyInitialisation(){
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		CollectionReader.Refresh ();
	}

	[Test]
	[ExpectedException(typeof( FileNotFoundException ))]
	public void RefreshThrowsFileNotFoundIfXmlDocumentNotLoaded(){
		// Explicitly set _uri to null
		CollectionReader.LoadXml (null);
		CollectionReader.Refresh ();
	}

	[Test]
	public void TestGetIdentifiersForCollections() {
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		string[] collectionIdentifiers = CollectionReader.GetIdentifiersForCollections ();

		Assert.That (collectionIdentifiers[0] == "P14C3H01D3R-00");
		Assert.That (collectionIdentifiers[1] == "P14C3H01D3R-01");
		Assert.That (collectionIdentifiers[2] == "P14C3H01D3R-02");
		Assert.That (collectionIdentifiers[3] == "P14C3H01D3R-03");
		Assert.That (collectionIdentifiers[4] == "P14C3H01D3R-04");
		Assert.That (collectionIdentifiers[5] == "P14C3H01D3R-05");
	}

	[Test]
	public void GetCollectionMetadataWithIdentifier(string collectionIdentifier){
		Assert.Fail();
	}

	[Test]
	public void GetIdentifiersForArtefactsInCollectionWithIdentifier(string collectionIdentifier){
		Assert.Fail();
	}

	[Test]
	public void GetTransformForArtefactWithIdentifierInCollection(string artefactIdentifier, string collectionIdentifier) {
		Assert.Fail();
	}
}
