#pragma strict

var browseContextInfo = BrowseContextInfoXml;

var instantParent : Transform; //parent new context media needs to be added below
var imgMediaAsset : GameObject;
var vidMediaAsset : GameObject;
var audMediaAsset : GameObject;

function instantXmlImg(imgName : String, sourceImgAd : String){ //instantiated via xml load
	
	var instImg = Instantiate(imgMediaAsset, instantParent.position, instantParent.rotation);
	instImg.transform.SetParent(instantParent, false);
	
	var impContScript : BrowseImpContextImg = instImg.GetComponentInChildren(BrowseImpContextImg);
	var imgNameText : Text = instImg.GetComponentInChildren(Text);
	imgNameText.text = imgName;
	
//	impContScript.ContextImgImp(sourceImgAd);
}

function clearChildren()
{
	for (var i = instantParent.childCount - 1; i >= 0; i--)
	{
		//GameObject.Destroy(instantParent.GetChild(i).gameObject);
		var curGo = instantParent.GetChild(i).gameObject;
		if (curGo.transform.tag == "Context Media Asset")
		{
			GameObject.Destroy(curGo);
		
		}
	}
	//instantParent.DetachChildren();
}

function instantXmlAud(audName: String, sourceAudAd : String){ //instantiated via xml load
	
	var instAud = Instantiate(audMediaAsset, instantParent.position, instantParent.rotation);
	instAud.transform.SetParent(instantParent, false);
	
	var impContScript : BrowseImpContextAudio = instAud.GetComponentInChildren(BrowseImpContextAudio);
	var audNameText : Text = instAud.GetComponentInChildren(Text);
	audNameText.text = audName;

	impContScript.ContextAudioImp(sourceAudAd);
}

#if UNITY_WEBGL

function instantXmlVid(vidName : String, sourceVidAd : String){
	Debug.LogError("BrowseContextMediaInstance.instantXmlVid not implemented in WebGL");
	Debug.Break();
}

#else


function instantXmlVid(vidName : String, sourceVidAd : String){ //instantiated via xml load
	
	var instVid = Instantiate(vidMediaAsset, instantParent.position, instantParent.rotation);
	instVid.transform.SetParent(instantParent, false);
	
	var impContScript : BrowseImpContextVid = instVid.GetComponentInChildren(BrowseImpContextVid);
	var vidNameText : Text = instVid.GetComponentInChildren(Text);
	vidNameText.text = vidName;
	
	impContScript.ContextVidImp(sourceVidAd);
}

#endif