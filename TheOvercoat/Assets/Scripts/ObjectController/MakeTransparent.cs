using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

//This class makes all objects that is assigned in array transparent and opaque.
public class MakeTransparent : MonoBehaviour {

    //For debug
    public bool switchTransparent = false;
    bool prevBool;

    public Material transparentMaterial;
    public GameObject[] objects;

    Material opaqueMaterial;

    bool isTransparent=false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        if (!prevBool && switchTransparent)
        {
            foreach (GameObject o in objects)
            {
                makeTransparent(o);
            }
            prevBool = switchTransparent;
        }
        else if (prevBool && !switchTransparent)
        {
            foreach (GameObject o in objects)
            {
                makeOpaque(o);
            }
            prevBool = switchTransparent;
        }


    }

    public void switchTransparentOpaque()
    {
        if(!isTransparent)
        {
            foreach (GameObject o in objects)
            {
                makeTransparent(o);
            }

            isTransparent = true;
        }
        else
        {
            foreach (GameObject o in objects)
            {
              
                makeOpaque(o);
            }

            isTransparent = false;
        }
    }

    //make obj transparent 
    public void makeTransparent(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();

        //assign opquematerial
        if (opaqueMaterial == null)
        {
            opaqueMaterial = rend.material;
        }

        rend.material = transparentMaterial;

        Timing.RunCoroutine(Vckrs._fadeObject(obj, 1f));
             
        
    }

    public void makeOpaque(GameObject obj)
    {
        Timing.RunCoroutine(_makeOpaque(obj));
    }

    //make object opaque if opaque material is not null
    IEnumerator<float> _makeOpaque(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();

        //Checks material is transparent.
        if (rend.material.GetFloat("_Mode") != 3)
        {
            Debug.Log("Material isn't transparent");
            yield break;
        }
        

        IEnumerator<float> handler= Timing.RunCoroutine(Vckrs._fadeObject(obj, 1f));
        yield return Timing.WaitUntilDone(handler);

        if (opaqueMaterial != null)
        {
            rend.material = opaqueMaterial;
        }

    }
}
