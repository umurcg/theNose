using UnityEngine;
using System.Collections;

//This script makes samething with v1 but you have to assign a prefab that contains meshges with mesh request script
public class assignRandomMeshV2 : MonoBehaviour {

    public GameObject commonPeople;
    RequestCharacterMesh rcm;

    public RequestCharacterMesh.gender gender;
    
	// Use this for initialization
	void Awake   () {
        rcm = commonPeople.GetComponent<RequestCharacterMesh>();
        Mesh mesh = rcm.requestRandomMesh(gender);
        GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = mesh;
        transform.name = mesh.name;
	}
	

}
