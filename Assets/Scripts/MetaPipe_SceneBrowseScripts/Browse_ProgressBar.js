#pragma strict

import UI;

var progSlider : Slider;
var progressTask : Text;


function setMaxVal( sliderMaxVal : int)
{
	progSlider.maxValue = sliderMaxVal;
	progSlider.value = 0;
}


function AddTask( taskName : String)
{
	progSlider.value++;
	progressTask.text = taskName;
}
