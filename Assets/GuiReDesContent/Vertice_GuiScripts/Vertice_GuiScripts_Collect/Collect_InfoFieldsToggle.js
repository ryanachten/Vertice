#pragma strict

//activates and controls the Collection Field panel
//TODO add functionality to draw from metadata reader

public var viewCollectionsPanel : GameObject; //view collections panel
public var collectFieldPanel : GameObject; //metadata field panel


function ActivateFields()
{
	if (viewCollectionsPanel.activeSelf)
	{
		viewCollectionsPanel.SetActive(false);
	}
	collectFieldPanel.SetActive(true);
}