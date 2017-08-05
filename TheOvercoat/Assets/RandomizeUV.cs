using UnityEngine;
using System.Collections;

public class RandomizeUV : MonoBehaviour {

    //public enum renderType {SkinendMesh,Mesh };
    //public renderType RenderType = renderType.SkinendMesh;

    Mesh mesh;

    public float minChange = -1000;
    public float maxChange = 1000;
    
    
    Vector2[] originalUV;

	// Use this for initialization
	void Start () {

 
        SkinnedMeshRenderer smr;

        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr) mesh = smr.sharedMesh;

        if (!mesh)
        {
            MeshFilter mf;
            mf = GetComponentInChildren<MeshFilter>();
            if (mf) mesh = mf.mesh;

        }

        if (!mesh)
        {
            enabled = false;
            return;
        }

      
        originalUV = mesh.uv;
        randomize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    private void OnDestroy()
    {
        mesh.uv = originalUV;   
    }

    public void randomize()
    {
        Vector2[] uvs=mesh.uv;

        float xChange = Random.Range(minChange, maxChange);
        float yChange = Random.Range(minChange, maxChange);

        for (int i=0; i<uvs.Length;i++)
        {
            uvs[i] += uvs[i] + new Vector2(xChange,yChange);
            
        }

        mesh.uv = uvs;

        //smr.sharedMesh = mesh;
        

    }
}
