﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MovementEffects;
//Raycast to for each aim objects from owner object
//If found object is specified layer it makes it transparent
//When mouse is over on those object also make them transparent

public class FadeOutBetweenObjects : MonoBehaviour {

    public GameObject[] targets;
    List<GameObject> targetsList;
    List<GameObject> fadedObjects;

    // Use this for initialization
    void Start () {

        targetsList = targets.ToList();
        fadedObjects = new List<GameObject>();
        GameObject player = CharGameController.getActiveCharacter();
        if (player != null && !targetsList.Contains(player))
        {
            targetsList.Add(player);
        }
        //foreach (GameObject obj in targetsList)
        //    Debug.Log(obj.transform.name);
	}
	
	// Update is called once per frame
	void Update () {

        

        foreach (GameObject obj in targetsList) {
            //Debug.Log("Raycasting");

            List<GameObject> allHittedObjects=new List<GameObject>();

            Ray ray = new Ray(transform.position,obj.transform.position-transform.position);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach  (RaycastHit hit in hits)
            {
                GameObject hittedObject = hit.transform.gameObject;
                allHittedObjects.Add(hittedObject);
                //Debug.Log(hit.transform.name);
                if ( hittedObject!=obj && hittedObject.layer==8)
                {        
                    makeObjectTransparent(hittedObject);  
                }
                             

            }

            foreach (GameObject fadedObj in fadedObjects)
            {
                
                if (!allHittedObjects.Contains(fadedObj))
                {
                    //Debug.Log("Restoring " + fadedObj.name);
                    Timing.RunCoroutine(restoreMaterial(fadedObj));
                }
            }

        }
	}


    void makeObjectTransparent(GameObject obj)
    {
        if (fadedObjects.Contains(obj)) return;



        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) return;

        Debug.Log("Fading " + obj.name);

        Material originalMat = rend.material;
        StandardShaderUtils.ChangeRenderMode(rend.material,StandardShaderUtils.BlendMode.Transparent);
        Timing.RunCoroutine(Vckrs._fadeObject(obj, 1f));
        fadedObjects.Add(obj);
    }

    IEnumerator<float> restoreMaterial(GameObject obj)  //TODO while restorematerial function running prevent call of makeObjectTransparent function for this object.
    {
        if (!fadedObjects.Contains(obj)) yield break;


        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) yield break;

        fadedObjects.Remove(obj);

        Debug.Log("Fading in " + obj.name);

        IEnumerator<float> handler= Timing.RunCoroutine(Vckrs._fadeObject(obj, 1f));
        yield return Timing.WaitUntilDone(handler);
        

        StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);

    }

    //TODO mouse over fade

}