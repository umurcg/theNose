using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using MovementEffects;

//This script replacement for ClickTrigger script
//It makes same think with only one instance of class.
//It force to walk palyer to clicked object and when clicked object is colliding
//It calls IClickTrigger action method of it.
//It must be assign to player object.
//Ignored layer is hardcoded with 8th layer. So becareful about that.

public class ClickTriggerSingleton : MonoBehaviour {

    NavMeshAgent agent;
    //This is for bird
    MoveToWithoutAgent mtwagent;

    //CharacterMouseLook cml;
    public string[] tags;

    List<GameObject> collidingObjects = new List<GameObject>();

    IEnumerator<float> walkToTargetHandler;
    public LayerMask ignoreMasks;
    PlayerComponentController pcc;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mtwagent = GetComponent<MoveToWithoutAgent>();
        pcc = GetComponent<PlayerComponentController>();
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
                //Debug.Log(hit.transform.name);
                if (tags.Contains(hit.transform.tag)) {

                    //Debug.Log(hit.transform.name);
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreMasks))
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


    
        IEnumerator<float> handler= Timing.RunCoroutine(walkToTarget(aim));

        //Here is walking
        while (checkIsColliding(aim) == false)
        {
            //print("walking");
            if (Input.anyKeyDown)
            {
                Timing.KillCoroutines(handler);
                stopToWalk();
                yield break;
            }
            yield return null;
        }


        Timing.KillCoroutines(handler);
        stopToWalk();

        Timing.RunCoroutine(Vckrs._lookTo(gameObject, aim, 1f));
        //Vckrs.testPosition(aim.transform.position);

        callAction(aim);
        yield break;
    }

    void stopToWalk()
    {
        if (agent)
        {
            agent.Stop();
        }else
        {
            mtwagent.stop();
        }
    }

    //Cancel all actions, coroutines
    public void abort()
    {
        if(walkToTargetHandler!=null)
        Timing.KillCoroutines(walkToTargetHandler);

        StopCoroutine("setAim");
        
        stopToWalk();
    }
   
    public void setDestination(Vector3 pos)
    {
        if (agent && agent.enabled)
        {
            //Vckrs.testPosition(pos);
            agent.SetDestination(pos);
        }else
        {
            mtwagent.setDestination(pos);
        }
    }

    IEnumerator<float> walkToTarget(GameObject aim)
    {

        //If player cant walk the you should abort the action
        if (pcc && !pcc.canPlayerWalk()) yield break;

        //Debug.Log("Walking");

        IClickActionDifferentPos icadp = aim.GetComponent<IClickActionDifferentPos>();

        if(agent && agent.enabled) agent.Resume();

        Vector3 position;
        while (true)
        {
               
            if (icadp != null)
            {
                //Debug.Log("different pos"+aim.gameObject.name);
                
                position=icadp.giveMePosition();


                //Vckrs.testPosition(position);
            }
            else
            {

               position=aim.transform.position;
            }
            //Debug.Log(position);

            //if(agent) agent.SetDestination(position);
            setDestination(position);


            if (aim.isStatic)
            {
                yield break;
            }
            yield return 0;
        }


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
        //print("callAction");
        IClickAction[] icas = go.GetComponents<IClickAction>();
        foreach(IClickAction ica in icas)
        {

            if (ica != null)
            {
                ica.Action();
                Debug.Log("Calling action");
            }
        }

        ISubtitleTrigger ist = go.GetComponent<ISubtitleTrigger>();
        if (ist != null)
        {
            if(ist.isAutomatic())     ist.callSubtitle();
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
