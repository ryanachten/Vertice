using UnityEngine;
using System.Collections;

public class Collect_ModifyHelp : MonoBehaviour {

	public GameObject helpPanel;
	public GameObject moveHelp;
	public GameObject rotateHelp;
	public GameObject scaleHelp;

	public void ActivateModifyHelp(string helpMode)
	{
		if(helpMode == "move")
		{
			moveHelp.SetActive(true);
			rotateHelp.SetActive(false);
			scaleHelp.SetActive(false);
		}
		else if(helpMode == "rotate")
		{
			moveHelp.SetActive(false);
			rotateHelp.SetActive(true);
			scaleHelp.SetActive(false);
		}
		else if(helpMode == "scale")
		{
			moveHelp.SetActive(false);
			rotateHelp.SetActive(false);
			scaleHelp.SetActive(true);
		}
	}
}
