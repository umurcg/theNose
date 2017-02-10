using UnityEngine;
using System.Collections;

public class GirtController : MonoBehaviour {

    DontLetPlayerToApproach dltpta;
	// Use this for initialization
	void Start () {
        dltpta = GetComponent<DontLetPlayerToApproach>();
        GameObject player = CharGameController.getActiveCharacter();

        if (player == null)
        {
            dltpta.enabled = false;  
        }else if (player.transform.name != "Kovalev")
        {
            dltpta.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
