using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class TestCollectionWriter {

	[SetUp]
	public void Setup() {
		CollectionWriter.SetOutputFile (Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml");
	}

	[TearDown]
	public void TearDown() {
		File.Delete (Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml");
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

		CollectionReader.LoadXmlFromFile (Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Collection_Writer_Test.xml");
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
	public void TestWriteCollectionData_OneToMany(){
		Assert.That (false == true);
	}

	[Test]
	public void TestOverwriteCollection(){
		Assert.That (false == true);
	}

	[Test]
	public void TestSaveLocationChanges() {
		Assert.That (false == true);
	}
}
