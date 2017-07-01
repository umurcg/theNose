using UnityEngine;
using System.Collections;

public class AssignMaterialToChildren : MonoBehaviour
{
    public Material materialToAssin;

    public void assignToAllChildren()
    {
        assignToAllChildren(materialToAssin, gameObject);
    }

    public static void assignToAllChildren(Material mat, GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = mat;
        }
    }

}
