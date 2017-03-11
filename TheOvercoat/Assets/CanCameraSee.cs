using UnityEngine;
using System.Collections;

public class CanCameraSee : MonoBehaviour {

    public GameObject aim;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 screenPoint = CharGameController.getCamera().GetComponent<Camera>().WorldToViewportPoint(aim.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        Debug.Log("onscreen " + onScreen.ToString());
    }
}
