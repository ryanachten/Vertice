#pragma strict

	//Tri Slider Report
public var triSliderVal : Slider;
public var objReport : ObjReport; //holds objReport script 

public var triSlideBg : GameObject;
public var triSlideBgImg : Image;

public var triSlideFill : GameObject;
public var triSlideFillImg : Image;

public var triCount : int;

public var liBlue : Color;
public var medBlue : Color;
public var liGreen : Color;
public var medGreen : Color;

function Awake(){

	triSliderVal = gameObject.GetComponent(Slider);
	triSlideBgImg = triSlideBg.GetComponent(Image);	
	triSlideFillImg = triSlideFill.GetComponent(Image);
}



function triSlideUpdate(){
	
	//Report to slider
	triCount = objReport.triCount;
	triSliderVal.value = triCount;

	//TriSlide Colours
	if (triCount < 10000){ //Light Weight Model
		Debug.Log("Model Tri: Real time ready (10,000 -> 30,000)");
		triSlideBgImg.color = liBlue;
		triSlideFillImg.color = medBlue;
	}
	if (triCount > 10000 && triCount < 30000){ //Real time ready bracket
		Debug.Log("Model Tri: Real time ready (10,000 -> 30,000)");
		triSlideBgImg.color = Color(0.7, 0.9, 0.8);
		triSlideFillImg.color = Color(0.3, 0.8, 0.6);
	} 
}
	
		
			
				
						