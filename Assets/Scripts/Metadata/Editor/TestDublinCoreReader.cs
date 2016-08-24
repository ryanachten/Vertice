using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

public class TestDublinCoreReader {

	[Test]
	public void TestDummyData(){
		Dictionary<string, Dictionary<string, string[]>> data = DublinCoreReader.GetArtefactWithIdentifier ("THIS WONT DO ANYTHING");

		// Ensure that the nested dictionary structure exists
		Assert.NotNull (data);
		Assert.NotNull (data ["descriptive"]);
		Assert.NotNull (data ["structural"]);

		// Ensure that the structure of the dictionary is as expected
		Assert.That(data.Keys.Count == 2);
		Assert.That(data["descriptive"].Keys.Count == 6);
		Assert.That(data["structural"].Keys.Count == 4);

		// Ensure that the length of the arrays in the 'leaves' is as expected
		//
		// <descriptive>
		Assert.That(data["descriptive"]["title"].Length == 3);
		Assert.That(data["descriptive"]["creator"].Length == 2);
		Assert.That(data["descriptive"]["description"].Length == 1);
		Assert.That(data["descriptive"]["format"].Length == 1);
		Assert.That(data["descriptive"]["identifier"].Length == 1);
		Assert.That(data["descriptive"]["relation"].Length == 1);

		// <structural>
		Assert.That(data["structural"]["creator"].Length == 1);
		Assert.That(data["structural"]["created"].Length == 1);
		Assert.That(data["structural"]["identifier"].Length == 2);
		Assert.That(data["structural"]["isVersionOf"].Length == 1);

		// Ensure that (some of) the actual data is correct
		Assert.That (data ["descriptive"] ["title"] [0] == "Another title for another original resource");
		Assert.That (data["descriptive"]["title"][1] == "Another title for that original resource, maybe in some other language?");
		Assert.That (data ["structural"] ["identifier"] [0] == "ID1234.19.A.obj");

	}
}
