using UnityEngine;
using System.Collections;

public class randomMovement : MonoBehaviour {
    public float speed;
    SphereCollider sc;
    Vector3 destination;
    Quaternion targetRotation;
    // Use this for initialization
    bool changeDirection;
    Vector3 tempPosition;
    float timer;

	void Awake () {
        sc = GetComponentInParent<SphereCollider>();
        changeDirection = true;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (changeDirection)
        {
            timer = 0;
            tempPosition = transform.position;
            destination = Random.insideUnitSphere * sc.radius + sc.gameObject.transform.position;
            targetRotation = Quaternion.LookRotation(destination - transform.position);
            changeDirection = false;
        }
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(tempPosition, destination, timer * speed);
        if (Vector3.Distance(transform.position, destination) <=0.01) changeDirection = true;
        transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation, speed * Time.deltaTime*4);
      
	}
   
}
