using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cam_RayDetect : MonoBehaviour {

	//used for detecting artefacts and allowing view of contextual information
	public BrowseCamMovement CamMove;
	public float raycastDistance = 10f;
	public string tagCheck = "Active Model";
	private string previousIdentifier; //used to check against duplicate hits
	public GameObject contextInfoPanel;
	public ContextPanel_InfoController ContextInfoCont;
	public Toggle infoToggle;
	public Toggle mediaToggle;
	public GameObject mediaViewer;

	public GameObject curArtefact; //used by Collect_RayCastModifyArtefact TODO not sure if this best way to get artefact into mod script


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
				curArtefact = hit.transform.gameObject; //TODO not sure if this best way to get artefact into mod script

				previousIdentifier = artefactIdentifier;
				contextInfoPanel.SetActive(true);
				infoToggle.isOn = true;
				mediaToggle.isOn = false;
				ContextInfoCont.LoadData(artefactIdentifier);
			}
		} else //if not an artefact
		{
			previousIdentifier = null;
			if (contextInfoPanel.activeSelf)
				contextInfoPanel.SetActive(false);
			if (mediaViewer.activeSelf)
			{
				mediaViewer.SetActive(false);
			}
		}
	}
}
