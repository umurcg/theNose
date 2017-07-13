using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using System.Linq;

//This class makes all objects that is assigned in array transparent and opaque.
public class MakeTransparent : MonoBehaviour {

    //For debug
    public bool switchTransparent = false;
    bool prevBool;

    public Material transparentMaterial;
    public GameObject[] objects;
    List<Renderer> allRenderers;

    Material opaqueMaterial;

    bool isTransparent=false;


	// Use this for initialization
	void Start () {

        allRenderers = new List<Renderer>();

	    foreach(GameObject obj in objects)
        {
            Renderer[] rends = obj.GetComponentsInChildren<Renderer>();
            
            allRenderers = allRenderers.Concat(rends).ToList<Renderer>();
        }
	}
	
    [ContextMenu ("Make Transparent")]
    public void makeTransparent()
    {
        foreach (Renderer o in allRenderers)
        {
            makeTransparent(o.gameObject);
        }
    }

    [ContextMenu("Make Opaque")]
    public void makeOpaque()
    {
        foreach (Renderer o in allRenderers)
        {
            makeOpaque(o.gameObject);
        }
    }

	// Update is called once per frame
	void Update () {


        if (!prevBool && switchTransparent)
        {
            foreach (Renderer o in allRenderers)
            {
                makeTransparent(o.gameObject);
            }
            prevBool = switchTransparent;
        }
        else if (prevBool && !switchTransparent)
        {
            foreach (Renderer o in allRenderers)
            {

                makeOpaque(o.gameObject);
            }

            prevBool = switchTransparent;
        }


    }

    public void switchTransparentOpaque()
    {
        if(!isTransparent)
        {
            makeTransparent();

            isTransparent = true;
        }
        else
        {
            makeOpaque();

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
