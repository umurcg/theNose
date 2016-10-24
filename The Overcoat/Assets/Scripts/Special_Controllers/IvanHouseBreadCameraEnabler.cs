using UnityEngine;
using System.Collections;
namespace CinemaDirector
{
public class IvanHouseBreadCameraEnabler : MonoBehaviour {


	public Cutscene Narrator;

	public GameObject BreadCamera;




	CollectableObject co;
	StopAndStartAnimation sasa;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {



	}

	
	

	void activateBread(){
       
		BreadCamera.SetActive (true);
	

	}




}
}