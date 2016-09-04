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
		string[] identifiers = DublinCoreReader.GetIdentifiersForCreators (new string[] {values[0]});
		Assert.That (identifiers.Length == 2);
		Assert.That (identifiers [0] == "DeerMan");
		Assert.That (identifiers [1] == "TestMonk");
	}

	[Test]
	public void TestGetIdentifiersForCreators_02()
	{
		string[] values = DublinCoreReader.GetValuesForCreator ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForCreators (new string[] {values[1]});
		Assert.That (identifiers.Length == 1);
		Assert.That (identifiers [0] == "TestMonk");
	}

	[Test]
	public void TestGetIdentifiersForCreators_03()
	{
		string[] values = DublinCoreReader.GetValuesForCreator ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForCreators (values);
		Assert.That (identifiers.Length == 2);
		Assert.That (identifiers [0] == "DeerMan");
		Assert.That (identifiers [1] == "TestMonk");
	}

	[Test]
	public void TestGetIdentifiersForCreators_NoResults(){
		string[] identifiers = DublinCoreReader.GetIdentifiersForCreators (new string[] {"No one"});
		Assert.That (identifiers.Length == 0);
	}

	[Test]
	public void TestGetValuesForContributor()
	{
		string[] values = DublinCoreReader.GetValuesForContributor ();
		Assert.That (values.Length == 4);
		Assert.That (values [0] == "Another Contributor");
		Assert.That (values [1] == "Doprah");
		Assert.That (values [2] == "Some Contributor");
		Assert.That (values [3] == "Yet Another Contributor");
	}

	[Test]
	public void TestGetIdentifiersForContributors_01()
	{
		// Test that passing all contributors returns all artefacts
		string[] values = DublinCoreReader.GetValuesForContributor ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForContributors (values);
		Assert.That (identifiers.Length == 2);
	}

	[Test]
	public void TestGetIdentifiersForContributors_02()
	{
		// Test set semantics (i.e. values[0] and values[1] are contributors for the same artefact, so should return one result)
		string[] values = DublinCoreReader.GetValuesForContributor ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForContributors (new string[] { values [1], values [3] });
		Assert.That (identifiers.Length == 1);
		Assert.That (identifiers[0] == "DeerMan");
	}

	[Test]
	public void TestGetIdentifiersForContributors_03()
	{
		// Test that a Contributor appearing in two artefacts returns two identifiers
		string[] values = DublinCoreReader.GetValuesForContributor ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForContributors (new string[] { values [0]});
		Assert.That (identifiers.Length == 2);
		Assert.That (identifiers[0] == "DeerMan");
		Assert.That (identifiers[1] == "TestMonk");
	}

	[Test]
	public void TestGetIdentifiersForContributors_NoResults()
	{
		// Test that a non-existant value returns no results
		string[] identifiers = DublinCoreReader.GetIdentifiersForContributors (new string[] { "NO ONE"});
		Assert.That (identifiers.Length == 0);
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
