﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GlobalController), true)]
public class GlobalControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GlobalController script = (GlobalController)target;
        if (GUILayout.Button("Fill debug scene list "))
        {
            script.setDebugListToIndex();

        }


        if (GUILayout.Button("Load next level "))
        {
            script.loadNextScene();

        }


        if (GUILayout.Button("Remove save file "))
        {
            script.removeSaveData();

        }
        //if (GUILayout.Button("Set full game scene list to default "))
        //{
        //    script.setDebugListToIndex();

        //}
    }
}