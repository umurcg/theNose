using UnityEngine;
using System.Collections;

public class CharacterMouseLook : MonoBehaviour {
    public float speed=3f;
    NavMeshAgent nma;

    public enum Mode{withAgent,withoutAgent };
    public Mode mode = Mode.withAgent;

    public LayerMask mask;

	float timer=0;
	Vector3 forcedTarget;

    //For without agent mode
    Vector3 prevPos;

	// Use this for initialization
	void Start () {
        if(mode==Mode.withAgent)
        nma = GetComponent<NavMeshAgent>();

        if (mode == Mode.withoutAgent)
            prevPos = transform.position;

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
                float velocity = 0;
                if (mode == Mode.withAgent)
                {
                    velocity = nma.velocity.magnitude;
                }
                else
                {
                    velocity = Vector3.Distance(transform.position, prevPos);   
                }
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && (velocity < 0.001 ))

                    
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask))
                        {
                            //Debug.Log(hit.transform.name);
                            Vector3 target = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position), Time.deltaTime * speed);

                        }

                    
                }

            }
        prevPos = transform.position;
    }
}
