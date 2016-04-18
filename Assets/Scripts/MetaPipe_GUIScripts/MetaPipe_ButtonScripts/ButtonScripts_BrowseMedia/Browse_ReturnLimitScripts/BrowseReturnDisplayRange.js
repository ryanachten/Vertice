#pragma strict

//used to control the displaying of the import limit range and adjust the range being displayed

//range vals
var minRangeVal : int;
var maxRangeVal : int;

var minValText : Text;
var maxValText : Text;
var totalRangeText : Text;

var slider : Slider;
var sliderVal : int;

var browseImprtObjScript : BrowseImportObj;
var impLimitRangeMin : int; //from importObj script
var totalBrowseResults : int;



function OnGUI()
{
	minRangeVal = browseImprtObjScript.impLimitRangeMin;
	minValText.text = minRangeVal.ToString();
	
	maxRangeVal = browseImprtObjScript.impLimitRangeMax;
	maxValText.text = maxRangeVal.ToString();
	
	totalBrowseResults = browseImprtObjScript.totalResultsCount;
	totalRangeText.text = totalBrowseResults.ToString() + " results";
	
}
