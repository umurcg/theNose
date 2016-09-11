using UnityEngine;
using System.Collections;

public class StopAndStartAnimation : MonoBehaviour {
	public float getUpTime;
	Animator objAnim;
	PlayerComponentController pcc;
	float timer;
    public string animBool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (timer > 0) {
			timer -= Time.deltaTime;
			if (timer <= 0)
				getUp ();

		} 

	}

	void OnTriggerEnter(Collider collider){
		if(collider.tag.Equals("Player")){
			objAnim = collider.GetComponent<Animator> ();
			objAnim.SetTrigger(animBool)	;
			pcc = collider.GetComponent<PlayerComponentController> ();
			Invoke("StopToWalk",0.5f);
			if (getUpTime > 0) {
				timer = getUpTime;


			}
		}

	}

	void StopToWalk(){
		pcc.StopToWalk ();

	}
	void ContinueToWalk(){
		pcc.ContinueToWalk();
		Destroy(this.gameObject);
	}



	public void getUp(){
		if (objAnim != null) {
	

			objAnim.SetTrigger(animBool);
			Invoke("ContinueToWalk",0.5f);


		}
	}

}
