using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//Rotates Camera around player

public class CameraRotator : MonoBehaviour {


    public float speed = 4f;
    public int rotationPerRound = 8;
    float rotateAngle;

    GameObject player;
    CameraFollower cf;

    [HideInInspector]
    public bool rotating=false;
    
    


    // Use this for initialization
    void Start () {
        updatePlayer();
        cf = GetComponent<CameraFollower>();
        rotateAngle = 360 / rotationPerRound;

        
	}
	
	// Update is called once per frame
	void Update () {



        
        if (Input.GetButtonDown("CameraRotator") && !rotating)
        {
            float angleDir = -Input.GetAxis("CameraRotator");
            float angle = angleDir * rotateAngle;
            if (player != null)
            {
                //Debug.Log(angle);
                Timing.RunCoroutine(_rotateCam(angle, speed));
                
            }     
           

        }
	}

    public void updatePlayer()
    {
        player = CharGameController.getActiveCharacter();
        

    }

    //TODO while rotating if character walks, he gets out from focus of camera. To prevent this I disabled player movement while rotating.
    //Fix this bug and enable player walk while camera rotates
    IEnumerator<float> _rotateCam(float angle,float speed)
    {
        player = CharGameController.getActiveCharacter();

        PlayerComponentController pcc = player.GetComponent<PlayerComponentController>();

        //If player cant walk then exit from rotation
        if (!pcc.canPlayerWalk()) yield break;

        //Factor speed
        speed *= 10;

        //cf.enabled = false;

        Vector3 initialVector = transform.forward;

        Vector3 cameraInitialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        //float initialHeight = player.transform.position.y;

        float totalAngleDif=0;

        //float y = 20;

        rotating = true;

        //Disabe player movement
        pcc.StopToWalk();
        pcc.pauseNma();

        while (totalAngleDif<Mathf.Abs(angle))
        {
           
            transform.RotateAround(player.transform.position, player.transform.up, Time.deltaTime * speed * Mathf.Sign(angle));
            //cf.follow();

            //transform.position = new Vector3(transform.position.x, initialPosition.y+ Mathf.Sin(Mathf.PI*totalAngleDif/Mathf.Abs(angle)*y)  , transform.position.z);
            totalAngleDif += Time.deltaTime * speed ;

            //cf.enabled = false;
            //cf.follow();
            cf.updateRelative();
            //cf.enabled = true;
            

            //Preserve height while changing camera direction
            //player.transform.position = new Vector3(player.transform.position.x, initialHeight, player.transform.position.z);

            yield return 0;
                        
        }

        //Enable player movement
        pcc.ContinueToWalk();

        //cf.enabled = true;

        //transform.position = initialPosition;
        //transform.rotation = initialRotation;
        //transform.RotateAround(player.transform.position, player.transform.up, angle);
        cf.updateRelative();




        //cf.enabled = true;

        rotating = false;

        yield break;
    }

}
