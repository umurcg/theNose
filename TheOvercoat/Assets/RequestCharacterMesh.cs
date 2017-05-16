using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//You can request mesh of common people from this script
public class RequestCharacterMesh : MonoBehaviour {

    public GameObject men, woman;

    public enum gender { men=0,woman=1, both=2};


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public Mesh requestRandomMesh(gender g)
    {
        GameObject parent=null;

        switch (g)
        {
            case gender.both:
                gender randomG = (gender)Random.Range(0, 1);
                parent = (randomG == gender.men) ? men : woman;
                break;

            case gender.men:
                parent = men;
                break;
            case gender.woman:
                parent = woman;
                break;
            
        }

      
        return parent.transform.GetChild(Random.Range(0, parent.transform.childCount-1)).GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

    }




    public Mesh requestMesh(string name)
    {
        List<GameObject> allMeshObjects=new List<GameObject>();

        for (int i = 0; i < woman.transform.childCount; i++) allMeshObjects.Add(woman.transform.GetChild(i).gameObject);
        for (int i = 0; i < men.transform.childCount; i++) allMeshObjects.Add(woman.transform.GetChild(i).gameObject);

        foreach(GameObject meshObj in allMeshObjects)
        {
            if(meshObj.name==name) return meshObj.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        }

        return null;

    }

    public Mesh requestMesh(gender g, int index)
    {
        GameObject parent = (g == gender.men) ? men : woman;
        return parent.transform.GetChild(index).GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
    }

}
