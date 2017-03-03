using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script changes material to another material from list.


public class ChangeMaterial : MonoBehaviour {
	Material originalMat;
    Renderer rend;
	public List<Material> matList;
	public int index;


	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();

		originalMat = rend.material;
		matList.Insert (0,originalMat);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void change(){
		if (index < matList.Count-1) {
			index++;
		} else {
			index = 0;
		}

		rend.material = matList [index];

	}

    public void changeWithIndex(int index)
    {
        if (index >= matList.Count)
        {
            Debug.Log("Index is bigger than materail list count");
            return;
        }
        this.index = index;
        rend.material = matList[index];
    }

}
