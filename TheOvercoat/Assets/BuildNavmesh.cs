using UnityEngine;
using System.Collections;


public class BuildNavmesh : MonoBehaviour {

    public GameObject[] roads;

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


        UnityEditor.NavMeshBuilder.BuildNavMesh();

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
