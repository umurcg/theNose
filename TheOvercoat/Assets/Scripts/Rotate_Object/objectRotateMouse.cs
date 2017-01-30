using UnityEngine;
using System.Collections;

public class objectRotateMouse : MonoBehaviour {

    // Use this for initialization
    public float rotationSpeed =5;
    public Camera cam;
    public Quaternion currentOrientation;
    public float factor = 20;
    Quaternion originalAngle;

    public float previous;

    void Awake () {
        originalAngle = transform.rotation;       
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 mp = Input.mousePosition;


        float diff = Mathf.Clamp( (mp.x - Screen.width/2),-45,45);


        //print(Quaternion.Angle(transform.rotation, originalAngle)+" "+diff);



        //if (Mathf.Abs(Quaternion.Angle(transform.rotation, originalAngle)) < Mathf.Abs( diff))
        //{

        //float x=-Input.GetAxis("Mouse Y");



        //transform.RotateAround(transform.position, transform.right, x * Time.deltaTime * rotationSpeed);

        if(Mathf.Abs(diff-previous)>0.01)
        transform.rotation = originalAngle * Quaternion.Euler(0, factor*diff*Time.deltaTime, 0);
        previous = diff;



    }


}
