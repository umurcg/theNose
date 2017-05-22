using UnityEngine;
using System.Collections;

public class CreateBoxColliderForLODObjects : MonoBehaviour {

	public void createCollider()
    {
        LODGroup[] lod = GetComponentsInChildren<LODGroup>();
        for (int i = 0; i < lod.Length; i++)
        {
            lod[i].transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
        }
    }
}

