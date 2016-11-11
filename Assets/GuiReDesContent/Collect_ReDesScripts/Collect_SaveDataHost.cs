using UnityEngine;
using System.Collections.Generic;

public struct CollectDataHost
{
	public static List<string> CollectionTitle;
	public static string CollectionIdentifier;
	public static List<string> CollectionCreator;
	public static List<string> CollectionContributor;
	public static List<string> CollectionDate;
	public static List<string> CollectionCoverage;
	public static List<string> CollectionSubject;
	public static string CollectionDescription;


	static public void ResetSaveData()
	{
		CollectionTitle = new  List<string>();
		CollectionIdentifier = null;
		CollectionCreator = new  List<string>();
		CollectionContributor = new  List<string>();
		CollectionDate = new  List<string>();
		CollectionCoverage = new  List<string>();
		CollectionSubject = new  List<string>();
		CollectionDescription = null;
	}


	static public void LoadXmlData(string collectionId)
	{
		Dictionary<string, string[]> data = new Dictionary<string, string[]>();

		data = new Dictionary<string, string[]>();
		data = CollectionReader.GetCollectionMetadataWithIdentifier(collectionId);

		CollectionTitle = CreateStructList("title", data);
		CollectionIdentifier = collectionId;
		CollectionCreator = CreateStructList ("creator", data);
		CollectionContributor = CreateStructList("contributor", data);
		CollectionDate = CreateStructList("date", data);
		CollectionCoverage = CreateStructList("coverage", data);
		CollectionSubject = CreateStructList("subject", data);

		try {
			CollectionDescription = data["description"][0];
		} catch (KeyNotFoundException e) {
			CollectionDescription = "";
		}
	}


	/// <summary>
	/// Instantiates prefabs for multi-field attributes
	/// </summary>
	/// <param name="data">Artefact dictionary data.</param>
	/// <param name="elementType">Dublin Core type to be searched (i.e. Descriptive or Structural).</param>
	/// <param name="elementName">Attribute to be found in artefact data (i.e. Title / Date etc)</param>
	/// <param name="fieldGroup">Parent for the prefab to be instanted under</param>
//	private void InstantFieldData (string elementName, Dictionary<string, string[]> data) 
//	{
//		ResetField(fieldGroup); //TODO ResetSaveData() here?

	static private List<string> CreateStructList(string elementName, Dictionary<string, string[]> data)
	{
		List<string> attributeValues = new List<string>();

		try {
			string[] curData = data[elementName];
			for (int i = 0; i < curData.Length; i++) 
			{
				attributeValues.Add(curData[i]);
			}
		}
		catch(System.Exception ex)
		{
			Debug.Log ("No data in field");
			attributeValues = null;
		}
		return attributeValues;
	}


	static public void DebugCollectionStruct()
	{
		Debug.Log("");
		Debug.Log("**** Collection Struct: ****");
		try {
			for (int i = 0; i < CollectionTitle.Count; i++)
			{
				Debug.Log("\tTitle: " + CollectionTitle[i]);
			}
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Title");	}

		try {
			Debug.Log("\tIdentifier: " + CollectionIdentifier);
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Identifier");	}
		
		try {
			for (int i = 0; i < CollectionCreator.Count; i++)
			{
				Debug.Log("\tCreator: " + CollectionCreator[i]);
			}
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Creator");	}

		try {
			for (int i = 0; i < CollectionContributor.Count; i++)
			{
				Debug.Log("\tContributor: " + CollectionContributor[i]);
			}
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Contributor");	}

		try {
		for (int i = 0; i < CollectionDate.Count; i++)
		{
			Debug.Log("\tDate: " + CollectionDate[i]);
		}
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Date");	}
		
		try {
		for (int i = 0; i < CollectionCoverage.Count; i++)
		{
			Debug.Log("\tCoverage: " + CollectionCoverage[i]);
		}
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Coverage");	}
		
		try {
		for (int i = 0; i < CollectionSubject.Count; i++)
		{
			Debug.Log("\tSubject: " + CollectionSubject[i]);
		}
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Subject");	}

		try {
			Debug.Log("\tDescription: " + CollectionDescription);
		}catch(System.Exception ex) {
			Debug.Log ("\tNo data in Description");	}

		
		Debug.Log("/**** Collection Struct: ****");
		Debug.Log("");
	}
}

