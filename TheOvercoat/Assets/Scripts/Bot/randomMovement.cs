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

    bool optimized = true;

	void Awake () {

        changeDirection = true;
		sc = transform.parent.GetComponent<SphereCollider> ();

        if (optimized) enabled = false;

    }
	
	// Update is called once per frame
	void Update() {

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


    //These part is for optimization

    private void OnBecameVisible()
    {

        if (!optimized) return;

//#if UNITY_EDITOR
//        if (Camera.current && Camera.current.name == "SceneCamera")
//            return;
//        #endif

                //Debug.Log("Became visile");

                enabled = true;

    }

    private void OnBecameInvisible()
    {
        if (!optimized) return;


        //#if UNITY_EDITOR
        //if (Camera.current && Camera.current.name == "SceneCamera")
        //            return;
        //#endif

                //Debug.Log("Became invisible");

                enabled = false;

    }


}
