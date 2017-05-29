using UnityEngine;
using System.Collections;


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
#if UNITY_EDITOR


        for (int i = 0; i < edges.Length; i++)
        {
            edges[i] = Mathf.Clamp(edges[i], 0, 100);
        }

        LODGroup[] lod = GetComponentsInChildren<LODGroup>();

        foreach (LODGroup l in lod)
        {

            //Debug.Log(lodGroup.);


            UnityEditor.SerializedObject obj = new UnityEditor.SerializedObject(l);

            UnityEditor.SerializedProperty valArrProp = obj.FindProperty("m_LODs.Array");
            for (int i = 0; valArrProp.arraySize > i; i++)
            {
                UnityEditor.SerializedProperty sHeight = obj.FindProperty("m_LODs.Array.data[" + i.ToString() + "].screenRelativeHeight");
                sHeight.doubleValue = 0;

            }
            obj.ApplyModifiedProperties();

      



        }

#endif
    }

    public void setValues()
    {


#if UNITY_EDITOR
        for (int i=0;i<edges.Length;i++)
        {
            edges[i] = Mathf.Clamp(edges[i], 0, 100);
        }

        LODGroup[] lod = GetComponentsInChildren<LODGroup>();

        foreach (LODGroup l in lod)
        {

            //Debug.Log(lodGroup.);

            UnityEditor.SerializedObject obj = new UnityEditor.SerializedObject(l);

            UnityEditor.SerializedProperty valArrProp = obj.FindProperty("m_LODs.Array");
            for (int i = 0; valArrProp.arraySize > i; i++)
            {
                UnityEditor.SerializedProperty sHeight = obj.FindProperty("m_LODs.Array.data[" + i.ToString() + "].screenRelativeHeight");
                sHeight.doubleValue = edges[i]/100;
          
            }
            obj.ApplyModifiedProperties();


        }
#endif
    }
}
