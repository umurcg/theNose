using UnityEngine;
using System.Collections;

//Rotates camera around player with lerp.
public class CameraRotatorSmooth : MonoBehaviour {

    public float rotationSpeed=1f;
    GameObject player;
    CameraFollower cf;

	// Use this for initialization
	void Start () {
        player = CharGameController.getActiveCharacter();
        cf = cf.GetComponent<CameraFollower>();
	}
	
	// Update is called once per frame
	void Update () {

        var wantedRotation =  rotationSpeed * Input.GetAxis("CameraRotator");
        transform.RotateAround(player.transform.position, player.transform.up, Time.deltaTime * rotationSpeed * wantedRotation);
        

    }

    //void RotateObject(Vector3 point,  Vector3 axis,
    //                  float rotateAmount)
    //{
    //    var step = 0.0f;//non-smoothed
    //    var rate = 1.0f; //amount to increase non-smooth step by
    //    var smoothStep =0f; //smooth step this time
    //    var lastStep =0f; //smooth step last time
    //    while (step < 1.0)
    //    { // until we're done
    //        step += Time.time * rate; //increase the step
    //        smoothStep = Mathf.SmoothStep(0.0f, 1.0f, step); //get the smooth step
    //        transform.RotateAround(point, axis,
    //                               rotateAmount * (smoothStep - lastStep));
    //        lastStep = smoothStep; //store the smooth step
        
    //    }

    //    //finish any left-over
    //    if (step > 1.0) transform.RotateAround(point, axis,
    //                                           rotateAmount * (1.0f - lastStep));
    //}

    public void updatePlayer()
    {
        player = CharGameController.getActiveCharacter();
        


    }
}
