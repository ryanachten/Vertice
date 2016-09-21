using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class TestVerticeTransform {

	[Test]
	public void TestInstantiateWithFloats(){
		VerticeTransform transform = new VerticeTransform (1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 0.0f, -1.0f, -2.0f, -3.0f);
		Assert.That (transform.position.x == 1.0f);
		Assert.That (transform.position.y == 2.0f);
		Assert.That (transform.position.z == 3.0f);
		Assert.That (transform.rotation.x == 4.0f);
		Assert.That (transform.rotation.y == 5.0f);
		Assert.That (transform.rotation.z == 6.0f);
		Assert.That (transform.rotation.w == 0.0f);
		Assert.That (transform.scale.x == -1.0f);
		Assert.That (transform.scale.y == -2.0f);
		Assert.That (transform.scale.z == -3.0f);
	}

	[Test]
	public void TestInstantiateWithCollectionReaderDictionary_Complete(){
		Assert.Fail();
	}

	[Test]
	public void TestInstantiateWithCollectionReaderDictionary_IncompletePosition(){
		Assert.Fail();
	}

	[Test]
	public void TestInstantiateWithCollectionReaderDictionary_IncompleteScale(){
		Assert.Fail();
	}

	[Test]
	public void TestInstantiateWithCollectionReaderDictionary_IncompleteRotation(){
		Assert.Fail();
	}

	[Test]
	public void TestInstantiateRandomTransform(){
		Assert.Fail();
	}
}
