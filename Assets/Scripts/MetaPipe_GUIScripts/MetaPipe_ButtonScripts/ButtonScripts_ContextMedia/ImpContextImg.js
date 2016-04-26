#pragma strict


//script for importing assets to be associated with imported objects

import UnityEngine.UI;

public var infoCont : ObjInfoControl;
public var contextMediaType : String = "Image"; //new - used to send to media viewer
public var texLocation : String;

public var contextImg : GameObject;
private var contImg : RawImage;
public var contImgTitle : GameObject;
private var imgTex : Texture2D;
private var imgRectTrans : RectTransform;
public var newImgTex : Texture2D;
//private var newImgSprite : Sprite;

private var assetFromFile : boolean;



function Awake(){

	contImg = contextImg.GetComponent(RawImage);
	imgRectTrans = contextImg.GetComponent(RectTransform);	
	
	var gameControl = GameObject.FindGameObjectWithTag("GameController");
	infoCont = gameControl.GetComponent.<ObjInfoControl>();
}



public function OpenImgAsset(){ //**REVISE** this function is pretty redundant
	
	//Filter Files
	var fileExtensions : String[] = [".jpg"]; //***NEW***
	UniFileBrowser.use.SetFileExtensions(fileExtensions); //***NEW***
	
	UniFileBrowser.use.OpenFileWindow(ImgAssetFile);
}



public function ImgAssetFile(pathToTex : String){
	
	texLocation = pathToTex;
	ContextImgImp();
	
	assetFromFile = true;
	
}

public function ContextImgImp(){

	//Debug.Log("Context Img Located at: " + texLocation);
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**	
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);

		yield www;
		
		var texWidth = www.texture.width;
		var texHeight = www.texture.height;
		
//		Debug.Log("texWidth: " + texWidth + " texHeight: " + texHeight);
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}

	//check img sizes
	var imgRect : RectTransform = contextImg.GetComponent.<RectTransform>();
	var mvMaxWidth = imgRect.rect.width;
	var mvMaxHeight = imgRect.rect.height;
//	Debug.Log("mvMaxWidth: " + mvMaxWidth + " mvMaxHeight: " + mvMaxHeight);
	
	if (texHeight > mvMaxHeight) //if img height is greater than max mv max height
	{
		//Debug.Log("Img.height too big for MV.height: downscaling");
		
		var scaleHFactor = mvMaxHeight /texHeight;
		//Debug.Log("scaleHFactor: " + scaleHFactor);
		
		texHeight = texHeight * scaleHFactor;
		texWidth = texWidth * scaleHFactor;			
	}
	if (texWidth > mvMaxWidth) //if img width is greater than max mv max width
	{
		//Debug.Log("Img.width too big for MV.width: downscaling");
		
		var scaleWFactor = mvMaxWidth /texWidth;
		//Debug.Log("scaleWFactor: " + scaleWFactor);
		
		texHeight = texHeight * scaleWFactor;
		texWidth = texWidth * scaleWFactor;
	}
	
	
	//Assigning to RawImg comp
	var newImgTex = new Texture2D(texWidth, texHeight); //HERE - need to change this to be further down - create size using texWidth and texHeight
	www.LoadImageIntoTexture(newImgTex);
	var imageRender : UI.RawImage = contextImg.GetComponent.<RawImage>();
	imageRender.texture = newImgTex;	
	
//	//Resize RawImg rect
	var layoutElement = contextImg.GetComponent.<LayoutElement>();
	layoutElement.minHeight = texHeight;	
	
	//***HERE***	
	if (assetFromFile){
		ImgNameSplit(); //only execute namesplit if from file
		
		//execute add create node for the context media
		infoCont.CreateContextNode("Image");
		infoCont.Save(); //**new** for autosave
	}
}


function ImgNameSplit(){

	var directorySplit : String[];
	directorySplit = texLocation.Split("/"[0]);
	var imgFileName = directorySplit[directorySplit.Length-1]; //original file name
	
	var imgNameSplit = imgFileName.Split("."[0]); 
	var imgName = imgNameSplit[imgNameSplit.Length-2]; //object name minus extension
	
	infoCont.control.cntxtMediaName = imgName;
	infoCont.control.cntxtMediaLocation = texLocation;
	
	
	var imgNameScript = contImgTitle.GetComponent(ContextImgTitle);
	imgNameScript.ImgName();
	
	
	//Debug.Log("Context Img Name: " + imgName);
	
	assetFromFile = false; //reset after import from file
}


function sendActive() //used to activate media viewer
{
	var mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
	var mediaActiveScript = mediaViewer.GetComponent.<MetaPipe_MediaV_Activate>();

	//Debug.Log("contextMediaType: " + contextMediaType + " texLocation: " + texLocation);
//	mediaActiveScript.activeMediaViewer(contextMediaType, texLocation);
	var imgTitle = contImgTitle.GetComponent.<Text>().text;
	Debug.Log("imgTitle: " + imgTitle);
	mediaActiveScript.activeMediaViewer(imgTitle, contextMediaType, texLocation);
}