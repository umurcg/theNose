using UnityEngine;
using System.Collections;
using UnityEditor;
using CinemaDirector.Helpers;

namespace CinemaDirector
{
    [CustomEditor(typeof(DivideSubtitles))]
    public class CreateSubtitleEditor : Editor
    {

        // Use this for initialization
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DivideSubtitles script = (DivideSubtitles)target;

            if (GUILayout.Button("Divide"))
            {
                script.Divide();
            }




        }
    }
}