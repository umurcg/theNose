using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//Rotates Camera around player

public class CameraRotator : MonoBehaviour {


    GameObject player;
    CameraFollower cf;

    [HideInInspector]
    public bool rotating=false;

    // Use this for initialization
    void Start () {
        updatePlayer();
        cf = GetComponent<CameraFollower>();
	}
	
	// Update is called once per frame
	void Update () {


        
        if (Input.GetButtonDown("CameraRotator") && !rotating)
        {
            float angleDir = -Input.GetAxis("CameraRotator");
            float angle = angleDir * 90;
            if (player != null)
            {
                //Debug.Log(angle);
                Timing.RunCoroutine(_rotateCam(angle, 30));
                
            }     
           

        }
	}

    public void updatePlayer()
    {
        player = CharGameController.getActiveCharacter();
        

    }

    IEnumerator<float> _rotateCam(float angle,float speed)
    {

        //cf.enabled = false;
        Vector3 initialVector = transform.forward;

        Vector3 initialPosition = transform.position;
        Quaternion initialRotation = transform.rotation;

        //float initialHeight = player.transform.position.y;

        float totalAngleDif=0;

        rotating = true;
        while (totalAngleDif<Mathf.Abs(angle))
        {
           
            transform.RotateAround(player.transform.position, player.transform.up, Time.deltaTime * speed * Mathf.Sign(angle));
            totalAngleDif += Time.deltaTime * speed ;
            cf.updateRelative();

            //Preserve height while changing camera direction
            //player.transform.position = new Vector3(player.transform.position.x, initialHeight, player.transform.position.z);

            yield return 0;

            
        }


        //transform.position = initialPosition;
        //transform.rotation = initialRotation;
        //transform.RotateAround(player.transform.position, player.transform.up, angle);
        cf.updateRelative();




        //cf.enabled = true;

        rotating = false;

        yield break;
    }

}
