#pragma strict


//loads collection models from xmlNodeList

import System.Xml;
import Paths;

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

		#if UNITY_WEBGL
		var importModel : GameObject[] = [];
		var importer : ObjReader.ObjData = ObjReader.use.ConvertFileAsync(Paths.Remote + curMeshLocation, false, standardMaterial);
		while (!importer.isDone){
			yield;
		}
		importModel = importer.gameObjects;

		#else
		// TODO This code expects an absolute path to a file, but paths will be relative. A BASE_URL should be declared for WebGL and 
		// the "Archive Folder Location" should be used to alter a BASE_PATH for standalone builds
		var importModel : GameObject[] = [];
		var importer : ObjReader.ObjData = ObjReader.use.ConvertFileAsync(Paths.Local + curMeshLocation, false, standardMaterial);
		while (!importer.isDone){
			yield;
		}
		importModel = importer.gameObjects;

		#endif

		for (var model : GameObject in importModel)
		{
			var curObj = model;
			//Add Tag
			curObj.tag = "Active Model";
			curObj.layer = 8;
			curObj.name = curObjName;
			var objTex = curObj.GetComponent(MeshRenderer);		
		
		
			//import texture
			#if UNITY_WEBGL
			var wwwDirectory = Paths.Remote + curTexLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
			#else
			// TODO This code expects an absolute path to a file, but paths will be relative. A BASE_URL should be declared for WebGL and 
			// the "Archive Folder Location" should be used to alter a BASE_PATH for standalone builds
			var wwwDirectory = Paths.Local + curTexLocation; //this will probably need to change for other OS (PC = file:/ [I think?]) - **REVISE**
			#endif

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

		// TODO This can be replaced with the new CollectionReader and VerticeTransform classes. E.g.

		// Dictionary<string, Dictionary<string, float>> transformData = GetTransformForArtefactWithIdentifierInCollection(string artefactIdentifier, string collectionIdentifier)
		// VerticeTransform transform = new VerticeTransform(transformData);
		// curObj.transform.position = transform.position;
		// curObj.transform.rotation = transform.roation;
		// curObj.transform.localScale = transform.scale;

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
		// TODO This functionality can be replaced by instantiating the VerticeTransform.cs class
		// e.g VerticeTransform randomPosition = new VerticeTransform(loadPlaneMin.x, loadPlaneMax.x, loadPlaneMin.z, loadPlaneMax.z);
		// curObj.transform.position = randomPosition.postion;
		if (existPosX == "" && existPosY == "" && existPosY == "")
		{
		
			var randPosX = Random.Range(loadPlaneMin.x, loadPlaneMax.x);
			var randPosZ = Random.Range(loadPlaneMin.z, loadPlaneMax.z);
			
			curObjPos.x = randPosX;
			curObjPos.z = randPosZ;
			curObjPos.y = 15; //***NEW***
			
			curObj.transform.position = curObjPos;
			
			//Debug.Log("Random start pos for obj: " + " /x: " + curObjPos.x + " /y: " + curObjPos.y + " /z: " + curObjPos.z);
		
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