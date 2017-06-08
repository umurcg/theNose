using UnityEngine;

[AddComponentMenu("Rendering/SetRenderQueue")]

public class SetRenderQueue : MonoBehaviour
{

    [SerializeField]
    protected int[] m_queues = new int[] { 3000 };

    protected void Awake()
    {
        Material[] materials = GetComponent<Renderer>().materials;
        //Debug.Log("Number of materials is " + materials.Length +" of "+ gameObject.name);
        for (int i = 0; i < materials.Length && i < m_queues.Length; ++i)
        {
            
            materials[i].renderQueue = m_queues[i];
        }
    }


    //private void Update()
    //{
    //    Material[] materials = GetComponent<Renderer>().materials;
    //    for (int i = 0; i < materials.Length && i < m_queues.Length; ++i)
    //    {
    //        materials[i].renderQueue = m_queues[i];
    //    }
    //}
}