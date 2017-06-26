﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script set light to day or night
//Owner should have light component
public class DayAndNightCycle : MonoBehaviour {

    
    public bool debugNight, debugDay;
    public float maxIntensity = 1;
    public float minIntensity = 0;
    public float speed = 1;

    [HideInInspector]
    public bool isNight = false;

	// Use this for initialization
	void Start () {
        Light l = GetComponent<Light>();
        if (l == null)
        {
            Debug.Log("No Light component");
            enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (debugDay)
        {
            debugDay = false;
            makeDay();

        }
        else if (debugNight)
        {
            debugNight = false;
            makeNight();
        }
    }

    public void makeNight()
    {
        Light l = GetComponent<Light>();
        l.intensity = maxIntensity;
        isNight = true;
        Timing.RunCoroutine(_changeLight(speed,l,minIntensity,maxIntensity));

    }



    public void makeDay()
    {
        Light l = GetComponent<Light>();
        l.intensity = minIntensity;
        isNight = false;
        Timing.RunCoroutine(_changeLight(speed, l,minIntensity,maxIntensity));
        
    }

    public static IEnumerator<float> _changeLight(float speed, Light light, float minIntensity, float maxIntensity)
    {

        float intensity = light.intensity;

        if (intensity == minIntensity)
        {
            while (intensity < maxIntensity)
            {
                intensity += Time.deltaTime * speed;
                light.intensity = intensity;

                broadCastLightChange();

                yield return 0;
            }

            broadCastLightChange();

            intensity =maxIntensity;
            light.intensity = intensity;


         

            yield break;
        }
        else
        {
            while (intensity > minIntensity)
            {
                intensity -= Time.deltaTime * speed;
                light.intensity = intensity;

                broadCastLightChange();

                yield return 0;
            }
            intensity = minIntensity;
            light.intensity = intensity;

            broadCastLightChange();

            yield break;
        }
    }


    static void broadCastLightChange()
    {

        foreach (GameObject l in AutoLightScript.allLightsInScene)
        {
            l.GetComponent<AutoLightScript>().lightIsChanging();
        }
    }

}
