using UnityEngine;
using System.Collections;

//_VertexFollower
//_Dependent to: MeshFilter
//_Doesn't Work!

// This script force object to follow nearest vertex from parent object.


public class VertexFollower : MonoBehaviour {

	int nearestVertex;

	// Use this for initialization
	void Start(){

	}
	void Awake () {
		
		nearestVertex= findNearestVertex ();
	}
	
	// Update is called once per frame
	void Update () {
		Mesh parentMesh=this.GetComponentInParent<MeshFilter> ().mesh;
		print (nearestVertex);
		print (parentMesh.vertices [nearestVertex]);
		transform.position = parentMesh.vertices [nearestVertex];
	}

	int findNearestVertex(){

		Mesh parentMesh=this.GetComponentInParent<MeshFilter> ().mesh;

		float smallestDistance = Mathf.Infinity;
		int foundVertex=0;


		for (int i=0;i<parentMesh.vertices.Length; i++ ){

			float distance = Vector3.Distance (transform.position, parentMesh.vertices[i]);
			if (distance<smallestDistance) {
				smallestDistance = distance;
				foundVertex = i;
			}
		}
		return foundVertex;
	}

}
