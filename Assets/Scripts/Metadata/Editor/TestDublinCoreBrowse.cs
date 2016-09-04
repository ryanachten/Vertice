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
	public void TestBrowseByCreator_Exact()
	{
		string[] identifiers = DublinCoreReader.BrowseByCreator ("Ryan Achten");
		Assert.That (identifiers.Length == 2);
	}

	[Test]
	public void TestBrowseByCreator_StartsWith()
	{
		string[] identifiers = DublinCoreReader.BrowseByCreator ("Ryan");
		Assert.That (identifiers.Length == 2);
		Assert.That (identifiers [0] == "DeerMan");
		Assert.That (identifiers [1] == "TestMonk");
	}

	[Test]
	public void TestBrowseByCreator_NoResults()
	{
		string[] identifiers = DublinCoreReader.BrowseByCreator ("No one");
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
