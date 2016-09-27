﻿using UnityEngine;
using System.Collections;

public class MaterialPropertyAutomaticSlider : MonoBehaviour {
	public Material mat;
	public float speed=1;
	float brightness=0;
	bool increasing=true;
	public string matPropert = "_Glossiness";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (increasing) {
			brightness= Mathf.Clamp (brightness + Time.deltaTime*speed, 0, 1);

			if (brightness == 1)
				increasing = false;

		} else {
			brightness= Mathf.Clamp (brightness - Time.deltaTime*speed, 0, 1);
	

			if (brightness == 0)
				increasing = true;
		}


		mat.SetFloat (matPropert, brightness);

	}
}