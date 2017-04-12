using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.SceneManagement;

public class SculpturerCatchesKovalev : GameController {

    public float camRotateSpeed = 1f;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

    }


	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Timing.RunCoroutine(fallCameraEffect());
        }
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

    IEnumerator<float> fallCameraEffect()
    {
        pcc.StopToWalk();

        GameObject cam = CharGameController.getCamera();
        CameraFollower cf = cam.GetComponent<CameraFollower>();
        cf.enabled = false;

        Debug.Log("Rotatinngggg");

        float angle = 0;
        while (angle < 90) {

            float rotateAmount = Time.deltaTime*camRotateSpeed;
            cam.transform.Rotate(cam.transform.position+cam.transform.forward, -rotateAmount);
            angle += rotateAmount;
            yield return 0;
            
         }

        handlerHolder = blackScreen.script.fadeOut();
        yield return Timing.WaitUntilDone(handlerHolder);
        
        cf.enabled = true;

        pcc.ContinueToWalk();
        

        SceneManager.LoadScene((int)(GlobalController.Scenes.Atolye));

        yield break;
    }

    public IEnumerator<float> callFirstSubtitle()
    {
        yield return 0;
        sc.callSubtitleWithIndex(0);
    }

}
