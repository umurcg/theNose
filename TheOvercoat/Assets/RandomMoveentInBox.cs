using UnityEngine;
using System.Collections;
using UnityEngine.AI;

//Random movement in box collider
public class RandomMoveentInBox : MonoBehaviour {


    public GameObject colliderObject;
    public float minSpeed=1;
    public float maxSpeed = 5;
    float speed;
    

    NavMeshAgent nma;

    float ratio = 0;
    Vector3 dest;
    Vector3 prevPos;
	// Use this for initialization
	void Start () {
        dest = Vckrs.generateRandomPositionInBox(colliderObject);
        prevPos = transform.position;
        speed = Random.Range(minSpeed, maxSpeed);
	
	}
	
	// Update is called once per frame
	void Update () {


        if (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(prevPos, dest, ratio);
        }else
        {
            ratio = 0;
            dest = Vckrs.generateRandomPositionInBox(colliderObject);
            prevPos = transform.position;
        }

	}
}
