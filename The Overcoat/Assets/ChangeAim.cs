using UnityEngine;
using System.Collections;

public class ChangeAim : MonoBehaviour {

    public Transform[] aims;
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
                GetComponent<LerpLookTo>().aimObject = aims[index];
                break;
            case (Scripts.LerpToPosition):
                GetComponent<LerpToPosition>().aim= aims[index];
                break;

            default:
                return;
        }

    }
}
