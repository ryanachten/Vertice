#pragma strict

import UnityEngine;

public static var Remote = "https://s3-ap-southeast-2.amazonaws.com/vertice-dev";
public static var Local = Application.dataPath;

#if UNITY_WEBGL
public static var CollectionMetadata = Remote + "/Vertice_CollectionMetadata.xml";
public static var ArtefactMetadata = Remote + "/Metadata/Vertice_ArtefactInformation.xml";
public static var VerticeArchive = Remote; //+ "/VerticeArchive";

#elif UNITY_STANDALONE
public static var CollectionMetadata = Local + "/Vertice_CollectionMetadata.xml";
public static var ArtefactMetadata = "file://" + Local + "/Vertice_ArtefactMetadata.xml";
public static var VerticeArchive = "file:///Users/ryanachten/Documents/UnityTests"; //FIXME needs to be changed
#endif