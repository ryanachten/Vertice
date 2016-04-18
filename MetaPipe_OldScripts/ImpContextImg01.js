#pragma strict


//script for importing assets to be associated with imported objects

import UnityEngine.UI;

public var infoCont : ObjInfoControl;
public var texLocation : String;

public var contextImg : GameObject;
private var contImg : Image;
public var contImgTitle : GameObject;
private var imgTex : Texture2D;
private var imgRectTrans : RectTransform;
public var newImgTex : Texture2D;
private var newImgSprite : Sprite;

private var assetFromFile : boolean;



function Awake(){

	contImg = contextImg.GetComponent(Image);
	imgRectTrans = contextImg.GetComponent(RectTransform);	
	
	var gameControl = GameObject.FindGameObjectWithTag("GameController");
	infoCont = gameControl.GetComponent(ObjInfoControl);
}



public function OpenImgAsset(){ //**REVISE** this function is pretty redundant
	
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
	newImgTex = new Texture2D(512, 512, TextureFormat.DXT1, false);
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);
		
		yield www;
	
		Debug.Log("Done Downloading Texture");
		www.LoadImageIntoTexture(newImgTex);
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}	
	
	var imgSprite = contImg.sprite;
	var imgRect = imgSprite.rect;
	var imgPivot = imgRectTrans.pivot;
	var pixelScale = 100;
	Debug.Log("Pixel Scale:" + pixelScale);
	
	newImgSprite = Sprite.Create(newImgTex, imgRect, imgPivot, pixelScale);
	contImg.sprite = newImgSprite;

	//imgRectTrans.rect.size = Vector2(130,130);
	//contImg.sprite.rect.size = Vector2(130,130);
	//contImg.type = Image.Type.Simple;
	
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
	
	
	Debug.Log("Context Img Name: " + imgName);
	
	assetFromFile = false; //reset after import from file
}