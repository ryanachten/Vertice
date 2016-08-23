#pragma strict

var objInfoCont : ObjInfoControl;

var instantParent : Transform; //parent new context media needs to be added below - Context Media Panel
var imgMediaAsset : GameObject;
var vidMediaAsset : GameObject;
var audMediaAsset : GameObject;

function instantImg(){

	var instImg = Instantiate(imgMediaAsset, instantParent.position, instantParent.rotation);
	instImg.transform.SetParent(instantParent, false);
	instImg.transform.SetAsFirstSibling(); //***NEW***
}

function instantXmlImg(){ //instantiated via xml load

	var sourceImgAd = objInfoCont.control.cntxtMediaLocation;
	
	var instImg = Instantiate(imgMediaAsset, instantParent.position, instantParent.rotation);
	instImg.transform.SetParent(instantParent, false);	
	
	var impContScript : ImpContextImg = instImg.GetComponentInChildren(ImpContextImg);
	var imgNameCont : ContextImgTitle = instImg.GetComponentInChildren(ContextImgTitle);
	impContScript.texLocation = sourceImgAd;
	
	impContScript.ContextImgImp();
	imgNameCont.ImgName();
}

#if UNITY_WEBGL
function instantVid(){
	Debug.LogError("InstantContextMedia.instantVid not implemented in WebGL");
	Debug.Break();
}

function instantXmlVid(){
	Debug.LogError("InstantContextMedia.instantXmlVid not implemented in WebGL");
	Debug.Break();
}

#else
function instantVid(){

	var instVid = Instantiate(vidMediaAsset, instantParent.position, instantParent.rotation);
	instVid.transform.SetParent(instantParent, false);
	instVid.transform.SetAsFirstSibling(); //***NEW***	
}

function instantXmlVid(){ //instantiated via xml load

	var sourceVidAd = objInfoCont.control.cntxtMediaLocation;
	
	var instVid = Instantiate(vidMediaAsset, instantParent.position, instantParent.rotation);
	instVid.transform.SetParent(instantParent, false);
	
	var impContScript : ImpContextVid = instVid.GetComponentInChildren(ImpContextVid);
	var vidNameCont : ContextVidTitle = instVid.GetComponentInChildren(ContextVidTitle);
	impContScript.texLocation = sourceVidAd;
	
	impContScript.ContextVidImp();
	vidNameCont.VidName();
}
#endif



function instantAud(){

	var instAud = Instantiate(audMediaAsset, instantParent.position, instantParent.rotation);
	instAud.transform.SetParent(instantParent, false);
	instAud.transform.SetAsFirstSibling(); //***NEW***
}

function instantXmlAud(){ //instantiated via xml load

	var sourceAudAd = objInfoCont.control.cntxtMediaLocation;
	
	var instAud = Instantiate(audMediaAsset, instantParent.position, instantParent.rotation);
	instAud.transform.SetParent(instantParent, false);
	
	var impContScript : ImpContextAudio = instAud.GetComponentInChildren(ImpContextAudio);
	var audNameCont : ContextAudTitle = instAud.GetComponentInChildren(ContextAudTitle);
	impContScript.audioLocation = sourceAudAd;
	
	impContScript.ContextAudioImp();
	audNameCont.AudName();
}