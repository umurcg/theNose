using UnityEngine;
using System.Collections;
using MovementEffects;

public class Trailer : MonoBehaviour {

    public GameObject bigBook;
    Camera cam;

	// Use this for initialization
	void Start () {

        CharGameController.getCamera().SetActive(false);

        cam = GetComponentInChildren<Camera>();

        bigBook.GetComponent<BookAC>().openBook();
        cam.orthographicSize = 1000;
        Timing.RunCoroutine(Vckrs._cameraSizeRootFunc(cam, 10, 55, 1f/*0.7f*/));

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
