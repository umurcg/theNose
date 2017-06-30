using UnityEngine;
using System.Collections;

public class RandomizeUV : MonoBehaviour {

    Mesh mesh;

    public float minChange, maxChange;

    SkinnedMeshRenderer smr;

    Vector2[] originalUV;

	// Use this for initialization
	void Start () {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        mesh = smr.sharedMesh;
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
