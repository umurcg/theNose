using UnityEngine;
using System.Collections;


public class changeTranparency : MonoBehaviour {
    public float t=1;
    Renderer r;
    Color textureColor;
    // Use this for initialization
    void Start () {
        r = GetComponent<Renderer>();
        textureColor = r.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        textureColor.a = Mathf.Clamp(t,0,1);
        r.material.color = textureColor;
	   
	}
}
