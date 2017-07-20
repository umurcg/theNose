using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pace1D : MonoBehaviour {

    public GameObject aim1,aim2;

    public Vector3 direction;
    GameObject currentAim;

    public float tolerance = 0.5f;
    public float speed = 1;

    int delay = 10;

	// Use this for initialization
	void Start () {
        currentAim = aim1;
        updateDirection();
        

	}
	
	// Update is called once per frame
	void Update () {

        if(delay>0)
        {
            delay--;
            return;
        }

        if (Vector2.Distance(transform.position, currentAim.transform.position) < tolerance)
        {
            currentAim=(currentAim==aim1) ? aim2: aim1;

            updateDirection();

            
        }
        
        transform.position  += (speed * Time.deltaTime) *direction ;
		
	}


    

    void updateDirection()
    {
        if (currentAim == aim1)
        {
            direction= aim1.transform.position - aim2.transform.position;
        }
        else
        {
            direction= -aim1.transform.position  + aim2.transform.position;
        }
    }

}
