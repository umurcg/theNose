using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script changes objects mesh quality.
//It is used for LOD system
public class ChangeQuality : MonoBehaviour {

    //static list is hold due to broadcast message that can be sent from lod script
    static List<ChangeQuality> scripts;

    public enum meshQuality {High=0, Middle=1, Low=0}

    //Meshes with different vertex counts. First one is most quality mesh and last is most 
    //optimized mesh.
    public Mesh[] meshes;

    MeshFilter meshFilter;

    private void OnEnable()
    {
        if (scripts == null) scripts = new List<ChangeQuality>();

        scripts.Add(this);
    }

    private void OnDisable()
    {
        scripts.Remove(this);
    }

    // Use this for initialization
    void Awake () {
        meshFilter = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeMeshQuality(meshQuality quality)
    {
        int index = (int)quality;

        if(meshes.Length>index && meshes[index] != null)
        {
            meshFilter.mesh = meshes[index];
        }

    }

    public static void broadCastChange(meshQuality quality)
    {
        foreach(ChangeQuality s in scripts)
        {
            s.changeMeshQuality(quality);
        }
    }

}
