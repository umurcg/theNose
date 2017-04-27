using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelScreenShotScript), true)]
public class ScreenShotEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelScreenShotScript script = (LevelScreenShotScript)target;



        if (GUILayout.Button("ScreenShot"))
        {
            script.takeScreenShot();

        }
        //if (GUILayout.Button("Set full game scene list to default "))
        //{
        //    script.setDebugListToIndex();

        //}
    }
}
