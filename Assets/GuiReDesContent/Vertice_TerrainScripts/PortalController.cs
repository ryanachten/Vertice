using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PortalController : MonoBehaviour
{
	public Vector3 centerPos = new Vector3 (0, 0, 0);    //center of circle/elipsoid
	
	public GameObject portal;                    //generic prefab to place on each point
	public float radiusX, radiusY;                    //radii for each x,y axes, respectively
	Vector3 pointPos;                                //position to place each prefab along the given circle
	
	public GameObject browseToggles;
	public GameObject searchToggles;
	GameObject toggleParent;

	public List<string> activeAttributes;



	public void ChangePortalCount (string queryType) //placeholder functionality using toggles
	{
		if (queryType == "browse")
			toggleParent = browseToggles;
		else if (queryType == "search")
			toggleParent = searchToggles;

		activeAttributes = new List<string>(); //resets list between browses

 		for (int i = 0; i < toggleParent.transform.childCount; i++) {
 			
			Toggle curToggle = toggleParent.transform.GetChild(i).GetComponent<Toggle>();
		
			if (curToggle.isOn)
			{
				activeAttributes.Add(curToggle.name);
			}
		}
		if (activeAttributes.Count > 0)
		{
			InstantPortals(activeAttributes);
		}
	}

	
	void InstantPortals ( List<string> activeAttr)
	{
		for (int j = 0; j < transform.childCount; j++) {
			GameObject existPortal = transform.GetChild(j).gameObject;
			Destroy(existPortal);
		}

		for (int i = 0; i < activeAttr.Count; i++) {

			float pointNum = (i * 1.0f) / activeAttr.Count; //multiply 'i' by '1.0f' to ensure the result is a fraction
			float angle = pointNum * Mathf.PI * 2; //angle along the unit circle for placing points
			
			float x = Mathf.Sin (angle) * radiusX;
			float y = Mathf.Cos (angle) * radiusY;

			pointPos = new Vector3 (x, 0, y) + centerPos;
			
			//place the prefab at given position
			GameObject curPortal = Instantiate (portal, pointPos, Quaternion.LookRotation(pointPos)) as GameObject;
			curPortal.transform.SetParent(transform);

//			Debug.Log("List item:" + activeAttr[i]);

			curPortal.GetComponent<PortalTextControl>().changeTitle(activeAttr[i]);
		}

		radiusY = radiusX;
	}
}
