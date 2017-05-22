using UnityEngine;
using System.Collections;

//This script spawns a objec from one sid of box collider and destroys it from other side. Also it controls the movement of object

public class BoatSc : MonoBehaviour {

    public GameObject boat;
    public float speed = 0.1f;

    public enum axis { x,y,z};
    public axis Axis;

    GameObject spawnedObject;
    Vector3 aim;
    Vector3 startPos;
    // Use this for initialization


    float ratio;
	
	// Update is called once per frame
	void Update () {

        if (ratio < 1 && spawnedObject!=null)
        {
            ratio += Time.deltaTime*speed;
            spawnedObject.transform.position = Vector3.Lerp(startPos, aim, ratio);
        }else
        {
            ratio = 0;
            Destroy(spawnedObject);
            spawn();
        }


	}

    void spawn()
    {
        Vector3 unitVector=Vector3.zero;

        switch (Axis)
        {
            case axis.x:
                unitVector = Vector3.right;
                break;
            case axis.y:
                unitVector = Vector3.up;
                break;
            case axis.z:
                unitVector = Vector3.forward;
                break;
        }

        float randomSign = Vckrs.randomSign();
        startPos = transform.TransformPoint(randomSign*unitVector/2);
        aim = transform.TransformPoint(-randomSign * unitVector / 2);

        spawnedObject= Instantiate(boat);
        //spawnedObject.transform.parent = transform;
        spawnedObject.transform.position = startPos;

       
        
    }

}
