#pragma strict

//script for importing img to Browse GUI prefab

import UnityEngine.UI;

//public var infoCont : ObjInfoControl;
//public var texLocation : String;
public var contextMediaType : String = "Image"; //new - used to send to media viewer

public var textureLocation : String; //new



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
}


public function ContextImgImp(texLocation){

	//Debug.Log("Context Img Located at: " + texLocation);
	textureLocation = texLocation;
	var wwwDirectory = "file://" + texLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
		
	newImgTex = new Texture2D(512, 512, TextureFormat.DXT1, false);
	
	while(true){
	
		var www : WWW = new WWW(wwwDirectory);
		
		yield www;
	
//		Debug.Log("Done Downloading Texture");
		www.LoadImageIntoTexture(newImgTex);
		
		if (www.isDone){
			break; //if done downloading image break loop
		}
	}	
	
	var imgSprite = contImg.sprite;
	var imgRect = imgSprite.rect;
	var imgPivot = imgRectTrans.pivot;
	var pixelScale = 100;
	//Debug.Log("Pixel Scale:" + pixelScale);
	
	newImgSprite = Sprite.Create(newImgTex, imgRect, imgPivot, pixelScale);
	contImg.sprite = newImgSprite;

	//imgRectTrans.rect.size = Vector2(130,130);
	//contImg.sprite.rect.size = Vector2(130,130);
	//contImg.type = Image.Type.Simple;
}


function sendActive() //new
{
	var mediaViewer = GameObject.FindGameObjectWithTag("MediaViewer");
	var mediaActiveScript = mediaViewer.GetComponent.<MetaPipe_MediaV_Activate>();

	mediaActiveScript.activeMediaViewer(contextMediaType, textureLocation);
}