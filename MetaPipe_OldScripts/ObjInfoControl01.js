#pragma strict

//Unity tutorial for saving and loading data from scene to scene
//'Unity Data Persistence' tutorial

//import System; //DUNNO IF NECESSARY IN JS
import System.Runtime.Serialization.Formatters.Binary;
import System.IO;


public class ObjInfoControl extends MonoBehaviour{

	public var health : float;
	public var experience : float;

	public var objName : String; //imported object name
	public var fileName : String; //original file name
	
	
	public static var control : ObjInfoControl; //this will be used to make sure there is only one present in scene
	

	function Awake () {

		if (control == null){

			DontDestroyOnLoad(gameObject); //whatever gameobject this is on will persist from scene to scene
			control = this; //this becomes the one object
			
		} 
		else if ( control != this){
		
			Destroy(gameObject);
		}
	}


	function OnGUI () { //even though not using GUI references this seems to work well
		
		//text display for output
		var healthText = gameObject.Find("Health Test").GetComponent(Text);
		var expText = gameObject.Find("Experience Test").GetComponent(Text);
		
		var objNameText = gameObject.Find("ObjectNameText").GetComponent(Text);
		var fileNameText = gameObject.Find("FileNameText").GetComponent(Text);
		
		//Assign class contents to text output
		healthText.text = "Health: " + health;
		expText.text = "Experience: " + experience;
		
		objNameText.text = objName;
		fileNameText.text = fileName;
		
	}


	public function Save(){ //this could be changed to OnDisable for autosave
		
		var bf = new BinaryFormatter();
		var file = File.Create(Application.persistentDataPath + "/objectInfo.dat"); //the file name "/objectInfo.dat" can be changed obvs // this might need to be changed to merge existing file

		var data = new ObjectData(); //to assign local data to serialisable class
		
		
		//serailise class contents
		data.health = health;
		data.experience = experience;
		
		data.objName = objName;
		data.fileName = fileName;
		
		
		bf.Serialize(file, data); //serialise data to file
		file.Close();
	}
	
	
	public function Load(){ //this could be used in OnEnable for autoload
	
		if(File.Exists(Application.persistentDataPath + "/objectInfo.dat")){ //makes sure the file exists before loading it
			
			var bf = new BinaryFormatter();
			var file = File.Open(Application.persistentDataPath + "/objectInfo.dat", FileMode.Open); //added File.Mode to resolve issue - dunno if this is correct
			var data = bf.Deserialize(file) as ObjectData; //need to (cast) the object pulled from file so unity can process it inthis case read as ObjectData object
			file.Close();
			
			health = data.health;
			experience = data.experience;
					
			objName = data.objName;
			fileName = data.fileName;
		}
	}
}


class ObjectData { //clean class to be oututted w/o MonoBehaviour

	//this data will be serialised
	public var health : float;
	public var experience : float;
	
	public var objName : String; 	
	public var fileName : String; 	

}