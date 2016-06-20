#pragma strict


var text : TextMesh;


function updateText(groupName : String)
{
	if (groupName.Length > 3)
	{
		text.text = groupName;	
	}
}