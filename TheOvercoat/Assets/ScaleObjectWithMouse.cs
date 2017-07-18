using UnityEngine;
using System.Collections;

public class ScaleObjectWithMouse : MonoBehaviour {

    public float minScale = 1;
    public float maxScale = 20;

    public float scaleSpeed = 150;
    public float lerpSpeed = 5f;
    public string axisName=("Scale");

    public bool scaleLerp;

    Vector3 aimScale;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        aimScale += Vector3.one* Input.GetAxis(axisName)*scaleSpeed;

        float currentScale = aimScale.x;
        //transform.localScale = Vector3.one* Mathf.Clamp(currentScale, minScale, maxScale);
        aimScale= Vector3.one * Mathf.Clamp(currentScale, minScale, maxScale);

        if (scaleLerp)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, aimScale, Time.deltaTime * lerpSpeed);
        }
        else
        {
            transform.localScale = aimScale;
        }
    }
}
