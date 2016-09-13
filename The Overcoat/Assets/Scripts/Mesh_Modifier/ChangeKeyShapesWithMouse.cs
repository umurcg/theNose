using UnityEngine;
using System.Collections;

public class ChangeKeyShapesWithMouse : MonoBehaviour {

	SkinnedMeshRenderer smr;
	float blend=0;
	public float speed=200;
	// Use this for initialization
	void Start () {
		smr = GetComponent<SkinnedMeshRenderer> ();
		smr.SetBlendShapeWeight (0, blend);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {

		
			blend += Input.GetAxis("Mouse X")*Time.deltaTime*speed;
			blend -= Input.GetAxis("Mouse Y")*Time.deltaTime*speed;
			smr.SetBlendShapeWeight (0, blend);
		}	


	}
}
