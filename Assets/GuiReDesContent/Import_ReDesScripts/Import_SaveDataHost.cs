using System.Collections.Generic;

public struct ArtefactSaveData
{
	public static string MeshLocation;
	public static string TexLocation;

	public static List<Dictionary<string, string>> ContextualMediaAssets; //TODO convert this to an array later


	static public void ClearSaveData()
	{
		ArtefactSaveData.MeshLocation = null;
		ArtefactSaveData.TexLocation = null;
		ArtefactSaveData.ContextualMediaAssets = new List<Dictionary<string, string>>();
	}
}

