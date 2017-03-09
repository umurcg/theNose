using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class HitchockController : MonoBehaviour, IClickAction  {

    public GameObject Camera,  MirrorPlane, /*KovMirrorPose,*/ GameController, CharController;
    GameObject MainCamera, Kovalev;
    public float fovSpeed;
    public float maxFov = 57f;
   

    ReflectionProbe rp;
    HitchcockShot hs;
    // Use this for initialization
    void Start () {
        rp = MirrorPlane.GetComponent<ReflectionProbe>();
        hs = Camera.GetComponent<HitchcockShot>();


    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public IEnumerator<float> _start()
    {
        MainCamera = CharGameController.getCamera();
        Kovalev = CharGameController.getActiveCharacter();

        MainCamera.SetActive(false);
        Camera.SetActive(true);

        PlayerComponentController pcc =Kovalev.GetComponent<PlayerComponentController>();
        pcc.StopToWalk();

        Kovalev.GetComponent<NavMeshAgent>().Stop();

        yield return 0;

        rp.RenderProbe();

        yield return 0;

        Kovalev.transform.position = gameObject.transform.position - 3 * transform.up;
        Kovalev.transform.LookAt(gameObject.transform.position);

        //KovMirrorPose.SetActive(true);

        yield return 0;

        Kovalev.transform.position = gameObject.transform.position - 3 * transform.up;
        Kovalev.transform.LookAt(gameObject.transform.position);

        rp.RenderProbe();
        yield return 0;

        Kovalev.transform.position = gameObject.transform.position - 3 * transform.up;
        Kovalev.transform.LookAt(gameObject.transform.position);

        //KovMirrorPose.SetActive(false);


        float minFov = hs.fov;
      
        
        while (hs.fov < maxFov)
        {

            //print((hs.fov));
            //print(Mathf.Exp((hs.fov - minFov) / (maxFov - minFov)));
            hs.fov += 1 * fovSpeed*Mathf.Exp(2*(hs.fov-minFov)/(maxFov-minFov));

            //Debug.Log("hitckoooooooooooock");

            yield return 0;
        }


        Text cs = CharController.GetComponent<Text>();
        cs.text = "Kovalev: Aman Tanrım!";

        yield return Timing.WaitForSeconds(5f);


        MainCamera.SetActive(true);
        Kovalev.SetActive(true);
        Camera.SetActive(false);

        pcc.ContinueToWalk();

        CallCoroutine cc = GetComponent<CallCoroutine>();
        cc.call();

        Destroy(this);
        gameObject.transform.tag = "Untagged";
        yield break;
        
    }


     public void Action(){
          Timing.RunCoroutine(_start());
      }

}
