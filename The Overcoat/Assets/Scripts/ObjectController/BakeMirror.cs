using UnityEngine;
using System.Collections;


//This script bakes reflection probe.
//If game object isn't null then it enables all objects in go array, bake and then disables them.

public class BakeMirror : MonoBehaviour {
    ReflectionProbe rp;
    public GameObject[] go;


	// Use this for initialization
	void Start () {
        rp = GetComponent<ReflectionProbe>();
	}
	
	// Update is called once per frame
	void Update () {

	
	}

    public IEnumerator bake()
    {
        print("ENTER ENUMERATOR");
        if (go.Length>0&&go!=null)
        {
            foreach (GameObject obj in go)
            {
                obj.SetActive(true);
               
            }

            Debug.Log("before wait");
            yield return new WaitForEndOfFrame();
            rp.RenderProbe();

            yield return new WaitForEndOfFrame();

            //yield return new WaitForFixedUpdate();
            Debug.Log("after wait");
            foreach (GameObject obj in go)
            {
                obj.SetActive(false);
            }

        }
        else
        {

            rp.RenderProbe();
        }

    }
}
