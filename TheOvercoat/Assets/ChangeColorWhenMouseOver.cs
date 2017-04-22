using UnityEngine;
using System.Collections;

public class ChangeColorWhenMouseOver : MonoBehaviour {

    public Material materialOnOver;
    Material originalMat;
    Renderer rend;
	// Use this for initialization
	void Awake () {
        rend = GetComponent<Renderer>();
        originalMat = rend.material;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseEnter()
    {
        rend.material = materialOnOver;
    }
    private void OnMouseExit()
    {
        rend.material = originalMat;
    }
}
