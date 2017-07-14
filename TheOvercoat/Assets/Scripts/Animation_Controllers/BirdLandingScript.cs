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
    BasicCharAnimations bca;

    bool isLanded=false;
    bool takingOff = false;
    bool landingOn = false;
    float timer;
    Vector3 lockPos;
	// Use this for initialization
	public void Start () {
      

        bc = GetComponent<BirdController>();
        mwa = GetComponent<MoveToWithoutAgent>();
        bca = GetComponent<BasicCharAnimations>();
        animContoller = GetComponent<Animator>();
        timer = delay * 2;
	}
	
	// Update is called once per frame
	void Update () {

        //If bird is in land, and taking off corouitene isn't working
        if (isLanded && !takingOff)
        {
            //Debug.Log(Input.GetAxis("Vertical"));
            if (Input.GetAxis("Vertical") != 0)
            {
                Timing.RunCoroutine(_takingOff());
            }
        }


        //Autolanding
        //If bird is not landed or not landing on right now
        if (!isLanded && !landingOn)
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

    
    //It makes bird take off
    public IEnumerator<float> _takingOff()
    {
        takingOff = true;

        Debug.Log("Taking Off");
        animContoller.SetBool(landingAnimationName, false);
        //Wait for animation
        yield return Timing.WaitForSeconds(2f);

        //Disable basic char animation for taking off
        bca.enabled = false;

        IEnumerator<float> handler = Timing.RunCoroutine(mwa._lookAndGo(transform.position+transform.up*2));
        yield return Timing.WaitUntilDone(handler);

        bc.pauseMovement = false;
        bc.pauseLimits = false;
        mwa.enabled = true;
        isLanded = false;

        //Enable basic char animation after finsiihng taking off
        bca.enabled = true;

        takingOff = false;
        yield break;
    }

    //TODO It can not detect object between character and aim
    public IEnumerator<float> _landOnTo(Vector3 position)
    {
        landingOn = true;

        //Disable basic char animation for landing
        bca.enabled = false;

        bc.pauseMovement=true;
        bc.pauseLimits = true;
        mwa.enabled = false;
        IEnumerator<float> handler = Timing.RunCoroutine(mwa._lookAndGo(position));
        yield return Timing.WaitUntilDone(handler);
        animContoller.SetBool(landingAnimationName, true);
        isLanded = true;

        landingOn = false;

        //Enable basic char animation after finsiihng landing
        bca.enabled = true;

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

    public void setAsLanded(bool isLanded)
    {

        animContoller.SetBool(landingAnimationName, isLanded);
        animContoller.SetBool(landingAnimationName + "Baked", false);
        this.isLanded = isLanded;
        bc.pauseMovement = isLanded;
        bc.pauseLimits = isLanded;
        mwa.enabled = !isLanded;
    }


    public void setAsLandedBaked(bool isLanded)
    {
        if(!animContoller) animContoller = GetComponent<Animator>();
        if(!bc) bc = GetComponent<BirdController>();
        if(!mwa) mwa = GetComponent<MoveToWithoutAgent>();

        animContoller.SetBool(landingAnimationName, isLanded);
        animContoller.SetBool(landingAnimationName+"Baked", true);
        this.isLanded = isLanded;
        bc.pauseMovement = isLanded;
        bc.pauseLimits = isLanded;
        mwa.enabled = !isLanded;
    }

    public bool isBirdOnLand()
    {
        return isLanded;
    }

}
