using UnityEngine;
using System.Collections;

public class SetTimeScale : MonoBehaviour {
    public float scale;
    float prevScale;
	// Use this for initialization
	void Start () {
        scale = Mathf.Clamp(scale, 0, 10);
        Time.timeScale = scale;
        prevScale = scale;
    }
	
	// Update is called once per frame
	void Update () {
        if (prevScale != scale)
        {
            scale = Mathf.Clamp(scale, 0, 10);
            Time.timeScale = scale;
            prevScale = scale;

        }
    }
}
