using UnityEngine;
using System.Collections;
//This script change blend key autimatically.
//It goes all the way up then all the way down with loop.


public class KeyAutomaticSlider : MonoBehaviour {
    
    public float speed = 1;
    bool open = true;
    SkinnedMeshRenderer smr;
    float value;
    // Use this for initialization
    void Start () {
        smr = GetComponent<SkinnedMeshRenderer>();
        value = smr.GetBlendShapeWeight(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (open)
        {
           value = Mathf.Clamp(value+ Time.deltaTime * speed, 0, 100);

            if (value == 100)
                open = false;

        }
        else
        {
            value = Mathf.Clamp(value - Time.deltaTime * speed, 0, 100);


            if (value == 0)
                open = true;
        }

        smr.SetBlendShapeWeight(0, value);

    }
}
