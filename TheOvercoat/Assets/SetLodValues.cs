using UnityEngine;
using System.Collections;
using UnityEditor;

public class SetLodValues : MonoBehaviour {


    public float[] edges;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void disableLODS()
    {
        for (int i = 0; i < edges.Length; i++)
        {
            edges[i] = Mathf.Clamp(edges[i], 0, 100);
        }

        LODGroup[] lod = GetComponentsInChildren<LODGroup>();

        foreach (LODGroup l in lod)
        {

            //Debug.Log(lodGroup.);

            SerializedObject obj = new SerializedObject(l);

            SerializedProperty valArrProp = obj.FindProperty("m_LODs.Array");
            for (int i = 0; valArrProp.arraySize > i; i++)
            {
                SerializedProperty sHeight = obj.FindProperty("m_LODs.Array.data[" + i.ToString() + "].screenRelativeHeight");
                sHeight.doubleValue = 0;

            }
            obj.ApplyModifiedProperties();


        }
    }

    public void setValues()
    {
        for(int i=0;i<edges.Length;i++)
        {
            edges[i] = Mathf.Clamp(edges[i], 0, 100);
        }

        LODGroup[] lod = GetComponentsInChildren<LODGroup>();

        foreach (LODGroup l in lod)
        {

            //Debug.Log(lodGroup.);

            SerializedObject obj = new SerializedObject(l);

            SerializedProperty valArrProp = obj.FindProperty("m_LODs.Array");
            for (int i = 0; valArrProp.arraySize > i; i++)
            {
                SerializedProperty sHeight = obj.FindProperty("m_LODs.Array.data[" + i.ToString() + "].screenRelativeHeight");
                sHeight.doubleValue = edges[i]/100;
          
            }
            obj.ApplyModifiedProperties();


        }
    }
}
