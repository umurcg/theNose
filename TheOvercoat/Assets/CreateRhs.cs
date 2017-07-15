using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CreateRhs : MonoBehaviour
{

    List<GameObject> newObjects = new List<GameObject>();

    // -------------------------------------------------------------------------
    [ContextMenu("Create Russian Boxes")]
    public void CreateBoxes()
    {

        RecursiveBoxCreate(transform.localScale.y);
        for (var i = 0; i < newObjects.Count; ++i)
        {
            newObjects[i].transform.parent = gameObject.transform;
        }

    }


    // ---------------------------------------------------------------------------
    void RecursiveBoxCreate(float scale)
    {

        if (scale > 0f)
        {

            GameObject child = Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
            child.transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
            newObjects.Add(child);
            // You may need to tweak the value here so that it is less than your nav height setting
            RecursiveBoxCreate(scale - 0.5f);
        }

    }

    [ContextMenu("ClearChildren")]
    public void clearAllChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

    }

}