using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System;

public class TestDublinCoreWriter {

	[Test]
	public void TestInstantiateNewWriter()
	{
		DublinCoreWriter dcWriter = new DublinCoreWriter (null, "*");
		Assert.NotNull(dcWriter);
		Assert.That (dcWriter.GetRootElement ().LocalName == "verticeMetadata");

	}

	[Test]
	public void TestInstantiateEditor()
	{
		DublinCoreWriter dcWriter = new DublinCoreWriter (null, "Assets/Scripts/Metadata/TestAssets/Vertice_RootOnly_Valid.xml");
		Assert.NotNull(dcWriter);
		Assert.That(dcWriter.GetRootElement().LocalName == "verticeMetadata");

	}

	[Test]
	public void TestAddMetadataFromDictionary()
	{

		Dictionary<string, object> metadata_basic = new Dictionary<string, object> ();
		Dictionary<string, object> descriptive = new Dictionary<string, object>();
		Dictionary<string, object> structural = new Dictionary<string, object>();

		descriptive.Add ("title", new string[] {
			"The title of the original resource",
			"The title of the original resource, maybe in some other language?"
		});
		descriptive.Add ("creator", new string[] {"Some Guy"});
		descriptive.Add ("description", new string[] {"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce blandit blandit massa vitae ultrices. Fusce ligula augue, ullamcorper non bibendum in, sollicitudin et risus. Maecenas eget elementum massa, vitae sagittis urna. Mauris vestibulum erat et volutpat fermentum. Nulla rutrum nisi enim, eget rhoncus ante condimentum ac. Phasellus nec eleifend eros. Vestibulum vitae molestie nisl, vitae porttitor mi. Pellentesque convallis justo a suscipit scelerisque. Pellentesque in luctus turpis, eu pretium nunc. Vivamus libero massa, ultrices in tempor ac, varius a diam. Sed nec urna quis nisi viverra volutpat sed quis arcu. Donec pharetra ex vel fringilla viverra. Cras quis congue mi."});
		descriptive.Add ("format", new string[] { "Statue" });
		descriptive.Add ("identifier", new string[] {"ID1234/19/A"});
		descriptive.Add ("relation", new string[] {"http://themuseum.org/catalogue/ID123419A"});
		metadata_basic.Add ("descriptive", descriptive);


		structural.Add ("creator", new string[] { "Digitisation Guy" });
		structural.Add ("created", new string[] { "2015-01-25" });
		structural.Add ("identifier", new string[] { "ID1234.19.A.obj\", \"ID1234.19.A.png" });
		structural.Add ("isVersionOf", new string[] { "http://themuseum.org/repository/ID123419A.zip" });
		metadata_basic.Add ("structural", structural);

		DublinCoreWriter dcWriter = new DublinCoreWriter (metadata_basic, "ID1234/19/A");
		XmlDocument builtDocument = dcWriter.GetDocument ();

		// Search for appropriate nodes to test the built XML document
		XmlNodeList verticeMetadataChildren = builtDocument.SelectSingleNode ("//verticeMetadata").ChildNodes;
		XmlNodeList artefact = builtDocument.SelectSingleNode ("//verticeMetadata/artefact").ChildNodes;
		XmlNodeList descriptiveChildren = builtDocument.SelectSingleNode ("//descriptive").ChildNodes;
		XmlNodeList structuralChildren = builtDocument.SelectSingleNode ("//structural").ChildNodes;
		XmlNodeList titleNodes = builtDocument.SelectNodes ("//descriptive/title");

		// Are <descriptive> and <structural> entered correctly
		Assert.That (verticeMetadataChildren.Count == 1); 
		Assert.That (artefact [0].LocalName == "descriptive" || artefact [0].LocalName == "structural");
		Assert.That (artefact [1].LocalName == "descriptive" || artefact [1].LocalName == "structural");

		// Are the child elements of <descriptive> and <structural> entered correctly
		Assert.That (descriptiveChildren.Count == 7);
		Assert.That (structuralChildren.Count == 4);

		// Are fields with unrestricted cardinality entered correctly
		Assert.That (titleNodes.Count == 2);
	}

	[Test]
	public void TestAddMetadataForMultipleArtefacts()
	{

		// Establish first record
		Dictionary<string, object> metadata_basic_01 = new Dictionary<string, object> ();
		Dictionary<string, object> descriptive_01 = new Dictionary<string, object>();
		Dictionary<string, object> structural_01 = new Dictionary<string, object>();

		descriptive_01.Add ("title", new string[] {
			"Another title for another original resource",
			"Another title for that original resource, maybe in some other language?",
			"Yet another title, for that original resource"
		});
		descriptive_01.Add ("creator", new string[] {"Some Guy", "Some Guy's Friend"});
		descriptive_01.Add ("description", new string[] {"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce blandit blandit massa vitae ultrices. Fusce ligula augue, ullamcorper non bibendum in, sollicitudin et risus. Maecenas eget elementum massa, vitae sagittis urna. Mauris vestibulum erat et volutpat fermentum. Nulla rutrum nisi enim, eget rhoncus ante condimentum ac. Phasellus nec eleifend eros. Vestibulum vitae molestie nisl, vitae porttitor mi. Pellentesque convallis justo a suscipit scelerisque. Pellentesque in luctus turpis, eu pretium nunc. Vivamus libero massa, ultrices in tempor ac, varius a diam. Sed nec urna quis nisi viverra volutpat sed quis arcu. Donec pharetra ex vel fringilla viverra. Cras quis congue mi."});
		descriptive_01.Add ("format", new string[] { "Statue" });
		descriptive_01.Add ("identifier", new string[] {"ID1234/19/A"});
		descriptive_01.Add ("relation", new string[] {"http://themuseum.org/catalogue/ID123419A"});
		metadata_basic_01.Add ("descriptive", descriptive_01);


		structural_01.Add ("creator", new string[] { "Digitisation Guy" });
		structural_01.Add ("created", new string[] { "2015-01-25" });
		structural_01.Add ("identifier", new string[] { "ID1234.19.A.obj", "ID1234.19.A.png" });
		structural_01.Add ("isVersionOf", new string[] { "http://themuseum.org/repository/ID123419A.zip" });
		metadata_basic_01.Add ("structural", structural_01);

		// Establish second record
		Dictionary<string, object> metadata_basic_02 = new Dictionary<string, object> ();
		Dictionary<string, object> descriptive_02 = new Dictionary<string, object>();
		Dictionary<string, object> structural_02 = new Dictionary<string, object>();

		descriptive_02.Add ("title", new string[] {
			"The title of the original resource",
			"The title of the original resource, maybe in some other language?"
		});
		descriptive_02.Add ("creator", new string[] {"Some Other Guy"});
		descriptive_02.Add ("description", new string[] { "Another description" });
		descriptive_02.Add ("format", new string[] { "Junk" });
		descriptive_02.Add ("identifier", new string[] {"ID1234/19/B"});
		descriptive_02.Add ("relation", new string[] {"http://themuseum.org/catalogue/ID123419B"});
		metadata_basic_02.Add ("descriptive", descriptive_02);


		structural_02.Add ("creator", new string[] { "Digitisation Guy" });
		structural_02.Add ("created", new string[] { "2015-01-25" });
		structural_02.Add ("identifier", new string[] { "ID1234.19.B.obj", "ID1234.19.B.png", "ID1234.19.B.ogg" });
		structural_02.Add ("isVersionOf", new string[] { "http://themuseum.org/repository/ID123419B.zip" });
		metadata_basic_02.Add ("structural", structural_02);


		DublinCoreWriter dcWriter_01 = new DublinCoreWriter (metadata_basic_01, "ID1234/19/A", Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Vertice_Test_Append.xml");
		DublinCoreWriter dcWriter_02 = new DublinCoreWriter (metadata_basic_02, "ID1234/19/B", Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Vertice_Test_Append.xml");
		XmlDocument builtDocument = dcWriter_02.GetDocument ();

		// Search for appropriate nodes to test the basic structure of the built XML document
		XmlNodeList verticeMetadataChildren = builtDocument.SelectSingleNode ("/verticeMetadata").ChildNodes;

		// First artefact
		XmlNodeList artefact_01 = builtDocument.SelectSingleNode (String.Format("/verticeMetadata/artefact[@id='{0}']", "ID1234/19/A")).ChildNodes;
		XmlNodeList descriptiveChildren_01 = builtDocument.SelectSingleNode (String.Format("/verticeMetadata/artefact[@id='{0}']/descriptive", "ID1234/19/A")).ChildNodes;
		XmlNodeList structuralChildren_01 = builtDocument.SelectSingleNode (String.Format("/verticeMetadata/artefact[@id='{0}']/structural", "ID1234/19/A")).ChildNodes;
		XmlNodeList titleNodes_01 = builtDocument.SelectNodes (String.Format("/verticeMetadata/artefact[@id='{0}']/descriptive/title", "ID1234/19/A"));

		// Second artefeact
		XmlNodeList artefact_02 = builtDocument.SelectSingleNode (String.Format("/verticeMetadata/artefact[@id='{0}']", "ID1234/19/B")).ChildNodes;
		XmlNodeList descriptiveChildren_02 = builtDocument.SelectSingleNode (String.Format("/verticeMetadata/artefact[@id='{0}']/descriptive", "ID1234/19/B")).ChildNodes;
		XmlNodeList structuralChildren_02 = builtDocument.SelectSingleNode (String.Format("/verticeMetadata/artefact[@id='{0}']/structural", "ID1234/19/B")).ChildNodes;
		XmlNodeList titleNodes_02 = builtDocument.SelectNodes (String.Format("/verticeMetadata/artefact[@id='{0}']/descriptive/title", "ID1234/19/B"));

		// Are there now two children of the <verticeMedata> element?
		Assert.That(verticeMetadataChildren.Count == 2);

		// Are <descriptive> and <structural> entered correctly
		Assert.That (artefact_01.Count == 2); 
		Assert.That (artefact_02.Count == 2); 
		Assert.That (artefact_01 [0].LocalName == "descriptive" || artefact_01 [0].LocalName == "structural");
		Assert.That (artefact_01 [1].LocalName == "descriptive" || artefact_01 [1].LocalName == "structural");
		Assert.That (artefact_02 [0].LocalName == "descriptive" || artefact_02 [0].LocalName == "structural");
		Assert.That (artefact_02 [1].LocalName == "descriptive" || artefact_02 [1].LocalName == "structural");

		// Are the child elements of <descriptive> and <structural> entered correctly
		Assert.That (descriptiveChildren_01.Count == 9);
		Assert.That (descriptiveChildren_02.Count == 7);
		Assert.That (structuralChildren_01.Count == 5);
		Assert.That (structuralChildren_02.Count == 6);

		// Are fields with unrestricted cardinality entered correctly
		Assert.That (titleNodes_01.Count == 3);
		Assert.That (titleNodes_02.Count == 2);

		File.Delete (Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Vertice_Test_Append.xml");
	}
}
