using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collect_DisableNav : MonoBehaviour {

	private BrowseCamMovement CamMove;
	private Collect_RaycastModifyIncrement ModifyArtefact;

	void Start()
	{
		InputField field = gameObject.GetComponent<InputField>();
		field.onValueChange.AddListener (delegate {DisableNavigation ();});
		field.onEndEdit.AddListener (delegate {EnableNavigation ();});

		CamMove = GameObject.FindGameObjectWithTag("Player").GetComponent<BrowseCamMovement>();
//		ModifyArtefact = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Collect_RaycastModifyIncrement>();
	}

	void DisableNavigation()
	{
		Collect_RaycastModifyIncrement.canModify = false;

//		Debug.Log("ModifyArtefact.canModify: [F]" + Collect_RaycastModifyIncrement.canModify);
//		ModifyArtefact.enabled = false;
//		CamMove.navMode = true;
	}

	void EnableNavigation()
	{
		Collect_RaycastModifyIncrement.canModify = true;
//		Debug.Log("ModifyArtefact.canModify [T]: " + Collect_RaycastModifyIncrement.canModify);
//		CamMove.navMode = false;

	}
}
