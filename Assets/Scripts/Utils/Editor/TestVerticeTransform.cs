using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class TestVerticeTransform {

	[Test]
	public void TestInstantiateWithFloats(){
		VerticeTransform transform = new VerticeTransform (1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 0.0f, -1.0f, -2.0f, -3.0f);
		Assert.That (transform.position.x == 1.0f);
		Assert.That (transform.position.y == 2.0f);
		Assert.That (transform.position.z == 3.0f);
		Assert.That (transform.rotation.x == 4.0f);
		Assert.That (transform.rotation.y == 5.0f);
		Assert.That (transform.rotation.z == 6.0f);
		Assert.That (transform.rotation.w == 0.0f);
		Assert.That (transform.scale.x == -1.0f);
		Assert.That (transform.scale.y == -2.0f);
		Assert.That (transform.scale.z == -3.0f);
	}

	[Test]
	public void TestInstantiateWithCollectionReaderDictionary(){

		// Load in the XML file
		CollectionReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml"); 

		// Get the list of collection identifiers, and subsequently the list of artefact identifiers for the first collection
		string[] collectionIdentifiers = CollectionReader.GetIdentifiersForCollections ();
		string[] artefactIdentifiers = CollectionReader.GetIdentifiersForArtefactsInCollectionWithIdentifier (collectionIdentifiers [0]);

		// Now get the transform data for the first artefact in the first collection, and attempt to construct a VerticeTransform with the data
		Dictionary<string, Dictionary<string, float>> transformData = CollectionReader.GetTransformForArtefactWithIdentifierInCollection (collectionIdentifiers [0], artefactIdentifiers [0]);
		VerticeTransform transform = new VerticeTransform (transformData);

		Assert.That (transform.position.x == 40.01599f);
		Assert.That (transform.position.y == -11.58916f);
		Assert.That (transform.position.z == 184.2516f);
		Assert.That (transform.rotation.x == 1.0f);
		Assert.That (transform.rotation.y == 1.0f);
		Assert.That (transform.rotation.z == 1.0f);
		Assert.That (transform.rotation.w == 1.0f);
		Assert.That (transform.scale.x == 1.0f);
		Assert.That (transform.scale.y == 1.0f);
		Assert.That (transform.scale.z == 1.0f);
	}

	[Test]
	public void TestInstantiateRandomTransform(){
		VerticeTransform transform = new VerticeTransform (-10.0f, 10.0f, -10.0f, 10.0f);

		// Check that x is between xMin and xMax
		Assert.That (transform.position.x > -10.0f);
		Assert.That (transform.position.x < 10.0f);

		// Check that the default value (i.e. y = 15) is respected
		Assert.That (transform.position.y == 15.0f);

		// Check that z is between zMin and zMax
		Assert.That (transform.position.z > -10.0f);
		Assert.That (transform.position.z < 10.0f);

		// Check that rotation is set to the identity
		Assert.That (transform.rotation.x == Quaternion.identity.x);
		Assert.That (transform.rotation.y == Quaternion.identity.y);
		Assert.That (transform.rotation.z == Quaternion.identity.z);
		Assert.That (transform.rotation.w == Quaternion.identity.w);

		// Check that scale is set to the identity
		Assert.That (transform.scale.x == 1.0f);
		Assert.That (transform.scale.y == 1.0f);
		Assert.That (transform.scale.z == 1.0f);

	}
}
