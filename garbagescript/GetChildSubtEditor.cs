using UnityEngine;
using System.Collections;
using CinemaDirector.Helpers;
using UnityEditor;

namespace CinemaDirector{
[CustomEditor (typeof(GetAllChildSubtitle))]
public class GetChildSubtEditor : Editor {

	// Use this for initialization
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

            GetAllChildSubtitle script = (GetAllChildSubtitle)target;

		if(GUILayout.Button("Get")){
			script.get ();
		}

	}
}
}