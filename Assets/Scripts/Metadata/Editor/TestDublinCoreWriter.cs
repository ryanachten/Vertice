using UnityEngine;
using UnityEditor;
using NUnit.Framework;

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
}
