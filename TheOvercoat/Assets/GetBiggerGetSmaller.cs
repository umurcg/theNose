using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBiggerGetSmaller : MonoBehaviour {

    public float speed=1f;
    public float minScale = 1;
    public float maxScale = 5;

    float scale;

    bool increasing = true;
	// Use this for initialization
	void Start () {

        scale = minScale;
        transform.localScale = minScale * Vector3.one;

	}
	
	// Update is called once per frame
	void Update () {

        if (increasing)
        {
            scale += Time.deltaTime * speed;
            if (scale > maxScale)
            {
                increasing = false;
                scale = maxScale;
            }

        }
        else
        {
            scale -= Time.deltaTime * speed;
            if (scale < minScale)
            {
                increasing = true;
                scale = minScale;
            }
        }

        transform.localScale = Vector3.one * scale;

	}
}
