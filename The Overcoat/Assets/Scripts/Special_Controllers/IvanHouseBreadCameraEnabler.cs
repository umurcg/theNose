using UnityEngine;
using System.Collections;
namespace CinemaDirector
{
public class IvanHouseBreadCameraEnabler : MonoBehaviour {
	public bool sit=false;
	public bool bread = false;

	public Cutscene Narrator;
	
	public GameObject chair;
	public GameObject BreadCamera;
	public GameObject player;
	public GameObject breadPH;



	CollectableObject co;
	StopAndStartAnimation sasa;
	// Use this for initialization
	void Start () {
		co = GetComponent<CollectableObject> ();
		sasa = chair.GetComponent<StopAndStartAnimation> ();

	}
	
	// Update is called once per frame
	void Update () {

		float breadDist = Vector3.Distance (chair.transform.position, player.transform.position + sasa.offset_MoveToHere);
		float sitDist=Vector3.Distance(breadPH.transform.position+co.unCollectPositionOffset , transform.position);


		if (breadDist<=1)
			bread = true;

		if (sitDist<=1)
			sit = true;

	
		    

		if (sit && bread) {
				
			player.GetComponent<Animator> ().SetTrigger ("SitTriggerHands");

				chair.GetComponent<StopAndStartAnimation> ().enabled = false;
				if(Narrator.State!=Cutscene.CutsceneState.Playing)
					Narrator.Play ();
				
				//Narrator.Invoke ("activateBread", 37.2f);
				this.enabled = false;
		}


	}

	
	
	void activateBread(){
       
		BreadCamera.SetActive (true);
	
	}




}
}