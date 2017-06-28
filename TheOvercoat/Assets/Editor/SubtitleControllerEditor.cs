using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(SubtitleController),true)]
public class SubtitleControllerEditor : Editor {

    bool isNoticed = false;

    // Use this for initialization

    public SerializedProperty longStringProp;

    void OnEnable()
    {
        longStringProp = serializedObject.FindProperty("AllSubtitles");
    }

    public override void OnInspectorGUI(){
        serializedObject.Update();
        longStringProp.stringValue = EditorGUILayout.TextArea(longStringProp.stringValue, GUILayout.MaxHeight(75));
        serializedObject.ApplyModifiedProperties();


        DrawDefaultInspector ();

        

        SubtitleController script = (SubtitleController)target;

        if (!isNoticed && script.textAsset == null)
        {
            script.assignTextAsset();
            if (script.textAsset == null)
            {
                Debug.Log("Can't find text asset");
                isNoticed = true;
            }
        }

        if (GUILayout.Button("Export subtitles to Text Asset"))
        {
            script.exportToTextFile();
        }
        if (GUILayout.Button("Assign Text Asset"))
        {
            script.assignTextAsset();

        }

        if (GUILayout.Button("Set Subtitles")){
            script.AllToArray();

		}
        if (GUILayout.Button("Get Subtitles"))
        {
            script.ArrayToAll();
        }

        //if (GUILayout.Button("Set Character Subtitle"))
        //{
        //    script.setCharSubtitle();

        //}

        //if (GUILayout.Button("Set Narrator Subtitle"))
        //{
        //    script.setNarSubtitle();

        //}
    }
}
