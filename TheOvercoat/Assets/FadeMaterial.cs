using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class FadeMaterial : MonoBehaviour {

    public Material materialToFade;

	// Use this for initialization
	void Start () {

        Timing.RunCoroutine(_fadeOut(1f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator<float> _fadeOut(float speed)
    {

        StandardShaderUtils.ChangeRenderMode(materialToFade, StandardShaderUtils.BlendMode.Fade);

        float alpha = 1;
        Color col = materialToFade.color;


        while (alpha > 0)
        {
            alpha -= Time.deltaTime*speed;
            col.a = alpha;
            materialToFade.color = col;
            yield return 0;
        }

        alpha =0;
        col.a = alpha;
        materialToFade.color = col;



        yield break;

    }
}
