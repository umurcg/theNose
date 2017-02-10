using UnityEngine;
using System.Collections;


//_ChangeKeyShapesWithMouse
//_Dependent to: SkinnedMeshRenderer

//This script enables player to change blend key of object with mouse position.

public class ChangeKeyShapesWithMouse : MonoBehaviour {

	SkinnedMeshRenderer smr;
	float blend=0;
	public float speed=200;
    public bool useRaycast;
	// Use this for initialization
	void Start () {
		smr = GetComponent<SkinnedMeshRenderer> ();
		smr.SetBlendShapeWeight (0, blend);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if (useRaycast)
            {
                RaycastHit hit;

                Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject == transform.gameObject)
                        changeBlendKey();
                }
            }else
            {
                changeBlendKey();
            }

        }


	}

	void changeBlendKey(){
		if (blend < 100) {
			blend += Input.GetAxis ("Mouse X") * Time.deltaTime * speed;
		} else {
			blend = 100;
		}

		if (blend > 0) {
			blend -= Input.GetAxis ("Mouse Y") * Time.deltaTime * speed;
		} else {
			blend = 0;
		}
		smr.SetBlendShapeWeight (0, blend);
	}
}
