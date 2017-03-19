using UnityEngine;
using System.Collections;

//This script arrange light component intensity according to dayandnight cycle.
//You can use it in street lamps etc..

public class AutoLightScript : MonoBehaviour {

    public float maxIntensity;

    Light l;
    Light globalLight;
    DayAndNightCycle danc;

	// Use this for initialization
	void Start () {
        l = GetComponent<Light>();
        GameObject sun = CharGameController.getSun();
        if (sun == null)
        {
            //Debug.Log("No sun");
            enabled = false;
            l.intensity = 0;
            return;

        }

        globalLight = sun.GetComponent<Light>();
        danc = sun.GetComponent<DayAndNightCycle>();

    }
	
	// Update is called once per frame
	void Update () {
        if (!danc || !globalLight) return;
        
        l.intensity = (danc.maxIntensity - globalLight.intensity)/(danc.maxIntensity-danc.minIntensity)*maxIntensity;
        //Debug.Log(l.intensity);
	}
}
