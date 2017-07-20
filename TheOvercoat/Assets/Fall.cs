using UnityEngine;
using System.Collections;

public class Fall : MonoBehaviour {

    public float fallSpeed;
    public Space space = Space.World;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) - ((space==Space.World) ? Vector3.up : transform.up) * Time.deltaTime * fallSpeed;
    }
}
