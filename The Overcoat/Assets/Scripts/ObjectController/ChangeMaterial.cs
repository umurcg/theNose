using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeMaterial : MonoBehaviour {
	Material originalMat;

	Renderer rend;
	public List<Material> matList;
	int index;


	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();

		originalMat = rend.material;
		matList.Add (originalMat);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void change(){
		if (index < matList.Count) {
			index++;
		} else {
			index = 0;
		}

		rend.material = matList [index];

	}

}
