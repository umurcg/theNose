using UnityEngine;
using System.Collections;
namespace CinemaDirector{
public class NoseClickAndDrag: ClickAndDrag {

	ChangeKeyShapesWithMouse ckswm;
	Transform bread;
	SkinnedMeshRenderer smr;
	public GameObject chair;


	
		public Cutscene NoseThrow;

	int dragged;
	StopAndStartAnimation sasa;

	


	new void Start(){
	
		sasa = chair.GetComponent<StopAndStartAnimation> ();
		base.Start ();
		for(int i=0;i<transform.parent.childCount;i++) {


			if (transform.parent.GetChild (i) != transform) {
				bread = transform.parent.GetChild (i);
				ckswm = bread.GetComponent<ChangeKeyShapesWithMouse> ();
				smr = bread.GetComponent<SkinnedMeshRenderer> ();
			}
		
		

		}
	}

	public override void dragObject() {

	
		if (smr.GetBlendShapeWeight(0) < 70f)
			return;
			
		
		ckswm.enabled = false;
				if (Input.GetAxis ("Mouse X") !=0|| Input.GetAxis ("Mouse Y")!=0) {
					transform.position += (transform.up/3-transform.forward)* Time.deltaTime*speed;
			dragged++;
		}
		ckswm.enabled = true;

		if (dragged > 40) {
			dropTheNose ();


		}

	}

   
	Vector3 getMedianOfVertices(){
		Vector3[] vertices = GetComponent<MeshFilter> ().mesh.vertices;
		Vector3 sum=Vector3.zero;
		foreach (Vector3 vertex in vertices)
			sum += vertex;
		return sum / vertices.Length;


	}


	void dropTheNose(){
			transform.parent.gameObject.active = false;
			NoseThrow.Play ();
	}

}

}