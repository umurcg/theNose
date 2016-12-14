using UnityEngine;
using System.Collections;

public class SplashWhenHitSomething : MonoBehaviour {
	public GameObject splash;
	ParticleSystem ps;
	public float destroyInSeconds=2f;
	float timer=0;
	// Use this for initialization
	void Start () {
		ps=	  splash.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0) {
			timer -= Time.deltaTime;
			print ("dest");
			if (timer <= 0) {

				timer = 0;
				Destroy (splash);
			}
		}
	}

    void OnTriggerEnter()
    {

    }

	void OnCollisionEnter(Collision col){
       
		col.transform.GetComponent<Collider> ().isTrigger = true;
		col.transform.gameObject.SetActive (false);
			
		splash.transform.position = col.contacts [0].point;

		ps.Play ();
	
	}

}
