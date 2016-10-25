using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class TestCollectionWriter {

	[SetUp]
	public void Setup() {
		Paths.CollectionMetadata = Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml";
	}

	[TearDown]
	public void TearDown() {
		File.Delete (Paths.CollectionMetadata);
	}

	[Test]
	public void TestWriteCollectionData_OneToOne() {
		// Test that a collection whose descriptive elements are non-repeating write correctly

		Dictionary<string, string[]> descriptiveData = new Dictionary<string, string[]>();
		Dictionary<string, VerticeTransform> structuralData = new Dictionary<string, VerticeTransform>();

		descriptiveData.Add("title", new string[] {"Test Title"});
		descriptiveData.Add("creator", new string[] {"Test Creator"});
		descriptiveData.Add("date", new string[] {"2016-10-20"});

		structuralData.Add("TEST-ARTEFACT-01", new VerticeTransform(Vector3.one, Quaternion.identity, Vector3.one));

		CollectionWriter.WriteCollectionWithIdentifer("TEST-COLLECTION-01", descriptiveData, structuralData);

		CollectionReader.LoadXmlFromFile (Paths.CollectionMetadata);
		Dictionary<string, string[]> collectionMetadata = CollectionReader.GetCollectionMetadataWithIdentifier ("TEST-COLLECTION-01");
		Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection ("TEST-COLLECTION-01", "TEST-ARTEFACT-01");

		Assert.That (collectionMetadata ["title"] [0] == "Test Title");
		Assert.That (collectionMetadata ["creator"] [0] == "Test Creator");
		Assert.That (collectionMetadata ["date"] [0] == "2016-10-20");

		Assert.That (transformData ["position"] ["x"] == Vector3.one.x);
		Assert.That (transformData ["position"] ["y"] == Vector3.one.y);
		Assert.That (transformData ["position"] ["z"] == Vector3.one.z);

		Assert.That (transformData ["rotation"] ["x"] == Quaternion.identity.x);
		Assert.That (transformData ["rotation"] ["y"] == Quaternion.identity.y);
		Assert.That (transformData ["rotation"] ["z"] == Quaternion.identity.z);
		Assert.That (transformData ["rotation"] ["w"] == Quaternion.identity.w);

		Assert.That (transformData ["scale"] ["x"] == Vector3.one.x);
		Assert.That (transformData ["scale"] ["y"] == Vector3.one.y);
		Assert.That (transformData ["scale"] ["z"] == Vector3.one.z);
	}

	[Test]
	public void TestWriteCollectionData_ManyCollectionsManyArtefacts(){
		// Test the typical use case for the CollectionWriter

		CreateTwoCollectionsWithTwoArtefacts ();
		CollectionReader.LoadXmlFromFile (Paths.CollectionMetadata);


		for (int i = 0; i < 2; i++) {
			
			Dictionary<string, string[]> collectionMetadata = CollectionReader.GetCollectionMetadataWithIdentifier (String.Format("TEST-COLLECTION-0{0}", i));
			Assert.That (collectionMetadata ["title"] [0] == "Test Title");
			Assert.That (collectionMetadata ["title"] [1] == "A second title");
			Assert.That (collectionMetadata ["creator"] [0] == "Test Creator");
			Assert.That (collectionMetadata ["creator"] [1] == "A co-creator");
			Assert.That (collectionMetadata ["date"] [0] == "2016-10-20");

			for (int j = 0; j < 2; j++) {
				Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection (String.Format("TEST-COLLECTION-0{0}", i), String.Format("TEST-ARTEFACT-0{0}", j));
				Assert.That (transformData ["position"] ["x"] == Vector3.one.x);
				Assert.That (transformData ["position"] ["y"] == Vector3.one.y);
				Assert.That (transformData ["position"] ["z"] == Vector3.one.z);

				Assert.That (transformData ["rotation"] ["x"] == Quaternion.identity.x);
				Assert.That (transformData ["rotation"] ["y"] == Quaternion.identity.y);
				Assert.That (transformData ["rotation"] ["z"] == Quaternion.identity.z);
				Assert.That (transformData ["rotation"] ["w"] == Quaternion.identity.w);

				Assert.That (transformData ["scale"] ["x"] == Vector3.one.x);
				Assert.That (transformData ["scale"] ["y"] == Vector3.one.y);
				Assert.That (transformData ["scale"] ["z"] == Vector3.one.z);
			}
		}
	}

	[Test]
	public void TestOverwriteCollection(){
		CreateTwoCollectionsWithTwoArtefacts ();

		Dictionary<string, string[]> descriptiveData = new Dictionary<string, string[]>();
		Dictionary<string, VerticeTransform> structuralData = new Dictionary<string, VerticeTransform>();

		descriptiveData.Add("title", new string[] {"Test Overwrite"});
		descriptiveData.Add("creator", new string[] {"Test Creator Overwrite"});
		descriptiveData.Add("date", new string[] {"2016-10-26"});

		structuralData.Add("TEST-ARTEFACT-03", new VerticeTransform(Vector3.one, Quaternion.identity, Vector3.one));

		CollectionWriter.WriteCollectionWithIdentifer("TEST-COLLECTION-00", descriptiveData, structuralData);

		CollectionReader.LoadXmlFromFile (Paths.CollectionMetadata);
		Dictionary<string, string[]> collectionMetadata = CollectionReader.GetCollectionMetadataWithIdentifier ("TEST-COLLECTION-00");
		Assert.That (collectionMetadata ["title"] [0] == "Test Overwrite");
		Assert.That (collectionMetadata ["creator"] [0] == "Test Creator Overwrite");
		Assert.That (collectionMetadata ["date"] [0] == "2016-10-26");

		Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection ("TEST-COLLECTION-00", "TEST-ARTEFACT-03");
		Assert.That (transformData ["position"] ["x"] == Vector3.one.x);
		Assert.That (transformData ["position"] ["y"] == Vector3.one.y);
		Assert.That (transformData ["position"] ["z"] == Vector3.one.z);

		Assert.That (transformData ["rotation"] ["x"] == Quaternion.identity.x);
		Assert.That (transformData ["rotation"] ["y"] == Quaternion.identity.y);
		Assert.That (transformData ["rotation"] ["z"] == Quaternion.identity.z);
		Assert.That (transformData ["rotation"] ["w"] == Quaternion.identity.w);

		Assert.That (transformData ["scale"] ["x"] == Vector3.one.x);
		Assert.That (transformData ["scale"] ["y"] == Vector3.one.y);
		Assert.That (transformData ["scale"] ["z"] == Vector3.one.z);

	}

	[Test]
	public void TestSaveLocationChanges() {
		// Check that, when the save location is changed, the two XML files a) can vary independently, and that b) the second file 
		// is an extension of whatever data existed in the first file (i.e. that, in a sense, operations on the second file are precluded by 
		// all of the operations that had occurred to establish the first file).

		CollectionWriter.EstablishNewDocument ();

		// Create some data
		Dictionary<string, string[]> descriptiveData = new Dictionary<string, string[]>();
		Dictionary<string, VerticeTransform> structuralData = new Dictionary<string, VerticeTransform>();

		descriptiveData.Add("title", new string[] {"Test Title"});
		descriptiveData.Add("creator", new string[] {"Test Creator"});
		descriptiveData.Add("date", new string[] {"2016-10-20"});

		structuralData.Add("TEST-ARTEFACT-01", new VerticeTransform(Vector3.one, Quaternion.identity, Vector3.one));

		// Write data to the first file
		Paths.CollectionMetadata = Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml";
		CollectionWriter.WriteCollectionWithIdentifer("TEST-COLLECTION-01", descriptiveData, structuralData);

		// Change location and save the new collection
		Paths.CollectionMetadata = Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test_02.xml";
		CollectionWriter.WriteCollectionWithIdentifer("TEST-COLLECTION-02", descriptiveData, structuralData);

		// Check that the first file has only one collection
		CollectionReader.LoadXmlFromFile (Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml");
		string[] collectionIdentifiers_01 = CollectionReader.GetIdentifiersForCollections ();
		Assert.That (collectionIdentifiers_01.Length == 1);

		// Now check that the second file has two
		CollectionReader.LoadXmlFromFile(Paths.CollectionMetadata);
		string[] collectionIdentifiers_02 = CollectionReader.GetIdentifiersForCollections ();
		Assert.That (collectionIdentifiers_02.Length == 2);

		// Tidy up (the second file will be deleted in TearDown)
		File.Delete(Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml");


	}

	public void CreateTwoCollectionsWithTwoArtefacts() {

		for (int i = 0; i < 2; i++) {
			Dictionary<string, string[]> descriptiveData = new Dictionary<string, string[]> ();
			Dictionary<string, VerticeTransform> structuralData = new Dictionary<string, VerticeTransform> ();

			descriptiveData.Add ("title", new string[] { "Test Title", "A second title" });
			descriptiveData.Add ("creator", new string[] { "Test Creator", "A co-creator" });
			descriptiveData.Add ("date", new string[] { "2016-10-20" });

			for (int j = 0; j < 2; j++) {
				structuralData.Add (String.Format ("TEST-ARTEFACT-0{0}", j), new VerticeTransform (Vector3.one, Quaternion.identity, Vector3.one));
			}

			CollectionWriter.WriteCollectionWithIdentifer (String.Format("TEST-COLLECTION-0{0}", i), descriptiveData, structuralData);
		}
	}
}
