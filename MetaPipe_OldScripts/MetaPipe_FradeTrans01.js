#pragma strict

var fadeTex : Texture;
var startColour : Color(0,0,0, 1);
var endColour : Color(0,0,0, 0);
var duration : float = 4.0;
internal var curColour : Color;



function Start () {
	
	curColour = startColour;
	


}


function OnGUI(){
	
	GUI.depth = -10; //makes sure fade trans goes on top of everything
	GUI.color = curColour;
	GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeTex);

}


function FixedUpdate(){
	
	curColour = Color.Lerp(startColour, endColour, Time.time /duration);



}