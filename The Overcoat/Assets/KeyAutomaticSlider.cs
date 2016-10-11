using UnityEngine;
using System.Collections;

public class KeyAutomaticSlider : MonoBehaviour {
    
    public float speed = 1;
    bool increasing = true;
    SkinnedMeshRenderer smr;
    float value;
    // Use this for initialization
    void Start () {
        smr = GetComponent<SkinnedMeshRenderer>();
        value = smr.GetBlendShapeWeight(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (increasing)
        {
           value = Mathf.Clamp(value+ Time.deltaTime * speed, 0, 100);

            if (value == 100)
                increasing = false;

        }
        else
        {
            value = Mathf.Clamp(value - Time.deltaTime * speed, 0, 100);


            if (value == 0)
                increasing = true;
        }

        smr.SetBlendShapeWeight(0, value);

    }
}
