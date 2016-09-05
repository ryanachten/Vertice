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
		// Test set semantics (i.e. values[1] and values[3] are contributors for the same artefact, so should return one result)
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
	public void TestGetIdentifiersForDateRange_01()
	{
		// Test that all artefact are returned when the date range is sufficiently wide
		DateTime start = DateTime.Parse("2015-10-02");
		DateTime end = DateTime.Parse ("2016-08-05");
		string[] identifiers = DublinCoreReader.GetIdentifiersForDateRange (start, end);
		Assert.That (identifiers.Length == 2);
		Assert.That (identifiers[0] == "DeerMan");
		Assert.That (identifiers[1] == "TestMonk");
	}

	[Test]
	public void TestGetIdentifiersForDateRange_02()
	{
		// Test that a specific artefact is returned when the date range is sufficient narrow
		DateTime start = DateTime.Parse("2015-10-02");
		DateTime end = DateTime.Parse ("2015-10-04");
		string[] identifiers = DublinCoreReader.GetIdentifiersForDateRange (start, end);
		Assert.That (identifiers.Length == 1);
		Assert.That (identifiers[0] == "DeerMan");
	}

	[Test]
	public void TestGetIdentifiersForDateRange_03()
	{
		// Test that no artefacts are returned when the date range does not overlap the artefacts
		DateTime start = DateTime.Parse("1970-01-01");
		DateTime end = DateTime.Parse ("1971-01-01");
		string[] identifiers = DublinCoreReader.GetIdentifiersForDateRange (start, end);
		Assert.That (identifiers.Length == 0);
	}

	[Test]
	public void TestGetIdentifiersForDateRange_04()
	{
		// Test that an artefact is returned when the start date and end date are equal, and equal the created date of the artefact
		DateTime start = DateTime.Parse("2015-10-03");
		DateTime end = DateTime.Parse ("2015-10-03");
		string[] identifiers = DublinCoreReader.GetIdentifiersForDateRange (start, end);
		Assert.That (identifiers.Length == 1);
		Assert.That (identifiers[0] == "DeerMan");
	}

	[Test]
	public void TestGetValuesForSubject()
	{
		string[] values = DublinCoreReader.GetValuesForSubject ();
		Assert.That (values.Length == 4);
		Assert.That (values [0] == "Album Art");
		Assert.That (values [1] == "Music");
		Assert.That (values [2] == "Stuff Ryan made");
		Assert.That (values [3] == "Testing");
	}

	[Test]
	public void TestGetIdentifiersForSubjects_01()
	{
		// Test that passing all subjects returns all artefacts
		string[] values = DublinCoreReader.GetValuesForSubject ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForSubjects (values);
		Assert.That (identifiers.Length == 2);
	}

	[Test]
	public void TestGetIdentifiersForSubjects_02()
	{
		// Test set semantics (i.e. values[0] and values[1] are subjects for the same artefact, so should return one result)
		string[] values = DublinCoreReader.GetValuesForSubject ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForSubjects (new string[] { values [0], values [1] });
		Assert.That (identifiers.Length == 1);
		Assert.That (identifiers[0] == "DeerMan");
	}

	[Test]
	public void TestGetIdentifiersForSubjects_03()
	{
		// Test that a Subject appearing in two artefacts returns two identifiers
		string[] values = DublinCoreReader.GetValuesForSubject ();
		string[] identifiers = DublinCoreReader.GetIdentifiersForSubjects (new string[] { values [2]});
		Assert.That (identifiers.Length == 2);
		Assert.That (identifiers[0] == "DeerMan");
		Assert.That (identifiers[1] == "TestMonk");
	}

	[Test]
	public void TestGetIdentifiersForSubjects_NoResults()
	{
		// Test that a non-existant value returns no results
		string[] identifiers = DublinCoreReader.GetIdentifiersForSubjects (new string[] {"NO SUBJECT"});
		Assert.That (identifiers.Length == 0);
	}
}
