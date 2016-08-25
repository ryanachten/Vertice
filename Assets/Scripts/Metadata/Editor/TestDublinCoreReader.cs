using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.IO;

public class TestDublinCoreReader {

	[Test]
	/// <summary>
	/// Indirectly test that a specified XML file can be read in and assigned to the internal _xmlDocument property
	/// </summary>
	public void TestLazyInitialisation(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		DublinCoreReader.Refresh ();
	}

	[Test]
	[ExpectedException(typeof( FileNotFoundException ))]
	public void RefreshThrowsFileNotFoundIfXmlDocumentNotLoaded(){
		// Explicitly set _uri to null
		DublinCoreReader.LoadXml (null);
		DublinCoreReader.Refresh ();
	}

	[Test]
	public void TestBasicMetadataReading_01(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, string[]>> metadata = DublinCoreReader.GetArtefactWithIdentifier ("TestMonk");

		// Check that both titles are represented
		Assert.That (metadata ["descriptive"] ["title"] [0] == "Test Monk" || metadata ["descriptive"] ["title"] [0] == "Doog");
		Assert.That (metadata ["descriptive"] ["title"] [1] == "Test Monk" || metadata ["descriptive"] ["title"] [1] == "Doog");

		// Check that the number of keys in each dictionary makes sense
		Assert.That(metadata["descriptive"].Keys.Count == 4); // Title, Description, Creator, Date
		Assert.That(metadata["structural"].Keys.Count == 5); // Creator, Created, Description, Identifier, Extent

		// Check that the content makes sense
		Assert.That(metadata["descriptive"]["description"][0] == "Model for testing Vertice Archive connection");
		Assert.That(metadata["descriptive"]["creator"][0] == "Ryan Achten");
		Assert.That(metadata["descriptive"]["date"][0] == "13/04/2016");

		Assert.That(metadata["structural"]["creator"][0] == "Ryan Achten");
		Assert.That(metadata["structural"]["created"][0] == "13/04/2016");
		Assert.That(metadata["structural"]["description"][0] == "Created in Blender");
		Assert.That(metadata["structural"]["identifier"][0] == "Doog.obj");
		Assert.That(metadata["structural"]["extent"][0] == "1.5 scale");

	}

	[Test]
	[ExpectedException(typeof( NoSuchArtefactException ))]
	public void TestBasicMetadataReading_Invalid(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, Dictionary<string, string[]>> metadata = DublinCoreReader.GetArtefactWithIdentifier ("DOES NOT EXIST");
	}

	[Test]
	public void TestGetMeshLocationForArtefactWithIdentifier(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		string meshLocation = DublinCoreReader.GetMeshLocationForArtefactWithIdentifier ("TestMonk");
		Assert.That (meshLocation == "/VerticeArchive/TEST/Doog.obj");
	}

	[Test]
	[ExpectedException(typeof( NoSuchArtefactException ))]
	public void TestGetMeshLocationForArtefactWithIdentifier_Invalid(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		string meshLocation = DublinCoreReader.GetMeshLocationForArtefactWithIdentifier ("DOES NOT EXIST");
	}

	[Test]
	[ExpectedException(typeof( NoModelInformationException ))]
	public void TestGetMeshLocationForArtefactWithIdentifier_NoAssets(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		string meshLocation = DublinCoreReader.GetMeshLocationForArtefactWithIdentifier ("NoAssets");
	}

	[Test]
	public void TestGetTextureLocationForArtefactWithIdentifier(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		string texLocation = DublinCoreReader.GetTextureLocationForArtefactWithIdentifier ("TestMonk");
		Assert.That (texLocation == "/VerticeArchive/TEST/Doog_Tex.jpg");
	}

	[Test]
	[ExpectedException(typeof( NoSuchArtefactException ))]
	public void TestGetTextureLocationForArtefactWithIdentifier_Invalid(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		string texLocation = DublinCoreReader.GetTextureLocationForArtefactWithIdentifier ("DOES NOT EXIST");
	}

	[Test]
	public void TestGetContextualMediaForArtefactWithIdentifier(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, string>[] contextualMedia = DublinCoreReader.GetContextualMediaForArtefactWithIdentifier ("TestMonk");

		Assert.That (contextualMedia [0] ["MediaName"] == "MetaPipe_TestTexs_1000");
		Assert.That (contextualMedia [0] ["MediaType"] == "Image");
		Assert.That (contextualMedia [0] ["MediaLocation"] == "/VerticeArchive/TEST/TestTexs_1000.jpg");

		Assert.That (contextualMedia [1] ["MediaName"] == "WorldACoke");
		Assert.That (contextualMedia [1] ["MediaType"] == "Video");
		Assert.That (contextualMedia [1] ["MediaLocation"] == "/VerticeArchive/TEST/WorldACoke.ogg");

		Assert.That (contextualMedia [2] ["MediaName"] == "MetaPipe_TestTexs_2000W");
		Assert.That (contextualMedia [2] ["MediaType"] == "Image");
		Assert.That (contextualMedia [2] ["MediaLocation"] == "/VerticeArchive/TEST/TestTexs_2000W.jpg");
	}

	[Test]
	[ExpectedException(typeof( NoSuchArtefactException ))]
	public void TestGetContextualMediaForArtefactWithIdentifier_Invalid(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, string>[] contextualMedia = DublinCoreReader.GetContextualMediaForArtefactWithIdentifier ("DOES NOT EXIST");
	}

	[Test]
	public void TestGetContextualMediaOfForArtefactWithIdentifierAndType(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, string>[] contextualMedia = DublinCoreReader.GetContextualMediaArtefactWithIdentifierAndType ("TestMonk", "Image");

		Assert.That (contextualMedia [0] ["MediaName"] == "MetaPipe_TestTexs_1000");
		Assert.That (contextualMedia [0] ["MediaType"] == "Image");
		Assert.That (contextualMedia [0] ["MediaLocation"] == "/VerticeArchive/TEST/TestTexs_1000.jpg");

		Assert.That (contextualMedia [1] ["MediaName"] == "MetaPipe_TestTexs_2000W");
		Assert.That (contextualMedia [1] ["MediaType"] == "Image");
		Assert.That (contextualMedia [1] ["MediaLocation"] == "/VerticeArchive/TEST/TestTexs_2000W.jpg");
	}

	[Test]
	[ExpectedException(typeof( NoSuchArtefactException ))]
	public void TestGetContextualMediaOfForArtefactWithIdentifierAndType_InvalidIdentifier(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, string>[] contextualMedia = DublinCoreReader.GetContextualMediaArtefactWithIdentifierAndType ("DOES NOT EXIST", "Image");
	}

	[Test]
	[ExpectedException(typeof( NoContextualMediaException ))]
	public void TestGetContextualMediaOfForArtefactWithIdentifierAndType_InvalidType(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore.xml");
		Dictionary<string, string>[] contextualMedia = DublinCoreReader.GetContextualMediaArtefactWithIdentifierAndType ("TestMonk", "INVALID TYPE");
	}
}
