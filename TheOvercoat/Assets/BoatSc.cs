using UnityEngine;
using System.Collections;

//This script spawns a objec from one sid of box collider and destroys it from other side. Also it controls the movement of object
//You can use gameobjects for ibndicating start and end positions

public class BoatSc : MonoBehaviour {

    public GameObject boat;
    public float speed = 0.1f;

    public enum axis { x,y,z};
    public axis Axis;

    public GameObject position1, position2;

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


        if (position1 != null && position2 != null)
        {
            int randomInt = Random.Range(0, 2);
            if (randomInt == 0)
            {
                aim = position1.transform.position;
                startPos = position2.transform.position;
            }
            else
            {

                aim = position2.transform.position;
                startPos = position1.transform.position;
            }


        }
        else
        {

            Vector3 unitVector = Vector3.zero;

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
            startPos = transform.TransformPoint(randomSign * unitVector / 2);
            aim = transform.TransformPoint(-randomSign * unitVector / 2);

        }

        spawnedObject= Instantiate(boat);
        //spawnedObject.transform.parent = transform;
        spawnedObject.transform.position = startPos;
        spawnedObject.transform.rotation = Quaternion.LookRotation(aim - startPos);
        
       
        
    }

}
