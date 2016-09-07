using UnityEngine;
using System.Collections;

public class Cam_RayDetect : MonoBehaviour {

	//used for detecting artefacts and allowing view of contextual information
	public BrowseCamMovement CamMove;
	public float raycastDistance;
	public string tagCheck = "Active Model";
	private string previousIdentifier; //used to check against duplicate hits
	public GameObject contextInfoPanel;
	public ContextPanel_InfoController ContextInfoCont;
	public ContextPanel_MediaController ContextMediaCont;

	void Start()
	{
		CamMove = gameObject.GetComponentInParent<BrowseCamMovement>();
		previousIdentifier = null;
	}

	void Update()
	{
		if (CamMove.navMode)
		{
			ArtefactDetect();
		}
	}

	private void ArtefactDetect()
	{
		RaycastHit hit;
		bool foundHit = Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance);


		if (foundHit && hit.transform.tag == tagCheck)
		{
			string artefactIdentifier = hit.transform.gameObject.name;
			if (artefactIdentifier != previousIdentifier)
			{
				Debug.Log("Hit artefact: " + artefactIdentifier);
				previousIdentifier = artefactIdentifier;
				contextInfoPanel.SetActive(true);
				ContextInfoCont.LoadData(artefactIdentifier);
			}
		}
	}


	/*

	else if (foundHit) //if the ray hits a valid object
	{
		curObjName = hit.transform.name;
		curObj = hit.transform.gameObject;	
	
		if (!selectPanelActive)// if panel is not active
		{
			activateSelectPanel();
			selectPanelActive = true;
		}		
	}	
}
	*/
}
