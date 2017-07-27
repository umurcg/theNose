using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is just for opening and closing bazaar
public class BazaarController : MonoBehaviour {


    public GameObject[] objects;


	// Use this for initialization
	void Start () {

        if (CharGameController.getSun().GetComponent<DayAndNightCycle>().isNight)
        {
            closeBazaar();
        }

	}
	
    void closeBazaar()
    {
        foreach (var obj in objects) obj.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
