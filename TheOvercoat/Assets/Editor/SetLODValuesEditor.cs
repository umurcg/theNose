using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SetLodValues), true)]
public class SetLODValuesEditor : Editor {



    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SetLodValues script = (SetLodValues)target;
        if (GUILayout.Button("Set Values "))
        {
            script.setValues();

        }


        if (GUILayout.Button("DisableLOD "))
        {
            script.disableLODS();

        }




    }
}
