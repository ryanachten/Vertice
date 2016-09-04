using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class TestDublinCoreBrowse {

	[SetUp]
	public void SetUpXml(){
		DublinCoreReader.LoadXml ("file://" + Environment.CurrentDirectory + "/Assets/Scripts/Metadata/TestAssets/Metapipe_ObjArchive_Subset_As_DublinCore_With_Browse_Fields.xml");
	}

	[Test]
	public void TestGetValuesForCreator()
	{
		string[] values = DublinCoreReader.GetValuesForCreator ();
		Assert.That (values.Length == 2);
		Assert.That (values [0] == "Ryan Achten");
		Assert.That (values [1] == "Some Other Guy");
	}

	[Test]
	public void TestGetIdentifiersForCreators_01()
	{
		string[] values = DublinCoreReader.GetValuesForCreator ();
		string[] identifiers = DublinCoreReader.GetIndentifiersForCreators (new string[] {values[0]});
		Assert.That (identifiers.Length == 2);
	}

	[Test]
	public void TestGetIdentifiersForCreators_02()
	{
		string[] values = DublinCoreReader.GetValuesForCreator ();
		string[] identifiers = DublinCoreReader.GetIndentifiersForCreators (new string[] {values[1]});
		Assert.That (identifiers.Length == 1);
	}

	[Test]
	public void TestGetIdentifiersForCreators_03()
	{
		string[] values = DublinCoreReader.GetValuesForCreator ();
		string[] identifiers = DublinCoreReader.GetIndentifiersForCreators (values);
		Assert.That (identifiers.Length == 2);
	}

	[Test]
	public void TestGetIdentifiersForCreators_NoResults(){
		string[] identifiers = DublinCoreReader.GetIndentifiersForCreators (new string[] {"No one"});
		Assert.That (identifiers.Length == 0);
	}

	[Test]
	public void TestBrowseByContributor()
	{
		Assert.Fail ();
	}

	[Test]
	public void TestBrowseByDateEquals()
	{
		Assert.Fail ();
	}

	[Test]
	public void TestBrowseByDateRange()
	{
		Assert.Fail ();
	}

	[Test]
	public void TestBrowseBySubject()
	{
		Assert.Fail ();
	}

	[Test]
	public void TestBrowseByCoverage()
	{
		Assert.Fail ();
	}
}
