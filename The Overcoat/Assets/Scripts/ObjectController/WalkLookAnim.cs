using UnityEngine;
using System.Collections;

// This script make subject object walk to owner object position, then rotates subject to forward vector of owner object and lastly it triggers or bools animation of it.
//Important! This uses nav mesh agent.
//i.e. Sitting animation.
//If subject is null, it assigns it as player

//To do 
//Match Target!!!!

public class WalkLookAnim : MonoBehaviour, IClickAction {
    public GameObject subject;
    public string animationName;
    public float tol = 0.1f;
    
    public enum AnimType {Trigger,Boolean};
    public AnimType animParameter;


    public float rotSpeed = 3f;

    bool sitting = false;
    Vector3 prevPos=Vector3.zero;

	// Use this for initialization
	void Start () {
        if (subject == null)
        {
            subject = CharGameController.getActiveCharacter();
        }
	}
	
	// Update is called once per frame
	void Update () {

        //call animation while getting up
        if (sitting)
        {
            if (prevPos == Vector3.zero)
            {
                prevPos = subject.transform.position;
            }else
            {
                //print(Vector3.Distance(prevPos, subject.transform.position));
                if (Vector3.Distance(prevPos, subject.transform.position) > 0.01f)
                {

                    Animator anim = subject.GetComponent<Animator>();
                    if (anim != null)
                        switch (animParameter)
                        {
                            case AnimType.Boolean:
                                anim.SetBool(animationName, false);
                                break;
                            case AnimType.Trigger:
                                anim.SetTrigger(animationName);
                                break;
                            default:
                                break;
                        }

                    sitting = false;
                }
                prevPos = subject.transform.position;
            }
        }
	}

    public IEnumerator start()
    {

        ////Disable collider
        //Collider col = GetComponent<Collider>();
        //col.enabled = false;
        
        NavMeshAgent nma = subject.GetComponent<NavMeshAgent>();
        if (nma != null)
        {
            nma.Resume();
            nma.destination = transform.position;
        } else
        {
            yield break;
        }

        
        while (Vector3.Distance(transform.position, subject.transform.position) > tol)
        {
            //print(Vector3.Distance(transform.position, subject.transform.position));

            yield return null;
        }

        nma.Stop();

        Vector3 localAim = transform.position + transform.forward;


        Quaternion initialRot = subject.transform.rotation;
        Quaternion aimRot = Quaternion.LookRotation(localAim - transform.position);
    
        float ratio = 0;

        while (ratio < 1)
        {

            // print(Vector3.Distance(transform.rotation.eulerAngles, aimRot.eulerAngles));
            ratio += Time.deltaTime * rotSpeed;
            subject.transform.rotation = Quaternion.Lerp(initialRot, aimRot, ratio);

            yield return null;
        }

        Animator anim = subject.GetComponent<Animator>();
        if(anim!=null)
        switch (animParameter) {
            case AnimType.Boolean:
               anim.SetBool(animationName, true);
                break;
            case AnimType.Trigger:
                anim.SetTrigger(animationName);
                break;
            default:
                break;
        }
        sitting = true;
        GetComponent<IWalkLookAnim>().finishedIWLA();

   //     anim.MatchTarget(transform.GetChild(0).position, transform.GetChild(0).rotation, AvatarTarget.Root,
                                                   //    new MatchTargetWeightMask(Vector3.one, 100f), 0.1f, 0.9f);
    

    }



    public void Action()
    {
     
        StartCoroutine(start());
    }
        


    
}
