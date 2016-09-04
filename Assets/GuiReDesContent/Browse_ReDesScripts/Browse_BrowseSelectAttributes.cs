using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Browse_SelectAttributes : MonoBehaviour {

	//enables select attribute panel for user to refine their
	//browse query, then sends to browse control

	public Transform attributeParent; //transform for prefabs to be instantiated under
	public Object attributePrefab;
	public Browse_BrowseControl BrowseCont;


	/// <summary>
	/// Gets all attributes related to a user browse query type. Executes DCReader function.
	/// </summary>
	/// <param name="browseType">Type of browse user wants to view</param>
	public void GetAttributes(string browseType)
	{
		//TODO DCReader function for returning attributes based on user type query
		//InstantAttributes();
	}


	/// <summary>
	/// Instantiates browse attribute prefabs for user to select
	/// </summary>
	/// <param name="InstantAttributes">attributes in XML related to user query</param>
	private void InstantAttributes( string[] browseAttributes)
	{
		ResetAttributes ();
		for (int i = 0; i < browseAttributes.Length; i++) {
			
			GameObject curBrowseAtt = Object.Instantiate (attributePrefab, attributeParent) as GameObject;
			curBrowseAtt.GetComponentInChildren<Text>().text = browseAttributes[i];
		}
	}
		

	/// <summary>
	/// Resets the attribute panel
	/// </summary>
	private void ResetAttributes()
	{
		for (int i = 0; i < attributeParent.childCount; i++) 
		{
			GameObject curAttr = attributeParent.GetChild (i).gameObject;
			Destroy(curAttr);
		}
	}

	/// <summary>
	/// Executed once user has finished their selection of relevant attributes
	/// </summary>
	public void DoneAttributeSelect()
	{
		List<string> activeAttributes = new List<string> ();

		for (int i = 0; i < attributeParent.childCount; i++) {

			Toggle curToggle = attributeParent.GetChild (i).GetComponent<Toggle>();
			if (curToggle.isOn) 
			{
				activeAttributes.Add (attributeParent.GetChild (i).GetComponentInChildren<Text>().text);
			}
		}
		BrowseCont.ImportArtefacts (activeAttributes);
		gameObject.SetActive (false);

	}
}
