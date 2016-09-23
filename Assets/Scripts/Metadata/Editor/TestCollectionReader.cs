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
	public void GetCollectionMetadataWithIdentifier(){
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		string[] collectionIdentifiers = CollectionReader.GetIdentifiersForCollections ();
		Dictionary<string, string[]> collectionMetadata = CollectionReader.GetCollectionMetadataWithIdentifier (collectionIdentifiers [0]);

		Assert.That (collectionMetadata ["identifier"] [0] == collectionIdentifiers [0]);
		Assert.That (collectionMetadata ["title"][0] == "Photogrammetry Test Scans");
		Assert.That (collectionMetadata ["creator"][0] == "Ryan Achten");
		Assert.That (collectionMetadata ["date"][0] == "29/11/2015");
		Assert.That (collectionMetadata ["description"][0] == "A museum is distinguished by a collection of often unique objects that forms the core of its activities for exhibitions, education, research, etc.");
		Assert.That (collectionMetadata ["subject"][0] == "Photogrammetry");
		Assert.That (collectionMetadata ["coverage"] [0] == "Evan's Bay");
		Assert.That (collectionMetadata ["coverage"] [1] == "Basin Reserve");
		Assert.That (collectionMetadata ["extent"] [0] == "5");

	}

	[Test]
	public void GetIdentifiersForArtefactsInCollectionWithIdentifier(){
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		string[] collectionIdentifiers = CollectionReader.GetIdentifiersForCollections ();
		string[] artefactIdentifiers = CollectionReader.GetIdentifiersForArtefactsInCollectionWithIdentifier(collectionIdentifiers[0]);

		Assert.That (artefactIdentifiers[0] == "Evans Bay Wharf");
		Assert.That (artefactIdentifiers[1] == "Cog Wheel Evans Bay");
		Assert.That (artefactIdentifiers[2] == "Evans Boat House");
		Assert.That (artefactIdentifiers[3] == "Cricket Monument");
		Assert.That (artefactIdentifiers[4] == "Doll Head");
	}

	[Test]
	[ExpectedException(typeof( NoSuchCollectionException ))]
	public void GetIdentifiersForArtefactsInCollectionWithIdentifier_Invalid(){
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		string[] artefactIdentifiers = CollectionReader.GetIdentifiersForArtefactsInCollectionWithIdentifier("THIS IS NOT A REAL COLLECTION ID");
	}

	[Test]
	public void GetTransformForArtefactWithIdentifierInCollection() {
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection("P14C3H01D3R-00", "Evans Bay Wharf");

		Assert.That (transformData ["position"] ["x"] == 40.01599f);
		Assert.That (transformData ["position"] ["y"] == -11.58916f);
		Assert.That (transformData ["position"] ["z"] == 184.2516f);

		Assert.That (transformData ["rotation"] ["x"] == 1.0f);
		Assert.That (transformData ["rotation"] ["y"] == 1.0f);
		Assert.That (transformData ["rotation"] ["z"] == 1.0f);
		Assert.That (transformData ["rotation"] ["w"] == 1.0f);

		Assert.That (transformData ["scale"] ["x"] == 1.0f);
		Assert.That (transformData ["scale"] ["y"] == 1.0f);
		Assert.That (transformData ["scale"] ["z"] == 1.0f);
	}

	[Test]
	[ExpectedException(typeof( MalformedTransformCoordinateException ))]
	public void GetTransformForArtefactWithIdentifierInCollection_IncompleteTransform() {
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection("P14C3H01D3R-00", "Cog Wheel Evans Bay");
	}

	[Test]
	[ExpectedException(typeof( NoSuchArtefactInCollectionException ))]
	public void GetTransformForArtefactWithIdentifierInCollection_InvalidArtefact() {
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection("P14C3H01D3R-00", "NO SUCH ARTEFACT");
	}
}
