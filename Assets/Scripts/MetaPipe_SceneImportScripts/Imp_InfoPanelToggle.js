#pragma strict


var artifactInfoPanel : GameObject;
var modelInfoPanel : GameObject;
var contextMediaPanel : GameObject;

var photogramPanel : GameObject;
var designPanel : GameObject;

function panelToggle (activePanel : String)
{
	if (activePanel == "artifact")
	{
		modelInfoPanel.SetActive(false);
		contextMediaPanel.SetActive(false);
		photogramPanel.SetActive(false);
		designPanel.SetActive(false);	
	}
	
	if (activePanel == "model")
	{
		artifactInfoPanel.SetActive(false);
		contextMediaPanel.SetActive(false);
		photogramPanel.SetActive(false);
		designPanel.SetActive(false);	
			
	}
	
	if (activePanel == "context")
	{
		artifactInfoPanel.SetActive(false);
		modelInfoPanel.SetActive(false);
		photogramPanel.SetActive(false);
		designPanel.SetActive(false);	 	
	}
	
	if (activePanel == "photo")
	{
//		Debug.Log("activePanel == photo");
		artifactInfoPanel.SetActive(false);
		modelInfoPanel.SetActive(false);
		contextMediaPanel.SetActive(false);
		designPanel.SetActive(false);	 	
	}
	
	if (activePanel == "design")
	{
//		Debug.Log("activePanel == design");
		artifactInfoPanel.SetActive(false);
		modelInfoPanel.SetActive(false);
		contextMediaPanel.SetActive(false);
		photogramPanel.SetActive(false);	 	
	}
}