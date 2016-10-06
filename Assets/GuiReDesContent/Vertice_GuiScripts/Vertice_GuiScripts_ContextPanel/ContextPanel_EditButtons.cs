using UnityEngine;
using System.Collections;

public class ContextPanel_EditButtons : MonoBehaviour {

	void Start () 
	{
		#if UNITY_WEBGL
		gameObject.SetActive(false);
		#endif
	}
}
