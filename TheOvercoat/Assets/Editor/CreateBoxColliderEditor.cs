using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(CreateBoxColliderForLODObjects), true)]
public class CreateBoxColliderEditor : Editor
{

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        CreateBoxColliderForLODObjects script = (CreateBoxColliderForLODObjects)target;

        if (GUILayout.Button("Create Colldier"))
        {
            script.createCollider();
        }
    }
}
