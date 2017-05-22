using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(AssignMaterialToChildren),true)]
public class AssignMaterialEditor : Editor
{

    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();

        AssignMaterialToChildren script = (AssignMaterialToChildren)target;

        if (GUILayout.Button("Assign to all children"))
        {
            script.assignToAllChildren();
        }
    }
}
