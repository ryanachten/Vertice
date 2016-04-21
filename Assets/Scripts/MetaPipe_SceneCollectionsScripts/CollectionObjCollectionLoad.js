#pragma strict


//loads collection models from xmlNodeList

import System.Xml;

var objInfoControl : ObjInfoControl;
var objInfoContRoot : XmlNode;

var loadPlane : GameObject;

var particleLocate : GameObject;

var progressBar : GameObject;
var progressBarScript : Browse_ProgressBar;
var camMoveScript : CollectionCamMovement;

@HideInInspector
var standardMaterial : Material;



function loadCollectionObjects( listNode : XmlNode)
{
	var loadPlaneBoxCol = loadPlane.GetComponent(BoxCollider);
	var loadPlaneMin = loadPlaneBoxCol.bounds.min;
	var loadPlaneMax = loadPlaneBoxCol.bounds.max;
	//Debug.Log("loadPlaneMin: " + loadPlaneMin + " loadPlaneMax: " + loadPlaneMax);
	
	if (objInfoControl == null)
	{
		Debug.Log("objInfoControl MISSING -> retrieving");
		var gc = GameObject.FindGameObjectWithTag("GameController");
		objInfoControl = gc.GetComponent(ObjInfoControl);
	}
	
	objInfoControl.Start();
	objInfoContRoot = objInfoControl.root;

	var collectionObjects : XmlNodeList = listNode.SelectNodes("MetaPipeObject");
	
	camMoveScript.navLocked = true;
	progressBar.SetActive(true);
	progressBarScript.setMaxVal(collectionObjects.Count);
	
	for (objNode in collectionObjects)
	{
		var curObjNode : XmlNode = objNode as XmlNode;
		
		var curObjName = curObjNode.SelectSingleNode("@name").Value;
	
		progressBarScript.AddTask(curObjName); 
	
		//info contained in ObjArchive XML
		var curObjInfoContNode = objInfoContRoot.SelectSingleNode("MetaPipeObject[@name='"+ curObjName +"']"); 
			var curMeshLocation = curObjInfoContNode.SelectSingleNode("./MeshLocation").InnerText;
			var curTexLocation = curObjInfoContNode.SelectSingleNode("./TexLocation").InnerText;		

		//import mesh
		var importModel : GameObject[] = ObjReader.use.ConvertFile(curMeshLocation, false, standardMaterial);

		for (var model : GameObject in importModel)
		{
			var curObj = model;
			//Add Tag
			curObj.tag = "Active Model";
			curObj.layer = 8;
			curObj.name = curObjName;
			var objTex = curObj.GetComponent(MeshRenderer);		
		
		
			//import texture
			var wwwDirectory = "file://" + curTexLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
			objTex.material.mainTexture = new Texture2D(512, 512, TextureFormat.DXT1, false);
			
			while(true){
				
				var www : WWW = new WWW(wwwDirectory);
				yield www;
				www.LoadImageIntoTexture(curObj.GetComponent.<Renderer>().material.mainTexture);
				if (www.isDone){
					break; //if done downloading image break loop
				}

			}
		}
		
		//assign placement
		var curObjPos : Vector3 = new Vector3();
		var existPosNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjPosition");
			var existPosX = existPosNode.SelectSingleNode("./ObjPosX").InnerText;
			var existPosY = existPosNode.SelectSingleNode("./ObjPosY").InnerText;
			var existPosZ = existPosNode.SelectSingleNode("./ObjPosZ").InnerText;
		
		var existRotateNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjRotation");				
			var existRotateX = existRotateNode.SelectSingleNode("./ObjRotateX").InnerText;
			var existRotateY = existRotateNode.SelectSingleNode("./ObjRotateY").InnerText;
			var existRotateZ = existRotateNode.SelectSingleNode("./ObjRotateZ").InnerText;
			
		var existScaleNode = curObjNode.SelectSingleNode("./ObjTransformInfo/ObjScale");				
			var existScaleX = existScaleNode.SelectSingleNode("./ObjScaleX").InnerText;
			var existScaleY = existScaleNode.SelectSingleNode("./ObjScaleY").InnerText;
			var existScaleZ = existScaleNode.SelectSingleNode("./ObjScaleZ").InnerText;
		
		//if no model settings have been assigned
		if (existPosX == "" && existPosY == "" && existPosY == "")
		{
		
			var randPosX = Random.Range(loadPlaneMin.x, loadPlaneMax.x);
			var randPosZ = Random.Range(loadPlaneMin.z, loadPlaneMax.z);
			Debug.Log("Random start pos for obj: " + randPosX + randPosZ);
			
			curObjPos.x = randPosX;
			curObjPos.z = randPosZ;
			curObj.transform.position = curObjPos;
		
		//if model settings exist
		} else
		{
			//Debug.Log("Existing location");
			
			curObjPos.x = parseFloat(existPosX);
			curObjPos.z = parseFloat(existPosZ);
			curObj.transform.position = curObjPos;
			
			var curObjRotate : Quaternion = new Quaternion();
			curObjRotate.x = parseFloat(existRotateX);
			curObjRotate.y = parseFloat(existRotateY);
			curObjRotate.z = parseFloat(existRotateZ);
			curObj.transform.rotation = curObjRotate;
			
			var curObjScale : Vector3 = new Vector3();
			curObjScale.x = parseFloat(existScaleX);
			curObjScale.y = parseFloat(existScaleY);
			curObjScale.z = parseFloat(existScaleZ);
			curObj.transform.localScale = curObjScale;
		}
		
		var curObjBoxCol = curObj.AddComponent(BoxCollider);
		
		var curPartLocate : GameObject = Instantiate(particleLocate, curObj.GetComponent.<BoxCollider>().bounds.center, Quaternion.identity);
		curPartLocate.transform.parent = curObj.transform;
		
		
		var rb = curObj.AddComponent(Rigidbody);
		rb.useGravity = true;
		rb.isKinematic = false;
	}
	camMoveScript.navLocked = false;
	progressBar.SetActive(false);
}