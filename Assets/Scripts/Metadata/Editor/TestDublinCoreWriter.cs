using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Xml;
using System.Collections.Generic;

public class TestDublinCoreWriter {

	[Test]
	public void TestInstantiateNewWriter()
	{
		DublinCoreWriter dcWriter = new DublinCoreWriter (null);
		Assert.NotNull(dcWriter);
		Assert.That (dcWriter.GetRootElement ().LocalName == "verticeMetadata");

	}

	[Test]
	public void TestInstantiateEditor_Valid()
	{
		DublinCoreWriter dcWriter = new DublinCoreWriter (null, "Assets/Scripts/Metadata/TestAssets/Vertice_RootOnly_Valid.xml");
		Assert.NotNull(dcWriter);
		Assert.That(dcWriter.GetRootElement().LocalName == "verticeMetadata");

	}

	[Test]
	public void TestInstantiateEditor_Invalid()
	{
		DublinCoreWriter dcWriter = new DublinCoreWriter (null, "Assets/Scripts/Metadata/TestAssets/Vertice_RootOnly_Invalid.xml");
		Assert.NotNull(dcWriter);
		Assert.That(dcWriter.GetRootElement() == null);

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

		DublinCoreWriter dcWriter = new DublinCoreWriter (metadata_basic);
		XmlDocument builtDocument = dcWriter.GetDocument ();

		// Search for appropriate nodes to test the built XML document
		XmlNodeList verticeMetadataChildren = builtDocument.SelectSingleNode ("//verticeMetadata").ChildNodes;
		XmlNodeList descriptiveChildren = builtDocument.SelectSingleNode ("//descriptive").ChildNodes;
		XmlNodeList structuralChildren = builtDocument.SelectSingleNode ("//structural").ChildNodes;
		XmlNodeList titleNodes = builtDocument.SelectNodes ("//descriptive/title");

		// Are <descriptive> and <structural> entered correctly
		Assert.That (verticeMetadataChildren.Count == 2); 
		Assert.That (verticeMetadataChildren [0].LocalName == "descriptive" || verticeMetadataChildren [0].LocalName == "structural");
		Assert.That (verticeMetadataChildren [1].LocalName == "descriptive" || verticeMetadataChildren [1].LocalName == "structural");

		// Are the child elements of <descriptive> and <structural> entered correctly
		Assert.That (descriptiveChildren.Count == 7);
		Assert.That (structuralChildren.Count == 4);

		// Are fields with unrestricted cardinality entered correctly
		Assert.That (titleNodes.Count == 2);
	}
}
