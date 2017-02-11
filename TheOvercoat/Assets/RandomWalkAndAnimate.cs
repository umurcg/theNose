using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This script randomly move owner andanimate between movements.

public class RandomWalkAndAnimate : GameController {

    public float minAnimationDuration, maxAnimationDuration;
    public GameObject sphere;
    public string[] animationNames;

    characterComponents dogCC;
    Vector3 prevPosition;
    SphereCollider col;
    float timer;

	// Use this for initialization
	public override void Start () {
        base.Start();
        dogCC = new characterComponents(gameObject);

        //Check sphere is not null
        if (sphere == null) enabled = false;

        //Check at least one animation is added
        if (animationNames.Length == 0) enabled = false;

        col = sphere.GetComponent<SphereCollider>();

        
	}
	
	// Update is called once per frame
	void Update () {

        //print(timer);

        //Decrease timer if it is set
        if (timer > 0) { timer -= Time.deltaTime; } else if (timer < 0) { timer = 0; }

        //If previous position is not set, set it to current position
        if (prevPosition == Vector3.zero) prevPosition = transform.position;

        //If timer is not set, trigger new position and animation
        if (timer == 0)
        {
            Vector3 target = findRandomPos(col.radius*sphere.transform.localScale.x);
            dogCC.navmashagent.SetDestination(target);
            string animationName = animationNames[Random.Range(0, animationNames.Length)];

            //Starts timer 
            timer = Random.Range(minAnimationDuration, maxAnimationDuration);

            Timing.RunCoroutine(_animateForSeconds(animationName));

        }

        
        	
	}

    //Animates dog for seconds
    //Animation must be boolean
    //You should set timer before call this function from outside
    public IEnumerator<float> _animateForSeconds(string name)
    {
        //Wait for object finish its transform. While waiting for it timer is not ticking.
        handlerHolder = Timing.RunCoroutine(Vckrs.waitUntilStop(gameObject, 0));
        yield return Timing.WaitUntilDone(handlerHolder);


        //Set animation
        dogCC.animator.SetBool(name, true);

        //print("Set animation");

        //Wait timer to be finished by update function.
        //Finish 1 second early to let the object finishes its animation
        while (timer > 0) yield return 0;

        //Disable animation
        dogCC.animator.SetBool(name, false);

        //Kill it self
        yield break;
      
    }

    //Finds a random position until a position that exceeds distance between nre pos and current pos exceeds minDistance is found
    public Vector3 findRandomPos(float  minDistance)
    {

        //Checks minDistance is below radius of collider
        if (minDistance >= col.radius* sphere.transform.localScale.x * 2) return Vector3.zero;

        Vector3 target=Vector3.zero;

        while (target == Vector3.zero || Vector3.Distance(transform.position, target) < minDistance)
        {
            float radius = col.radius*sphere.transform.localScale.x;
            Vector3 origin = sphere.transform.position + col.center;
            target= (Random.insideUnitSphere * radius) + origin;

        }


        //print(Vector3.Distance(transform.position, target) +" minDistanvce "+minDistance);
        
        return target;

    }


    public override void activateController()
    {
        base.activateController();
        gameObject.SetActive(true);
    }
    public override void deactivateController()
    {
        base.deactivateController();
        gameObject.SetActive(false); 

    }

}
