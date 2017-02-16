using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class MoveRandomlyOnNavMesh : MonoBehaviour {

    public float radius;
    public float speed=3f;
    NavMeshAgent nma;

    bool walking=false;

    // Use this for initialization
    void Awake () {

        nma = GetComponent<NavMeshAgent>();

        if (!nma)
        {
            Debug.Log("One of required components is empty");
            enabled = false;
        }

        nma.speed = speed;
       
    }
	
	// Update is called once per frame
	void Update () {
       
        if(!walking)
        {
            //Get position on circle
            Vector3 randomPos= Vckrs.generateRandomPositionOnCircle(transform.position, radius);

            //Cast it to position on navmesh
            Vector3 castedPos;
            if(Vckrs.findNearestPositionOnNavMesh(randomPos, nma.areaMask, radius, out castedPos)){
                Timing.RunCoroutine(_walkToPoint(castedPos));
            }

        }
	}




    IEnumerator<float> _walkToPoint(Vector3 pos)
    {
        walking = true;
        nma.Resume();
        nma.SetDestination(pos);

        while (Vector3.Distance(pos, transform.position) > 0.01f) yield return 0;
        walking = false;

        yield break;

    }



}
