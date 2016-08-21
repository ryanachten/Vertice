using UnityEngine;
using System.Collections;

public class PortalTextControl : MonoBehaviour {

	public TextMesh portalLabel;

	// controls the text label for each portal
	public void changeTitle(string portalName)
	{
		//Browse references
		if (portalName == "BrowseTitle_FieldToggle")
		{
			portalLabel.text = "Title";
		}
		else if (portalName == "BrowseCreator_FieldToggle")
		{
			portalLabel.text = "Creator";
		}
		else if (portalName == "BrowseContributor_FieldToggle")
		{
			portalLabel.text = "Contributor";
		}
		else if (portalName == "BrowseDate_FieldToggle")
		{
			portalLabel.text = "Date";
		}
		else if (portalName == "BrowseSubject_FieldToggle")
		{
			portalLabel.text = "Subject";
		}
		else if (portalName == "BrowseCoverage_FieldToggle")
		{
			portalLabel.text = "Coverage";
		}

		//Search references
		else if (portalName == "SearchTitle_FieldToggle")
		{
			portalLabel.text = "Title";
		}
		else if (portalName == "SearchCreator_FieldToggle")
		{
			portalLabel.text = "Creator";
		}
		else if (portalName == "SearchContributor_FieldToggle")
		{
			portalLabel.text = "Contributor";
		}
		else if (portalName == "SearchSubject_FieldToggle")
		{
			portalLabel.text = "Subject";
		}
		else if (portalName == "SearchCoverage_FieldToggle")
		{
			portalLabel.text = "Coverage";
		}
		else if (portalName == "SearchDescription_FieldToggle")
		{
			portalLabel.text = "Description";
		}
//		else
//		{
//			portalLabel.text = portalName;
//		}
	}

}
