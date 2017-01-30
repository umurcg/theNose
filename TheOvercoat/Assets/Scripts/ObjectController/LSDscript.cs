using UnityEngine;
using System.Collections;

public class LSDscript : MonoBehaviour {


    public float maxScalex, minScalex, maxScaley, minScaley, maxScalez, minScalez,minSpeed,maxSpeed;
    SliderScript[] sliders;
    SliderScript sx,sy,sz;
    public float aimx, aimy, aimz;

    // Use this for initialization
    void Start () {
        sliders = GetComponents<SliderScript>();
        
        if (sliders.Length<3)
        {
            print("there is no enough slider you fucker.");
            this.enabled = false;
        }

        sx = sliders[0];
        sy = sliders[1];
        sz = sliders[2];
                
    }
	
	// Update is called once per frame
	void Update () {

        if (aimx == sx.value)
        {
            sx.speed = Random.Range(minSpeed, maxSpeed);
            aimx = Random.Range(minScalex, maxScalex);
            sx.slideTo(aimx);
        }
        if (aimy == sy.value)
        {
            sy.speed = Random.Range(minSpeed, maxSpeed);
            aimy = Random.Range(minScaley, maxScaley);
            sy.slideTo(aimy);
        }
        if (aimz == sz.value)
        {
            sz.speed = Random.Range(minSpeed, maxSpeed);
            aimz = Random.Range(minScalez, maxScalez);
            sz.slideTo(aimz);
        }


        transform.localScale = new Vector3(sx.value, sy.value, sz.value);

	}






}
