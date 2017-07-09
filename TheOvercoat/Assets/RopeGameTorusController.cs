using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class RopeGameTorusController : MonoBehaviour {

    public RopeGameController RGC;
    public Material purple;
    public Material red;
    public Material green;
    Material originalMat;

    enum color { original,red,green,purple};


    GameObject key;

    public GameObject insideObject;

    ClickAndDrag cacOfInsideObject;

    Renderer rend;

    // Use this for initialization
    void Awake () {
        rend = GetComponent<Renderer>();
        originalMat = rend.material;
        
	}


    // Update is called once per frame
    void Update () {

        if (insideObject != null)
        {
            if (cacOfInsideObject.touched)
            {
                changeMaterial(color.purple);
            }
            //User release mouse button
            else
            {
                //Debug.Log("inside but not touchign");
                if (insideObject == key)
                {
                    changeMaterial(color.green);
                   Timing.RunCoroutine(sendMessage(1, "untie"));
                    enabled = false;

                }
                else
                {
                    changeMaterial(color.red);
                    Timing.RunCoroutine(sendMessage(1, "resetGame"));
                    enabled = false;
                        
                }
            }

        }

	}

    IEnumerator<float> sendMessage(float delay, string message)
    {
        yield return Timing.WaitForSeconds(delay);


        Debug.Log("Senging messsage");
        RGC.gameObject.SendMessage(message,this);

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter trigger");
        if (other.GetComponent<ClickAndDrag>() != null)
        {
            insideObject = other.gameObject;
            cacOfInsideObject = insideObject.GetComponent<ClickAndDrag>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (insideObject != null && other.gameObject == insideObject)
        {
            insideObject = null;
            cacOfInsideObject = null;
            changeMaterial(color.original);
        }

    }

    public void setKey(GameObject key)
    {
        this.key = key;
    }

    public void setRopeGameController(RopeGameController controller)
    {
        RGC = controller;
    }

    void changeMaterial(color c)
    {
        Material mat=null;
        switch (c)
        {
            case color.green:
                mat = green;
                break;
            case color.original:
                mat = originalMat;
                break;
            case color.purple:
                mat = purple;
                break;
            case color.red:
                mat = red;
                break;  
            
        }


        if (rend.material == mat) return;

        rend.material = mat;


    }

    public void destroyTorusAndKey()
    {
        //if (key == null) Debug.Log("No key");
        //Debug.Log("Destroyign");
        Destroy(key);
        Destroy(gameObject);
    }

}
