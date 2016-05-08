#pragma strict

var inputField : GameObject;
var userText : Text;
var collectionControl : ObjCollectionControl;
var loadCollectionAssetsScript : BrowseLoadCollectionAssets;

var collectionFeedback : FeedbackScript; //***NEW***

function activateInputField() //input field used to define collection name
{
	if (!inputField.activeSelf)
	{
		inputField.SetActive(true);	
	}		
}

function sendCollectionToAdd()
{
	if (collectionControl == null)
	{
		Debug.Log("collectionControl == NULL");
		collectionControl = GameObject.FindGameObjectWithTag("GameController").GetComponent.<ObjCollectionControl>();	
	}

	var collectionName = userText.text;
	if (collectionName.Length > 3)
	{
		collectionControl.addNewCollection(collectionName);
		loadCollectionAssetsScript.getCollectionList();
	}
	inputField.SetActive(false);
	
	collectionFeedback.Feedback(); //***NEW***		
}