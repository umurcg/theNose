using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script starts landing animation of bird
//If user wait near of landing place it triggers landing after a delay
//Also it has method for direct landing function to specific place
public class BirdLandingScript : MonoBehaviour {

    public string landingAnimationName;
    [Tooltip("Distance for detecting landign position")]
    public float distance;
    [Tooltip("Delay befor triggering landing")]
    public float delay=5f;

    //public GameObject debugObject;

    Animator animContoller;
    MoveToWithoutAgent mwa;
    BirdController bc;

    bool isLanded=false;
    float timer;
    Vector3 lockPos;
	// Use this for initialization
	void Start () {
      

        bc = GetComponent<BirdController>();
        mwa = GetComponent<MoveToWithoutAgent>();
        animContoller = GetComponent<Animator>();
        timer = delay * 2;
	}
	
	// Update is called once per frame
	void Update () {

        if (isLanded)
        {
            //Debug.Log(Input.GetAxis("Vertical"));
            if (Input.GetAxis("Vertical") != 0)
            {
                Timing.RunCoroutine(_landOff());
            }
        }


        //Autolanding
        if (!isLanded)
        {
            //Delay*2 means timer is locked. I know it is stupid.
            if (timer == delay*2)
            {
                Ray ray = new Ray(transform.position, -Vector3.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    //Debug.Log("Hit!");
                    timer = delay;
                    lockPos = transform.position;
                }
            }else if (timer <= 0)
            {
                //Debug.Log("hi");
                Timing.RunCoroutine(_landOnTo(getBelowPosition(transform.position)));
                timer = delay * 2;

            }else
            {
                //Debug.Log(timer);
                timer -= Time.deltaTime;
                if (Vector3.Distance(transform.position, lockPos) > 0.05f)
                {
                    //Player moved so cancel landing
                    timer = delay*2;
                }
            }

        }
        

	}

    

    public IEnumerator<float> _landOff()
    {
        //Debug.Log("LandOff");
        animContoller.SetBool(landingAnimationName, false);
        //Wait for animation
        yield return Timing.WaitForSeconds(2f);

        IEnumerator<float> handler = Timing.RunCoroutine(mwa._lookAndGo(transform.position+transform.up*2));
        yield return Timing.WaitUntilDone(handler);
        bc.pauseMovement = false;
        bc.pauseLimits = false;
        mwa.enabled = true;
        isLanded = false;
        yield break;
    }

    //TODO It can not detect object between character and aim
    public IEnumerator<float> _landOnTo(Vector3 position)
    {

        bc.pauseMovement=true;
        bc.pauseLimits = true;
        mwa.enabled = false;
        IEnumerator<float> handler = Timing.RunCoroutine(mwa._lookAndGo(position));
        yield return Timing.WaitUntilDone(handler);
        animContoller.SetBool(landingAnimationName, true);
        isLanded = true;
        yield break;
    }


    //This function raycast to below of owner and returns hit position
    //If fails return zero vector
    Vector3 getBelowPosition(Vector3 position)
    {
       
        Ray ray = new Ray(position,  - Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.point.ToString());
            return hit.point;
        }

        return Vector3.zero;
    }



}
