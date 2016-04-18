#pragma strict


var collectInfoPanel : GameObject;




function activeCollectInfo()
{
	if (collectInfoPanel.activeSelf == false)
	{
//		Debug.Log("Collect info on");
		collectInfoPanel.SetActive(true);
	}
	
	else if (collectInfoPanel.activeSelf == true)
	{
//		Debug.Log("Collect info off");
		collectInfoPanel.SetActive(false);
	}
}
