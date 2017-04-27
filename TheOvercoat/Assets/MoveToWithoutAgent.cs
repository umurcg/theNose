﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MoveToWithoutAgent : MonoBehaviour {

    public float speed = 3;
    IEnumerator<float> handler;
    public GameObject floor;

	// Use this for initialization
	void Start () {
        if (floor == null)
        {
            Debug.Log("No floor object");
            enabled = false;
            
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Event e = Event.current;
  
        if (Input.anyKey)
        {

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                stop();
          
            }
            else if (Input.GetMouseButtonDown(0))
            {
                //print("mouse");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))

                {
                    //print(hit.transform.name);
                    if (hit.transform.gameObject==floor)
                    {
                        Vector3 aim = hit.point;
                        aim = new Vector3(aim.x, transform.position.y, aim.z);

                        Timing.RunCoroutine(_lookAndGo(aim));
                        
                        
                        
                    }
                }

            }



        }
    }

    public void stop()
    {
        if (handler != null)
            Timing.KillCoroutines(handler);
    }

    public void setDestination(Vector3 pos)
    {
        Timing.RunCoroutine(_lookAndGo(pos));
    }

    public IEnumerator<float> _lookAndGo(Vector3 aim)
    {
        //Debug.Log("Look and go");
        stop();

        float dist = Vector3.Distance(aim, transform.position);
        float time = speed / dist;

        if (handler != null) 
        Timing.KillCoroutines(handler);

        IEnumerator<float> localHandler = Timing.RunCoroutine(Vckrs._lookTo(gameObject, aim-transform.position, 2f));
        yield return Timing.WaitUntilDone(localHandler);
        handler = Timing.RunCoroutine(Vckrs._Tween(gameObject, aim, time));
        yield return Timing.WaitUntilDone(handler);

        //Debug.Log("Finished look and go");

        yield break;
    }

}
