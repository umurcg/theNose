using UnityEngine;
using System.Collections;
using UnityEditor;

namespace CinemaDirector{
[CustomEditor (typeof(SetSubtitleTimes))]
public class SetSubtitleTimesEditor : Editor {

	// Use this for initialization
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		SetSubtitleTimes script = (SetSubtitleTimes)target;

		if(GUILayout.Button("Set Times")){
			script.setTimes ();
		}
		if(GUILayout.Button("Restore")){
				script.restore ();
		}
			if(GUILayout.Button("SetDuration")){
				script.setDuration ();
			}
	}
}
}