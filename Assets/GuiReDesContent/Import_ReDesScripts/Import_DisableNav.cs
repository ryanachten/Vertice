using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Import_DisableNav : MonoBehaviour {

	private Import_CamMove CamMove;

	void Start()
	{
		InputField field = gameObject.GetComponent<InputField>();
		field.onValueChange.AddListener (delegate {DisableNavigation ();});
		field.onEndEdit.AddListener (delegate {EnableNavigation ();});

		CamMove = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Import_CamMove>();
	}

	void DisableNavigation()
	{
		CamMove.GuiMode = true;
	}

	void EnableNavigation()
	{
		CamMove.GuiMode = false;
	}
}
