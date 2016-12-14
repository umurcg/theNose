using UnityEngine;
using System.Collections;



//_MaterialPropertyAutomaticSlider
//_Dependent to: Material

//This script increases and decreases a porperty of material contionusly.

public class MaterialPropertyAutomaticSlider : MonoBehaviour {
	public Material mat;
	public float speed=1;
	float brightness=0;
	bool open=true;
	public string matPropert = "_Glossiness";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {
			brightness= Mathf.Clamp (brightness + Time.deltaTime*speed, 0, 1);

			if (brightness == 1)
				open = false;

		} else {
			brightness= Mathf.Clamp (brightness - Time.deltaTime*speed, 0, 1);
	

			if (brightness == 0)
				open = true;
		}


		mat.SetFloat (matPropert, brightness);

	}
}
