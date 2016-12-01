using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(SubtitleController))]
public class SubtitleControllerEditor : Editor {

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



		if(GUILayout.Button("Set Subtitles")){
            script.AllToArray();

		}
        if (GUILayout.Button("Get Subtitles"))
        {
            script.ArrayToAll();

        }

        if (GUILayout.Button("Set Character Subtitle"))
        {
            script.setCharSubtitle();

        }

        if (GUILayout.Button("Set Narrator Subtitle"))
        {
            script.setNarSubtitle();

        }
    }
}
