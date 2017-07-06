using UnityEngine;
using System.Collections;

//Preserves position even parent is on move
public class preservePosition : MonoBehaviour {

    public bool preserveX, preserveY, preserveZ;

    Vector3 initialPosition;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;

    }
	
	// Update is called once per frame
	void Update () {

        Vector3 posToAssing = transform.position;

        if (preserveX)
        {
            posToAssing.x = initialPosition.x;
        }

        if (preserveY)
        {
            posToAssing.y = initialPosition.y;
        }
        if (preserveZ)
        {
            posToAssing.z = initialPosition.z;
        }

        transform.position = posToAssing;

    }
}
