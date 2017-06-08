using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script arrange light component intensity according to dayandnight cycle.
//You can use it in street lamps etc..

public class AutoLightScript : MonoBehaviour {


    public static List<GameObject> allLightsInScene;
    public float maxIntensity;



    Light l;
    Light globalLight;
    DayAndNightCycle danc;

    private void Awake()
    {
        if (allLightsInScene == null) allLightsInScene = new List<GameObject>();

        allLightsInScene.Add(gameObject);
    }

    private void OnDestroy()
    {
        allLightsInScene.Remove(gameObject);
    }


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


        lightIsChanging();

    }
	



    public void lightIsChanging()
    {
        if (!danc || !globalLight) return;

        l.intensity = (danc.maxIntensity - globalLight.intensity) / (danc.maxIntensity - danc.minIntensity) * maxIntensity;
    }

}
