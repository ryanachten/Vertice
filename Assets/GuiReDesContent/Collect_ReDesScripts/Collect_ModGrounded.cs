using UnityEngine;
using System.Collections;

public class Collect_ModGrounded : MonoBehaviour {

	public bool grounded;

	void Start()
	{
//		grounded = true;
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.tag == "Terrain")
		{
			grounded = true;
		}
	}

	void OnCollisionStay(Collision collision)
	{
		if(collision.transform.tag == "Terrain")
		{
			grounded = true;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if(collision.transform.tag == "Terrain")
		{
			grounded = false;
		}
	}
}
