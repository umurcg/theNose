using UnityEngine;
using System.Collections;



//_changeTransparency.cs
//_Dependent to: 
//This script changes transperency of 2d image.
//It is written for controlling 2d image from Cinema Suit


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
