using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class MoveBotOnScreen : MonoBehaviour/*, IVisibility*/
{

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
        nma.enabled = false;
        Debug.Log("Movinfg");
        Vckrs.setPositionToOutsideOfCameraAndOnNavmesh(gameObject, player.transform.position, 100, cam,30,40);
        //Vector3 castedPos=Vector3.zero;
        //if(Vckrs.findNearestPositionOnNavMesh())
        nma.enabled = true;
    }


    public void onVisible()
    {
        enabled = false;
    }

    public void onInvisible()
    {
        enabled = true;
    }
}
