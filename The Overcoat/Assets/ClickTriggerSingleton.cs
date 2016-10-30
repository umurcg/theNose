using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

//This script replacement for ClickTrigger script
//It makes same think with only one instance of class.
//It force to walk palyer to clicked object and when clicked object is colliding
//It calls IClickTrigger action method of it.


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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
            //    print(hit.transform.tag);
                if (tags.Contains(hit.transform.tag))
                {

                    StopCoroutine("setAim");
                    StartCoroutine(setAim(hit.transform.gameObject));   
                }

            }

        }
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

  
        walkToTarget(aim.transform.position);

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
