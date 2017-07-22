using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]
public class TextAlphaSlider : MonoBehaviour {

    public float maxAlpha = 1;
    public float minAlpha = 0;
    public float speed = 1f;
    float alpha = 0;

    Text text;

    bool increasing = true;
   
	// Use this for initialization
	void Start () {

        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);
        minAlpha = Mathf.Clamp(minAlpha, 0, 1);

        if (maxAlpha < minAlpha) enabled = false;

        text = GetComponent<Text>();

        alpha = text.color.a;
	}
	
	// Update is called once per frame
	void Update () {

        if (increasing)
        {
            alpha += Time.deltaTime * speed;

            if (alpha >= 1)
            {
                alpha = 1;
                increasing = false;
            }
        }
        else{
            alpha -= Time.deltaTime * speed;

            if (alpha <= 0)
            {
                alpha = 0;
                increasing = true;
            }

        }

        Color col = text.color;
        col.a = alpha;
        text.color = col;


	}
}
