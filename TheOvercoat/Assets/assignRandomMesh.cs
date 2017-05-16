using UnityEngine;
using System.Collections;


//Assings mesh that is chosen from meshes array randomly

public class assignRandomMesh : MonoBehaviour {

    //Using game object instead of mesh directly due to easiness of assigning them
    public Mesh[] meshes;

	// Use this for initialization
	void Awake () {

        if(meshes.Length==0)
        {           
            return;
        }

        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr)
        {
            smr.sharedMesh = meshes[Random.Range(0, meshes.Length)];
        }
        else
        {
            Debug.Log("Couldn't found skinnedmesh render");
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
