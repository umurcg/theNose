using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChangeMaterialWhenInside : MonoBehaviour {

    public Material mat;

    //While this scripts doesn't detects objects that are already inside of collider at start of game you should put them in that array.
    //Objects in that array will forced to change it materials.
    public GameObject[] objectsAlreadyInside;
    Dictionary<GameObject, Material> originalMaterials;
    
	// Use this for initialization
	void Start () {
                
        originalMaterials = new Dictionary<GameObject, Material>();
        foreach(GameObject obj in objectsAlreadyInside)
        {
            ChangeMaterial(obj);
        }

    }
	


    private void ChangeMaterial(GameObject obj) { 
    
        if (!originalMaterials.ContainsKey(obj))
        {
            
            originalMaterials.Add(obj, obj.transform.GetComponentInChildren<SkinnedMeshRenderer>().material);
            obj.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
        }
    }



    private void RecoverMateral(GameObject obj)
    {
        if (originalMaterials.ContainsKey(obj))
        {
            obj.gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = originalMaterials[obj];
            originalMaterials.Remove(obj);

        }
    }

    void OnTriggerEnter(Collider col)
    {
        ChangeMaterial(col.transform.gameObject);
        
    }

    void OnTriggerExit(Collider col)
    {
        RecoverMateral(col.transform.gameObject);

    }

}

