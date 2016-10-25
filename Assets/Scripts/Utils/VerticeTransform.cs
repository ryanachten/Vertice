using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct VerticeTransform {

	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;


	/// <summary>
	/// Initialise a Vertice Transform with the passed in position, scale, and rotation coordinates
	/// </summary>
	/// <param name="positionX">Position x.</param>
	/// <param name="positionY">Position y.</param>
	/// <param name="positionZ">Position z.</param>
	/// <param name="rotationX">Rotation x.</param>
	/// <param name="rotationY">Rotation y.</param>
	/// <param name="rotationZ">Rotation z.</param>
	/// <param name="scaleX">Scale x.</param>
	/// <param name="scaleY">Scale y.</param>
	/// <param name="scaleZ">Scale z.</param>
	public VerticeTransform(float positionX, float positionY, float positionZ, float rotationX, float rotationY, float rotationZ, float rotationW, float scaleX, float scaleY, float scaleZ){

		position = new Vector3 (positionX, positionY, positionZ);
		rotation = new Quaternion (rotationX, rotationY, rotationZ, rotationW);
		scale = new Vector3 (scaleX, scaleY, scaleZ);

	}

	/// <summary>
	/// Initialise a VerticeTransform with the passed in Position, Scale, and Rotation Unity objects. Useful for packaging transform data to pass to the CollectionWriter
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	/// <param name="scale">Scale.</param>
	public VerticeTransform(Vector3 position, Quaternion rotation, Vector3 scale) {
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
	}

	/// <summary>
	/// Convenience initialiser for VerticeTransform that will unpack a nested dictionary produced by CollectionReader
	/// </summary>
	/// <param name="transformData">A nested dictionary of the form:
	/// 
	/// {
	/// 	'position': {
	/// 		'x': #.#f
	/// 		'y': #.#f
	/// 		'z': #.#f
	/// 	}
	/// 
	/// 	'rotation': {
	/// 		'x': #.#f
	/// 		'y': #.#f
	/// 		'z': #.#f
	/// 	}
	/// 
	/// 	'scale': {
	/// 		'x': #.#f
	/// 		'y': #.#f
	/// 		'z': #.#f
	/// 	}
	/// 
	/// }
	/// 
	/// </param>
	public VerticeTransform(Dictionary<string, Dictionary<string, float>> transformData){

		// Unpack position coordinates
		float posX = transformData["position"]["x"];
		float posY = transformData["position"]["y"];
		float posZ = transformData["position"]["z"];

		// Unpack rotation coordinates
		float rotX = transformData["rotation"]["x"];
		float rotY = transformData["rotation"]["y"];
		float rotZ = transformData["rotation"]["z"];
		float rotW = transformData["rotation"]["w"];

		// Unpack scale coordinates
		float scaleX = transformData["scale"]["x"];
		float scaleY = transformData["scale"]["y"];
		float scaleZ = transformData["scale"]["z"];

		position = new Vector3 (posX, posY, posZ);
		rotation = new Quaternion (rotX, rotY, rotZ, rotW);
		scale = new Vector3 (scaleX, scaleY, scaleZ);


	}

	/// <summary>
	/// Initializes a new instance of the <see cref="VerticeTransform"/> class, where rotation and scale are identity transformations (i.e. no change in rotation or scale occurs), 
	/// and position is set to a random position on the passed in plane.
	/// </summary>
	/// <param name="xMin">The minimum x coordinate of the plane</param>
	/// <param name="xMax">The maximum x coordinate of the plane</param>
	/// <param name="zMin">The minimum z coordinate of the plane</param>
	/// <param name="zMax">The maximum z coordinate of the plane</param>
	/// <param name="y">The y coordinate; defaults to 15</param>
	public VerticeTransform(float xMin, float xMax, float zMin, float zMax, float y = 15.0f){

		// Set position to a random point on the plane
		position = new Vector3 (Random.Range (xMin, xMax), y, Random.Range (zMin, zMax));

		// Set rotation to the identitity
		rotation = Quaternion.identity;

		// Set scale to the identity
		scale = Vector3.one;

	}


}
