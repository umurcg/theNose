using UnityEngine;
using System.Collections;

public class BakeMirror : MonoBehaviour {
    ReflectionProbe rp;
    public GameObject go;
	// Use this for initialization
	void Start () {
        rp = GetComponent<ReflectionProbe>();
	}
	
	// Update is called once per frame
	void Update () {

	
	}

    public void bake()
    {
        go.SetActive(true);
        rp.RenderProbe();
        go.SetActive(false);

    }
}
