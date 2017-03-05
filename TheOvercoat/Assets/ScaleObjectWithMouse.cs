using UnityEngine;
using System.Collections;

public class ScaleObjectWithMouse : MonoBehaviour {

    public float minScale = 1;
    public float maxScale = 20;

    public float scaleSpeed = 150;
    public string axisName=("Scale");


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale += Vector3.one* Input.GetAxis(axisName)*scaleSpeed;

        float currentScale = transform.localScale.x;
        transform.localScale = Vector3.one* Mathf.Clamp(currentScale, minScale, maxScale);
    }
}
