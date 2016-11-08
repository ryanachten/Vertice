using UnityEngine;
using System.Collections;

public class WebGl_HideObjects : MonoBehaviour {

	public GameObject[] hideObjects;

	void Start () {
		#if UNITY_WEBGL
		for (int i = 0; i < hideObjects.Length; i++) {
			hideObjects[i].SetActive(false);
		}
		#endif
	}
}
