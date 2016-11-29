﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

//This script replacement for ClickTrigger script
//It makes same think with only one instance of class.
//It force to walk palyer to clicked object and when clicked object is colliding
//It calls IClickTrigger action method of it.
//It must be assign to player object.
//Ignored layer is hardcoded with 8th layer. So becareful about that.

public class ClickTriggerSingleton : MonoBehaviour {

    NavMeshAgent agent;
    //CharacterMouseLook cml;
    public string[] tags;

    List<GameObject> collidingObjects = new List<GameObject>();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //cml = GetComponent<CharacterMouseLook>();
    }

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
     //   printObjectNames();

        if (Input.GetMouseButtonUp(0))
        {

            RaycastHit hit;

            object obj = raycastFirst();
            if (obj != null)
            {

                hit = (RaycastHit)obj;
                if (tags.Contains(hit.transform.tag)) { 
                StopCoroutine("setAim");
                StartCoroutine(setAim(hit.transform.gameObject));


            }
            }
          }

        
    }


    //Returns first raycasthit from camera to mousePosition.
    //If object having one of element of tags hitted it ignores it.
    //Fuck it
    //object raycastFirstExceptTag(string[] tags)
    //{
    //    RaycastHit[] hits;
    //    hits = Physics.RaycastAll(Camera.main.transform.position,Input.mousePosition );
    //    print("Number of hit is "+ hits.Length);
    //    Dictionary<RaycastHit, float> hitsAndDistance = new Dictionary<RaycastHit, float>();
    //    foreach (RaycastHit h in hits)
    //    {
    //        hitsAndDistance[h] = Vector3.Distance(h.transform.position, Camera.main.transform.position);
    //    }

    //    hitsAndDistance.OrderBy(x => x.Value);
        

    //    List<RaycastHit> hitsList = new List< RaycastHit > (hitsAndDistance.Keys);

    //    foreach (RaycastHit hit in hitsList)
    //    {
    //        print(hit.transform.name);
    //    }

    //        foreach (RaycastHit hit in hitsList)
    //    {
    //        if (!System.Array.Exists(tags, element => element == hit.transform.tag))
    //            return hit;
    //    }

    //    return null;
    //}


    object raycastFirst()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,~(1 << 8)))
        {
            return hit;
        }

        return null;
        
    }


    //debug
    void printObjectNames()
    {
        print(collidingObjects.Count);
        foreach (GameObject go in collidingObjects)
        {
            print(go.name);
        }
    }

    IEnumerator setAim(GameObject aim)
    {
        if (checkIsColliding(aim))
        {
          
            callAction(aim);
            yield break;
        }


        IClickActionDifferentPos icadp = aim.GetComponent<IClickActionDifferentPos>();
        if (icadp!=null)
        {
            walkToTarget(icadp.giveMePosition());
        }
        else
        {

            walkToTarget(aim.transform.position);
        }
        //Here is walking
        while (checkIsColliding(aim) == false)
        {
       
            if (Input.anyKeyDown)
            {
                stopToWalk();
                yield break;
            }
            yield return null;
        }

        stopToWalk();
        callAction(aim);
        yield break;
    }

    void stopToWalk()
    {
        agent.Stop();
    }

    void walkToTarget(Vector3 position)
    {
     
        agent.Resume();
        agent.SetDestination(position);

    }

    bool checkIsColliding(GameObject obj)
    {
        return collidingObjects.Contains(obj);

    }


    void OnTriggerEnter(Collider col)
    {
        if (tags.Contains(col.gameObject.tag))
        {
            if (!collidingObjects.Contains(col.gameObject))
                collidingObjects.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (collidingObjects.Contains(col.gameObject))
        {
            collidingObjects.Remove(col.gameObject);
        }
    }


    void callAction(GameObject go)
    {
        IClickAction[] icas = go.GetComponents<IClickAction>();
        foreach(IClickAction ica in icas)
        {
            ica.Action();
        }

        ISubtitleTrigger ist = go.GetComponent<ISubtitleTrigger>();
        if (ist != null)
        {
            ist.callSubtitle();
        }

    }

    public void removeMe(GameObject go)
    {
        if (collidingObjects.Contains(go))
        {
            collidingObjects.Remove(go);
        }
    }


}
