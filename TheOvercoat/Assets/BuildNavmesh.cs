using UnityEngine;
using System.Collections;

//With this script you can build navmesh without enabling and disablig object that is created for specifyn areas.
//Gameobjects and meshcolliders will be enabled and disabled.
public class BuildNavmesh : MonoBehaviour {

    public GameObject[] roads;
    public MeshCollider[] meshColliders;

	// Use this for initialization
	void Start () {
	
	}
	
    public void build()
    {
#if UNITY_EDITOR

        foreach (GameObject obj in roads)
        {
            if (obj == gameObject) continue;
            obj.SetActive(true);
        }  

        foreach(MeshCollider mc in meshColliders)
        {
            mc.enabled = true;
        }


        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

        foreach (MeshCollider mc in meshColliders)
        {
            mc.enabled = false;
        }


        foreach (GameObject obj in roads)
        {
            if (obj == gameObject) continue;
            obj.SetActive(false);
        }
#endif
    }

    // Update is called once per frame
    void Update () {
	
	}
}

#if UNITY_EDITOR

[UnityEditor.CustomEditor(typeof(BuildNavmesh), true)]
public class BuildNavmeshEditor : UnityEditor.Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BuildNavmesh script = (BuildNavmesh)target;
        if (GUILayout.Button("Build "))
        {
            script.build();

        }


    }

}
#endif
