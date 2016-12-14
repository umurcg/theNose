using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;

public class HitchockController : MonoBehaviour, IClickAction  {

    public GameObject MainCamera, Camera, Kovalev, MirrorPlane, KovMirrorPose, GameController, CharController;
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
        MainCamera.SetActive(false);
        Kovalev.SetActive(false);
        Camera.SetActive(true);

        KovMirrorPose.SetActive(true);
      
        rp.RenderProbe();
        yield return Timing.WaitForSeconds(0.1f);

        KovMirrorPose.SetActive(false);


        float minFov = hs.fov;
      
        
        while (hs.fov < maxFov)
        {
            //print((hs.fov));
            //print(Mathf.Exp((hs.fov - minFov) / (maxFov - minFov)));
            hs.fov += 1 * fovSpeed*Mathf.Exp(2*(hs.fov-minFov)/(maxFov-minFov));
            

            yield return 0;
        }


        Text cs = CharController.GetComponent<Text>();
        cs.text = "Kovalev: Aman Tanrım!";

        yield return Timing.WaitForSeconds(5f);


        MainCamera.SetActive(true);
        Kovalev.SetActive(true);
        Camera.SetActive(false);


        CallCoroutine cc = GetComponent<CallCoroutine>();
        cc.call();

        
    }


     public void Action(){
          Timing.RunCoroutine(_start());
      }

}
