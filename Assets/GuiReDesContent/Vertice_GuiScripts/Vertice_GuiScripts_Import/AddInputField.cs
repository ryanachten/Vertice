using UnityEngine;
using System.Collections;

//reusable script for instantiating a new field into a multiple field metadata attribute

public class AddInputField : MonoBehaviour {

	public Object field; //field prefab to be duplicated
	public Transform fieldParent; //parent reference to position field
	
	public void AddField()
	{
		GameObject newField = Object.Instantiate(field) as GameObject;
		newField.name = field.name;
		newField.transform.SetParent(fieldParent, false);
	}
}
