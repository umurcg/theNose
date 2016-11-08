using UnityEngine;
using System.Collections;

public class randomMovement : MonoBehaviour {
    public float speed;
    Vector3 destination;
    Quaternion targetRotation;
    // Use this for initialization
    bool changeDirection;
    Vector3 tempPosition;
    float timer;
	SphereCollider sc;

	void Awake () {

        changeDirection = true;
		sc = transform.parent.GetComponent<SphereCollider> ();
    }
	
	// Update is called once per frame
	void Update () {

        if (changeDirection)
        {
            timer = 0;
            tempPosition = transform.position;
			destination = Random.insideUnitSphere * sc.radius*2+ transform.parent.position;
            targetRotation = Quaternion.LookRotation(destination - transform.position);
            changeDirection = false;
        }
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(tempPosition, destination, timer * speed);
        if (Vector3.Distance(transform.position, destination) <=0.01) changeDirection = true;
        transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation, speed * Time.deltaTime*4);
      
	}
   
}
