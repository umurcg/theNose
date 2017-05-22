using UnityEngine;
using System.Collections;

public class AssignMaterialToChildren : MonoBehaviour
{
    public Material materialToAssin;

    public void assignToAllChildren()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = materialToAssin;
        }
    }

}
