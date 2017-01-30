using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class addRemoveLCDscript : MonoBehaviour {

    public GameObject[] objects;
    public bool addBool = false;
    public bool removeBool = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (addBool )
        {
            addBool = false;
            addScript<LSDscript>(objects);

        }else if (removeBool)
        {
            removeBool = false;
            removeScript<LSDscript>(objects);
        }

	}


    public void add()
    {
        addScript<LSDscript>(objects);

    }

    public void remove()
    {
        addScript<LSDscript>(objects);
    }

    List<T> addScript<T>(GameObject[] objs)
    {

        //List holding all created components
        List<T> components = new List<T>(); ;

        //Add component if object doesn't have that component.
        foreach (GameObject obj in objs)
        {
            T comp = obj.GetComponent<T>();
            if (comp == null)
            {
               obj.AddComponent(typeof(T));
               T addedComp = obj.GetComponent<T>();
                components.Add(addedComp);
            }
        }

        return components;

    }


    void removeScript<T>(GameObject[] objs)
    {

        //Remove component if obj does have that component.
        foreach (GameObject obj in objs)
        {
            T comp = obj.GetComponent<T>();
            if (comp!=null)
            {
                MonoBehaviour compMB = obj.GetComponent<T>() as MonoBehaviour;
                Destroy(compMB);

            }
        }
    }

}
