using UnityEngine;
using System.Collections;

public class CharacterMouseLook : MonoBehaviour {
    public float speed=3f;
    NavMeshAgent nma;


	float timer=0;
	Vector3 forcedTarget;

   

	// Use this for initialization
	void Start () {
        nma = GetComponent<NavMeshAgent>();
	}




	public void LookTo(Vector3 t, float time){
		forcedTarget = t;
		timer = time;
	}


	// Update is called once per frame
	void Update () {


        if (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forcedTarget - transform.position), Time.deltaTime * speed);
            if (timer <= 0)
                timer = 0;

        }
        else
        {

            if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
             //   print(nma.velocity );
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && nma.velocity.magnitude<0.001 )
                if (Physics.Raycast(ray, out hit,Mathf.Infinity,~(1<<8)))
                {



                    Vector3 target = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), Time.deltaTime * speed);

                }
                      }

            }
        }
}
