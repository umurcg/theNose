using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

//This script make subject object walk to owner object position, then rotates subject to forward vector of owner object and lastly it triggers or bools animation of it.
//Important! This doesn't use nav mesh agent
//i.e. Sitting animation.
//If subject is null, it assigns it as player
//Add child object for specifying exact position

//To do 
//Match Target!!!!

public class WalkLookAnim : MonoBehaviour, IClickAction {
    public GameObject subject;
    public string animationName;
    public float speed = 1f;
    public float rotSpeed = 3f;
    public Vector3 lookDirection = Vector3.forward;
    public bool sitAtStart = false;
    
    //Active script is script that is called sit but not getup yet
    public static WalkLookAnim activeScript=null;

    //Locks sitting
    public bool lockSit;

    public enum AnimType { Trigger, Boolean };
    public AnimType animParameter;

    bool sitting = false;
    Vector3 prevPos=Vector3.zero;
    GameObject childObject;

    Collider col;
    IEnumerator<float> handler;
    Animator anim;



    //public bool getup;
    // Use this for initialization
    void Start () {
        if (subject == null)
        {
            subject = CharGameController.getActiveCharacter();
            if (subject == null)
            {
                enabled = false;
                return;
            }
        }

        col = GetComponent<Collider>();
        anim = subject.GetComponent<Animator>();

        if (sitAtStart)
        {
            Timing.RunCoroutine(_sit(true));
        }

    }

    // Update is called once per frame
    void Update () {



        if(subject.transform.tag=="Player"){
            if (sitting && !lockSit)
            {
                //If trying to move
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) Timing.RunCoroutine(_getUp());


                if (Input.GetMouseButton(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))

                    {
                        if (hit.transform.tag == "Floor")
                        {
                            //TODO If is a dialouge open we should prevent getting up if subtitleController' lockPlayer boolean is true
                            //We can not understand directly wther dialouges locksPlayer or not.
                            ////For now just check subtitile
                            //Text subt = SubtitleFade.subtitles["CharacterSubtitle"];
                            //if (subt != null)
                            //{
                            //    if (subt.text != "")
                            //        return;

                            //}


                            Timing.RunCoroutine(_getUp());
                        }
                    }


                }

            }
        }




    }


    public void start()
    {
        Timing.RunCoroutine(_sit());
    }

    public IEnumerator<float> _sit(bool baked=false)
    {
      
        //Validate is not sitting
        if (sitting)  yield break;

        //Disable collider
        if(col!=null)
        col.enabled = false;

        //Set player components
        disablePlayer(true);

        //Looking for child
        if (transform.childCount > 0)
        {
            childObject = transform.GetChild(0).gameObject;
        }

        Vector3 aim = (childObject != null) ? childObject.transform.position : transform.position;

        //Prevent y change while walking.
        //float y = aim.y;
        aim = new Vector3(aim.x, aim.y, aim.z);

        if (!baked)
        {
            //Tween to position
            handler = Timing.RunCoroutine(Vckrs._Tween(subject, aim, speed));
            yield return Timing.WaitUntilDone(handler);

            //Rotate to forward
            handler = Timing.RunCoroutine(Vckrs._lookTo(subject, lookDirection, rotSpeed));


            ////Set y while sitting
            //Vector3 aimWithY = new Vector3(subject.transform.position.x, y, subject.transform.position.z);
            //IEnumerator<float> handler2 = Timing.RunCoroutine(Vckrs._Tween(subject, aimWithY, speed));
            //yield return Timing.WaitUntilDone(handler2);

            yield return Timing.WaitUntilDone(handler);
        }
        else
        {
            //Set position and rotation directly
            subject.transform.position = aim;
            subject.transform.LookAt(subject.transform.position+ lookDirection);

        }



        //Animation according to enum value
        if (anim != null)
        {
            switch (animParameter)
            {
                case AnimType.Boolean:

                    yield return 0;
                    anim.SetBool(animationName, true);
                                     

                    break;
                case AnimType.Trigger:
                    anim.SetTrigger(animationName);
                    break;
                default:
                    break;
            }
        }else
        {
            Debug.Log("No animator");
        }

        
        //set sittin boolean
        sitting = true;

        //Call interface method
        IWalkLookAnim iwla = GetComponent<IWalkLookAnim>();
        if (iwla!=null)  iwla.finishedIWLA();

        activeScript = this;
    }


    public void getUp()
    {
        Timing.RunCoroutine(_getUp());
    }

    public IEnumerator<float> _getUp()
    {
        if (!sitting) yield break;


        //Animation according to enum value
        if (anim != null)
        {
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
        }
        else
        {
            Debug.Log("No animator");
        }

        //handler = Timing.RunCoroutine(Vckrs._Tween(subject, subject.transform.position+subject.transform.forward , speed));
        //yield return Timing.WaitUntilDone(handler);

        yield return Timing.WaitForSeconds(1f); //Todo get this from animator

        disablePlayer(false);

        if(col!=null)
        col.enabled = true;
        sitting = false;


        activeScript = null;
    }

    public void disablePlayer(bool disable)
    {
        //Check for pcc
        PlayerComponentController pcc = subject.GetComponent<PlayerComponentController>();
        NavMeshAgent nma = subject.GetComponent<NavMeshAgent>();
        
        //if (!pcc||!nma) return;

        if (disable)
        {
            if(pcc)  pcc.StopToWalk();
            if(nma)  nma.enabled = false;
        }
        else
        {
            if(pcc) pcc.ContinueToWalk();
            if(nma) nma.enabled = true;
        }

    }


    public void Action()
    {
     
        start();
    }
        
    public bool isSitting()
    {
        return sitting;
    }

    public void lockPlayer()
    {
        lockSit = true;
    }




}
