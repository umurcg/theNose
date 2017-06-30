using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class drunkManController : GameController {

    public float timeBetweenDrinks = 7.5f;
    MoveRandomlyOnNavMesh moveRand;
    float timer = 0;
    Animator anim;

    float talkTimer;
    public float timeBetweenTalks = 7.5f;


	// Use this for initialization
	public override void Start () {
        base.Start();
        moveRand = GetComponent<MoveRandomlyOnNavMesh>();
        anim = GetComponent<Animator>();
        timer = timeBetweenDrinks;
        talkTimer = timeBetweenTalks;
    
        
	}
	
	// Update is called once per frame
	void Update () {




        if (talkTimer > 0)
        {
            talkTimer -= Time.deltaTime;

            if (talkTimer <= 0)
            {
                talkTimer = timeBetweenTalks;
                if(subtitle.text=="")
                    sc.callRandomSubtTime(0);
            }

        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = timeBetweenDrinks;
                anim.SetTrigger("Drink");
            }

        }

	}

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }



}
