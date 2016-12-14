using UnityEngine;
using System.Collections;

public class SlideTextureWithMouse : MonoBehaviour {


    public float scrollSpeed = 0.5F;
    public Renderer rend;
    float x, y;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
        float offset = Time.time * scrollSpeed;

        x += Input.GetAxis("Mouse X")*scrollSpeed;
        y += Input.GetAxis("Mouse Y") * scrollSpeed;

        rend.material.SetTextureOffset("_MainTex", new Vector2(y,x));
    }
}
