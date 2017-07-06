using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class MoveBotOnScreen : MonoBehaviour {

    public float timeBetweenMoves = 60f;
    Timer timer;
    NavMeshAgent nma;
    GameObject player;
    Camera cam;
	// Use this for initialization
	void Start () {
        timer = new Timer(timeBetweenMoves);
        nma = GetComponent<NavMeshAgent>();
        player = CharGameController.getActiveCharacter();
        cam = CharGameController.getCamera().GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {

        if (timer.ticTac(Time.deltaTime))
        {
            moveToScreen();
        }

	}


    public void moveToScreen()
    {
        Vckrs.setPositionToOutsideOfCameraAndOnNavmesh(gameObject, player.transform.position, 10, cam);
        //Vector3 castedPos=Vector3.zero;
        //if(Vckrs.findNearestPositionOnNavMesh())

    }
}
