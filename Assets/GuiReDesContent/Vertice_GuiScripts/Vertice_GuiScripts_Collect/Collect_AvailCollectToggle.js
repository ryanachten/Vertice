#pragma strict

//activates and controls the Available Collection panel
//TODO connect to metadata reading functionaltiy


public var viewCollectionsPanel : GameObject; //view collections panel
public var collectFieldPanel : GameObject; //metadata field panel


function ViewCollections() //activates panel on button press
{	
	if (collectFieldPanel.activeSelf)
	{
		collectFieldPanel.SetActive(false); //turn field panel off to avoid overlaying panels
	}

	viewCollectionsPanel.SetActive(true);
}