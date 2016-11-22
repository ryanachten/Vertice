#pragma strict

import UnityEngine;

private static var Remote = "https://s3-ap-southeast-2.amazonaws.com/vertice-dev";
private static var Local = Application.dataPath;

#if UNITY_WEBGL
public static var CollectionMetadata = Remote + "/Metadata/Vertice_CollectionMetadata.xml";
public static var ArtefactMetadata = Remote + "/Metadata/Vertice_ArtefactMetadata.xml";
public static var VerticeArchive = Remote; //+ "/VerticeArchive";

public static function PathToFile(fileLocation : String)
{	
	var verticeFileLocation = VerticeArchive + fileLocation;
	return verticeFileLocation;
}


#elif UNITY_STANDALONE
public static var CollectionMetadata = Local + "/Vertice_CollectionMetadata.xml";
public static var ArtefactMetadata = Local + "/Vertice_ArtefactMetadata.xml";
public static var VerticeArchive;

public static function PathToFile(fileLocation : String)
{	
	var filePrefix : String;

	#if UNITY_STANDALONE_WIN
	filePrefix = "file://c:/"; //TODO test: Windows directory should be something like "file://c://Users/"
	#else
	filePrefix = "file://"; //OSX diretory: "file:///Users/"
	#endif

	var verticeFileLocation = filePrefix + VerticeArchive + fileLocation;
//	Debug.Log("verticeFileLocation: " + verticeFileLocation);
	return verticeFileLocation;
}
#endif