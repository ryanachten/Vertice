using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestCollectionReader {

	[Test]
	/// <summary>
	/// Indirectly test that a specified XML file can be read in and assigned to the internal _xmlDocument property
	/// </summary>
	public void TestLazyInitialisation(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_UserCollections_As_DublinCore.xml");
		DublinCoreReader.Refresh ();
	}

	[Test]
	[ExpectedException(typeof( FileNotFoundException ))]
	public void RefreshThrowsFileNotFoundIfXmlDocumentNotLoaded(){
		// Explicitly set _uri to null
		DublinCoreReader.LoadXml (null);
		DublinCoreReader.Refresh ();
	}
}
