using UnityEngine;
using System.Collections;
using UnityEditor;
using CinemaDirector.Helpers;

namespace CinemaDirector{
[CustomEditor (typeof(createAndAddSubtitles))]
public class CreateSubtitleEditor : Editor {

	// Use this for initialization
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		createAndAddSubtitles script = (createAndAddSubtitles)target;

		if(GUILayout.Button("Create and Add Subtitles")){
			script.createAndAdd();
		}

	


	}
	}}