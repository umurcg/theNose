using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovementInCircle : MonoBehaviour {

    public float radius = 10;
    NavMeshAgent nma;
    Vector3 center;
    Vector3 prevPos;
    public float tol = 0.2f;

	// Use this for initialization
	void Start () {

        center = transform.position;
        prevPos = transform.position;
        nma = GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //Debug.Log(Vector3.Distance(prevPos, transform.position));
        if (Vector3.Distance(prevPos, transform.position) < tol)
        {
            nma.SetDestination(getPos());
        }

        prevPos = transform.position;

	}

    Vector3 getPos()
    {

        Vector3 pos;
        pos=Vckrs.getRandomPosInCircle(center, radius,Plane.XZ);
        Vckrs.findNearestPositionOnNavMesh(pos, nma.areaMask, 30f, out pos);
        return pos;
    }

}


#if UNITY_EDITOR

[UnityEditor.CustomEditor(typeof(RandomMovementInCircle))]
public class RandomMovementInCircleEditor : UnityEditor.Editor
{

    private RandomMovementInCircle script;

    public void OnSceneGUI()
    {
        script = this.target as RandomMovementInCircle;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(script.transform.position + (script.transform.forward) // position
                                      , script.transform.up                       // normal
                                      , script.radius);                              // radius


    }
}

#endif