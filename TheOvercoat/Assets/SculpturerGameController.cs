using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class SculpturerGameController : GameController {

    public GameObject sculpturer;
    public float healthOfPlayer = 100;
    float health;
    List<HeykelController> heykels;
    public GameObject healthBarPrefab;
    Image healthBar;

    //public GameObject target;
    //public GameObject source;
    //public GameObject master;
    //float AA;
    //float BB;


	// Use this for initialization
	public override void Awake () {
        base.Awake();
        heykels = new List<HeykelController>();
       
	}

    public override void Start()
    {
        enableHeykels(false);
        base.Start();
        //startGame();
        //Timing.RunCoroutine(innerSpeech());

    }

    // Update is called once per frame
    void Update () {

        ////Debug.Log(Vector3.Distance(master.transform.position, target.transform.position)
        ////    + Vector3.Distance(master.transform.position, source.transform.position) + " " + (AA + BB / 2));

        //if (Vector3.Distance(master.transform.position, target.transform.position)
        //    + Vector3.Distance(master.transform.position, source.transform.position) >( AA + BB / 2))
        //{
        //    Debug.Log("Outside of capsule");
        //}
        //else
        //{
        //    Debug.Log("InsideOfCapsule");
        //}

        //Debug.DrawLine(target.transform.position, master.transform.position);
        //Debug.DrawLine(source.transform.position, master.transform.position);

    }



    //void drawElipse()
    //{
    //    //Dont shoot master
    //    //TODO Test it
    //    Vector3 planarTarget = target.transform.position;
    //    Vector3 planarPostion = source.transform.position;

    //    Vector3 planarMaster = master.transform.position;
    //    AA = Vector3.Distance(planarTarget, planarPostion);
    //    float B = 1;
    //    BB = B * 2;

    //    GameObject capsule = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Capsule));
    //    //capsule.transform.eulerAngles = new Vector3(90, 0, 0);
    
    //    CapsuleCollider cc= capsule.GetComponent<CapsuleCollider>();
    //    cc.direction = 2;
    //    cc.height = AA + BB;
    //    cc.radius = BB;

    //    //capsule.transform.localScale =Vector3.right* cc.height;

    //    capsule.transform.position = Vector3.Lerp(planarTarget, planarPostion, 0.5f);
  

    //}


    void startGame()
    {
        enableHeykels(true);
        Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        healthBar=  ((GameObject)Instantiate(healthBarPrefab,canvas) as GameObject).GetComponent<Image>();
        health = healthOfPlayer;
        healthBar.fillAmount = health/healthOfPlayer;
    }


    IEnumerator<float> innerSpeech()
    {
        //Look each other
        player.transform.LookAt(new Vector3(sculpturer.transform.position.x, player.transform.position.y, sculpturer.transform.position.z));
        sculpturer.transform.LookAt(new Vector3(player.transform.position.x, sculpturer.transform.position.y, player.transform.position.z));

        lockPlayer();

      
        sc.callSubtitleWithIndex(0);
        while (subtitle.text != "")
        {
            //if(GetComponent<SubtitleController>().getCurrentIndex()==4 && handlerHolder ==null)
            //    //Make pace sculpturer
            //    handlerHolder = Timing.RunCoroutine(Vckrs._pace(sculpturer, sculpturer.transform.position + sculpturer.transform.right * 4, sculpturer.transform.position - sculpturer.transform.right * 4));

            yield return 0;
        }

        //Timing.KillCoroutines(handlerHolder);

        unlockPlayer();

        yield break;
    }
    
    //Locks player while it is captured by sculpturer
    //TODO modelling and animations
    void lockPlayer()
    {
        pcc.StopToWalk();
    }

    void unlockPlayer()
    {
        pcc.ContinueToWalk();
    }


    public void registerHeykel(HeykelController heykel)
    {
        heykels.Add(heykel);
    }

    public void removeHeykel(HeykelController heykel)
    {
        heykels.Remove(heykel);
    }

    void enableHeykels(bool enable)
    {
        foreach (HeykelController heykel in heykels)
        {
            heykel.enabled = enable;
        }
    }

    public void damage(int amount)
    {
        health -= amount;
        healthBar.fillAmount = health/healthOfPlayer;
    }

}
