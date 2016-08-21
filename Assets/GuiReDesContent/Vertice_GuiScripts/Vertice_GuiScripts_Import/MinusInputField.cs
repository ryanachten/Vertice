using UnityEngine;
using System.Collections;

//reusable script for removing a field from a multiple field metadata attribute

public class MinusInputField : MonoBehaviour {

	// Update is called once per frame
	public void MinusField () 
	{
		GameObject fieldAttr = gameObject.transform.parent.gameObject;
		Destroy(fieldAttr);
	}
}
