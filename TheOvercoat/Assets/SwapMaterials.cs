using UnityEngine;
using System.Collections;

public class SwapMaterials : MonoBehaviour {

    public Material colorPalette;
    public Material dynamicColorPalette;

   Renderer rend;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void activate()
    {
        rend.material = dynamicColorPalette;
    }

    public void deactivate()
    {
        rend.material = colorPalette;
    }
}
