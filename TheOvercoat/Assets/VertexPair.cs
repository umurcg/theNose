﻿using UnityEngine;
using System.Collections;


//This scripts enable user to match to vertex that are child of onwer object.
public class VertexPair : MonoBehaviour {

    LineRenderer lr;
    GameObject v1, v2;

    GameObject selectedVertex;

    CursorImageScript cis;

    bool edgeIsDrawed = false;

    public GameObject messageReciever;
    public string message;

    public Texture2D cursor;

	// Use this for initialization
	void Start () {
        lr=GetComponent<LineRenderer>();
        lr.SetVertexCount(0);
        v1 = transform.GetChild(0).gameObject;
        v2 = transform.GetChild(1).gameObject;
        cis = Camera.main.GetComponent<CursorImageScript>();
    }

    // Update is called once per frame
    void Update()
    {
        cis.externalTexture = null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!edgeIsDrawed && Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.transform.name + " "+ hit.transform.tag);
            if (hit.transform.gameObject == v1 || hit.transform.gameObject==v2)
            {
                cis.externalTexture = cursor;

                if (Input.GetMouseButtonDown(0))
                {
                    //Debug.Log("You hit a vertex");
                       if (selectedVertex!=null)
                    {
                        if (hit.transform.gameObject != selectedVertex)
                        {
                            //Set position to second vertex because player drawed the edge
                            lr.SetPosition(1, hit.transform.position);
                            edgeDrawed();
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log("You picked a vertex");
                            lr.SetVertexCount(2);
                            lr.SetPosition(0, hit.transform.position);
                            selectedVertex = hit.transform.gameObject;

                    }
                }
            } else
            {
               

                if (Input.GetMouseButtonDown(0))
                {
                    resetLine();
                }
            }

        }else if(!edgeIsDrawed)
        {
  

            if (Input.GetMouseButtonDown(0))
            {
                resetLine();
            }
        }

        if (selectedVertex!=null) lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Update lines in rotation
        if (!edgeIsDrawed && selectedVertex != null)
        {
            lr.SetPosition(0, selectedVertex.transform.position);
        }else if (edgeIsDrawed)
        {
            lr.SetPosition(0, selectedVertex.transform.position);
            GameObject otherVertex = (selectedVertex == v1) ? v2 : v1;
            lr.SetPosition(1, otherVertex.transform.position);
        }
        

    }

    void edgeDrawed()
    {
        if (edgeIsDrawed) return;
        
        edgeIsDrawed = true;

        Debug.Log("One edge is drawed");

        if (messageReciever == null) return;

        messageReciever.SendMessage(message);
    }

    void resetLine()
    {
        selectedVertex = null;
        lr.SetVertexCount(0);
        
    }
}
