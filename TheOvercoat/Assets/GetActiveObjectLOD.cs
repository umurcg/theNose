using UnityEngine;
using System.Collections;
using MovementEffects;

public class GetActiveObjectLOD : MonoBehaviour {

    LODGroup lod;

	// Use this for initialization
	void Start () {

            
	}
	
	// Update is called once per frame
	void Update () {

	}

    public GameObject getActiveObject()
    {
      
       foreach (Transform child in transform)
       {
             var renderer = child.GetComponent<Renderer>();
                if (renderer != null && renderer.isVisible)
                {
                return renderer.gameObject;
                }
            
        }
        return null;
    }


}
