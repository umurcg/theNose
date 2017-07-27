using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class HitchockController : GameController, IClickAction  {

    public GameObject Camera,  MirrorPlane, /*KovMirrorPose,*/ GameController, CharController;
    GameObject MainCamera, Kovalev;
    public float fovSpeed;
    public float maxFov = 57f;

    MirrorReflection mr;

    KovalevHomeGameController khgc;

    HitchcockShot hs;

    public AudioClip soundEffect;

    // Use this for initialization
    public override void Start () {
        base.Start();
        hs = Camera.GetComponent<HitchcockShot>();
        mr = GetComponentInChildren<MirrorReflection>();

        khgc = GameController.GetComponent<KovalevHomeGameController>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public IEnumerator<float> _start()
    {
        mr.enabled = true;
        MainCamera = CharGameController.getCamera();
        Kovalev = CharGameController.getActiveCharacter();

        MainCamera.SetActive(false);
        Camera.SetActive(true);

        //Play sound effect
        LevelMusicController.playSoundEffect(soundEffect);

        PlayerComponentController pcc =Kovalev.GetComponent<PlayerComponentController>();
        pcc.StopToWalk();

        Kovalev.GetComponent<UnityEngine.AI.NavMeshAgent>().Stop();

        gameObject.transform.tag = "Untagged";

        yield return 0;


        //KovMirrorPose.SetActive(true);

        yield return Timing.WaitForSeconds(0.1f);


        yield return 0;

        Kovalev.transform.position = gameObject.transform.position - 1 * transform.up;
        Kovalev.transform.LookAt(gameObject.transform.position);

        //KovMirrorPose.SetActive(false);


        float minFov = hs.fov;
      
        
        while (hs.fov < maxFov)
        {

            //print((hs.fov));
            //print(Mathf.Exp((hs.fov - minFov) / (maxFov - minFov)));
            hs.fov += 1 * fovSpeed*Time.deltaTime*Mathf.Exp(2*(hs.fov-minFov)/(maxFov-minFov));

            //Debug.Log("hitckoooooooooooock");

            yield return 0;
        }

        if (sc == null) Debug.Log("sc is nışş");


        bool kovalevLoosesHisNOse = false;
        if (khgc.khs == KovalevHomeGameController.kovalevHomeScene.Dream)
        {
            sc.callSubtitleWithIndexTime(1);
            while (subtitle.text != "") yield return 0;

        }
        else if (khgc.khs == KovalevHomeGameController.kovalevHomeScene.KovalevLoosesHisNose)
        {
            sc.callSubtitleWithIndexTime(0);
            while (subtitle.text != "") yield return 0;

            kovalevLoosesHisNOse = true;




        }
        //yield return Timing.WaitForSeconds(5f);

        MainCamera.SetActive(true);
        Kovalev.SetActive(true);
        Camera.SetActive(false);

        pcc.ContinueToWalk();

        if (kovalevLoosesHisNOse)
        {
            CallCoroutine cc = GetComponent<CallCoroutine>();
            cc.call();
        }

        mr.enabled = false;

        LevelMusicController.clearSoundEffectFromSource();

        Destroy(this);
        
        
        yield break;
        
    }


     public override void Action(){
          Timing.RunCoroutine(_start());
      }

}
