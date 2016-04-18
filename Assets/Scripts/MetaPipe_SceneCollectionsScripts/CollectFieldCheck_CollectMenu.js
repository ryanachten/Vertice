#pragma strict

//used to detect whether or not fields in the Import scene have lost their connection to the GameController (due to scene change delete etc)
//will reconnect if this is the case

//held on Obj Info Panel

var collectName : InputField;
var collectCreator : InputField;
var collectDescript : InputField;

var collectGuiCont : CollectionGUIcontrol;
var loadCollectButton : CollectionLoadCollectionAssets;

function Start () {
	
	UpdateFieldActions();
	
	if (loadCollectButton == null)
	{
		loadCollectButton = GameObject.Find("MediaAssets").GetComponent.<CollectionLoadCollectionAssets>();
	}
}

function UpdateFieldActions () {
	
	if (collectGuiCont == null)
	{
		collectGuiCont = gameObject.GetComponent.<CollectionGUIcontrol>();	
	}
	
	
	collectName.onEndEdit.AddListener(function()
	{
		collectGuiCont.changeCollectionTitle();
		loadCollectButton.getCollectionList(); //updates available collections
	});
	
	
	collectCreator.onEndEdit.AddListener(function()
	{
		collectGuiCont.changeCollectionCreator();	
	});
	
	collectDescript.onEndEdit.AddListener(function()
	{
		collectGuiCont.changeCollectionDescription();	
	});
}

