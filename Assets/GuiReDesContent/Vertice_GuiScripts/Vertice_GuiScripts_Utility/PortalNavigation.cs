using UnityEngine;
using System.Collections;

public class PortalNavigation : MonoBehaviour {

	public string portalDestination;
	public Vertice_LoadScene LoadScene;
	
	void OnTriggerEnter(Collider otherCol) {

		if (otherCol.tag == "Player")
		{
			if(portalDestination == "import")
			{
				LoadScene.loadImport();
			}
			else if (portalDestination == "browse")
			{
				LoadScene.loadBrowse();
			}
			else if (portalDestination == "collect")
			{
				LoadScene.loadCollection();
			}
		}
	}
}
