using UnityEngine;
using System.Collections;

public class IvanHouseBreadCameraEnabler : MonoBehaviour {
	public bool sit=false;
	public bool bread = false;


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

		if (sit && bread)
			Invoke ("activateBread", 2);



	}


	void activateBread(){
		BreadCamera.SetActive (true);
		this.enabled = false;
	}

}
