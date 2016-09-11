using UnityEngine;
using System.Collections;

public class CharacterLook : MonoBehaviour {
    public float speed=3f;
    NavMeshAgent nma;

	// Use this for initialization
	void Start () {
        nma = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetAxis("Horizontal")==0&&Input.GetAxis("Vertical") == 0&&nma.velocity==Vector3.zero)
        if (Physics.Raycast(ray,out hit)) {
            Vector3 target=new Vector3(hit.point.x,transform.position.y,hit.point.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target - transform.position),Time.deltaTime*speed);

    }
}
}
