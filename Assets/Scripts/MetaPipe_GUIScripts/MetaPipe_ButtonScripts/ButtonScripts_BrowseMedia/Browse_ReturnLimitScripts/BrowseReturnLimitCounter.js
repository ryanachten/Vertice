#pragma strict

var slider : Slider;
var sliderVal : int;

var counterText : Text;


function OnGUI()
{
	sliderVal = slider.value;
	counterText.text = sliderVal.ToString();
}