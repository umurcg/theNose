using UnityEngine;
using System.Collections;

public class MaterialController : MonoBehaviour {

    public Material nonActiveMat;
    public Material activeMat;
    public string activeObjectTag = "ActiveObject";
    public bool controlChildren = false;

    bool active = false;
    Renderer[] renderers;
    Renderer ownerRenderer;
	// Use this for initialization
	void Start () {
        renderers = GetComponentsInChildren<Renderer>();
        ownerRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    
        if(!active && transform.tag==activeObjectTag)
        {
            setMaterialToRenderer(activeMat);
            active = true;
        } else if(active && transform.tag == "Untagged")
        {
            setMaterialToRenderer(nonActiveMat);
            active = false;
        }

	}


    void setMaterialToRenderer(Material mat)
    {
        if (controlChildren|| ownerRenderer==null )
        {
            foreach (Renderer rend in renderers)
            {
                rend.material = mat;
            }
        }
        else
        {
            ownerRenderer.material = mat;

        }
    }
}
