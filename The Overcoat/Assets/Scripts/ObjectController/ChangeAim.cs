using UnityEngine;
using System.Collections;


//This script changes aim of LerpLookTo or LerpLookToPosition script programitacly.

public class ChangeAim : MonoBehaviour {

    public Transform[] aims;
    //public Vector3[] aimVectors;
    public enum Scripts {LerpLookTo,LerpToPosition }
    public Scripts script;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void change(int index)
    {
        switch (script)
        {
            case (Scripts.LerpLookTo):
                LerpLookTo llt = GetComponent<LerpLookTo>();
                llt.aimObject = aims[index];
           //     llt.aim = aimVectors[index];

                break;
            case (Scripts.LerpToPosition):
                LerpToPosition ltp = GetComponent<LerpToPosition>();
                ltp.aim= aims[index];
                break;

            default:
                return;
        }

    }
}
