using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RandomObjectGenerator), true)]
public class ObjectFGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RandomObjectGenerator script = (RandomObjectGenerator)target;
        if (GUILayout.Button("Generate "))
        {
            script.generate();

        }

    }
}