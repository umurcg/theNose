using UnityEngine;
using System.Collections;

public class girtyBeFriendsScript : MonoBehaviour {

    characterComponents girty;
    GameObject player;

	// Use this for initialization
	void Start () {
        girty = new characterComponents(gameObject);
        player = CharGameController.getActiveCharacter();
        transform.parent = CharGameController.getOwner().transform;
        GetComponent<DontLetPlayerToApproach>().enabled = false;
        GetComponent<RandomWalkAndAnimate>().enabled = false;

        girty.animator.SetBool("Bark", false);
        girty.animator.SetBool("Smell", false);
    }
	
	// Update is called once per frame
	void Update () {

        if(Vector3.Distance(player.transform.position,transform.position)>3)
        girty.navmashagent.SetDestination(player.transform.position-player.transform.forward*2);

	}
}
