using UnityEngine;
using System.Collections;

public class LerpStandAlone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static IEnumerator lerp(Transform transform, Transform aim, float speed, float tolerance)
    {
        print("hi");
        Vector3 initialPosition = transform.position;
        //	print (initialPosition.y);
        float  ratio = 0;

        Vector3 aimPos = aim.transform.position + aim.transform.forward  + aim.transform.right ;

        while (Vector3.Distance(transform.position, aimPos) > tolerance)
        {
            ratio += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(initialPosition, aimPos, ratio);


          
            yield return null;

        }

    }

}
