using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script is rewritten collecting functionality.
//It works with IClickAction interface.
//There are 4 different option for collecting types.
//Hands can only collect one object
//If you choos dontcare it fills all hands then it starts to send hidden envanter.
//There are interface methods called during collecting and uncollecting.
//Uncollecting performes one by one.


public class CollectObjectAndDropAnywhere : MonoBehaviour, IClickAction{

    public static GameObject next;

    public enum Hand {DontCare,None,Right,Left};
    public Vector3 scale;
    public Hand hand;
    public Vector3 offset;


    Transform player;
    Transform rightHand;
    Transform leftHand;
    

    Collider col;
    Vector3 originalScale;
    Quaternion originalRotation;

    public bool collectEvenFull = false;
    bool collected = false;


    // Use this for initialization
    void Awake () {

        

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rightHand = player.Find("Armature/Torso/Chest/Arm_R/Hand_R");
        leftHand = player.Find("Armature/Torso/Chest/Arm_L/Hand_L");

        originalScale = transform.localScale;
        originalRotation = transform.rotation;
        col = GetComponent<Collider>();
        


    }


    
    // Update is called once per frame
    void Update () {

        if (next == gameObject)
        {
          //  print(gameObject.name);
            if (Input.GetMouseButtonUp(1) && collected)
            {
                //   UnCollect(player.transform.position + player.transform.forward * 2);
                RaycastHit hit;
                              
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
               
                    if (hit.transform.CompareTag("Floor"))
                    {
                        UnCollect(hit.point);
                    }
                  }

            }
        }

	}

    public IEnumerator findNewNext() {

        yield return new WaitForFixedUpdate();

       CollectObjectAndDropAnywhere[] objects= Object.FindObjectsOfType<CollectObjectAndDropAnywhere>();
       for(int i = 0; i < objects.Length; i++)
        {
            CollectObjectAndDropAnywhere aday = objects[i];
            if (aday.collected)
            {
                next = aday.gameObject;
            }
        }
    }


    public void Action()
    {
        Collect();
    }

    public void Collect()
    {
        if(hand==Hand.DontCare)
        if (rightHand.childCount==0)
        {
            hand = Hand.Right;
        }else if (leftHand.childCount == 0)
        {
            hand = Hand.Left;
        }else
        {
            hand = Hand.None;
        }
     

        switch (hand)
        {
            case Hand.None:
                try
                {
                    GetComponent<ICollectableObjectAction>().startingToCollecting();
                }
                catch
                {
                    print("no interface");
                }
                partiallyDisableObject(false);
                collected = true;
                return;
            case Hand.Right:
                if (rightHand.childCount > 0)
                    return;
                try
                {
                    GetComponent<ICollectableObjectAction>().startingToCollecting();
                }
                catch
                {
                    print("no interface");
                }
                col.enabled = false;
                transform.parent = rightHand;
                transform.position = rightHand.position + offset;
                transform.localScale = scale;
                collected = true;
                next = gameObject;
                return;
            case Hand.Left:
                if (leftHand.childCount > 0)
                    return;

                try
                {
                    GetComponent<ICollectableObjectAction>().startingToCollecting();
                }
                catch
                {
                    print("no interface");
                }
                col.enabled = false;
                transform.parent = leftHand;
                transform.position = leftHand.position + offset;
                transform.localScale = scale;
                collected = true;
                next = gameObject;
                return;
            default:
          
                return;

        }
       

    
    }

    void partiallyDisableObject(bool boo)
    {
        try
        {
            GetComponent<Renderer>().enabled = boo;
            GetComponent<Collider>().enabled = boo;


        }
        catch
        {
            print("nalll referansssss");
        }

        
        
    }

    public virtual void UnCollect(Vector3 pos)
    {
        

        try
        {
            GetComponent<ICollectableObjectAction>().startingToUncollecting();
        }
        catch
        {
            print("no interface");
        }


        if (hand == Hand.None)
        {
            partiallyDisableObject(true);
        }


        col.enabled = true;
        transform.parent = null;
        transform.position = pos;
        transform.localScale = originalScale;
        transform.rotation = originalRotation;
        collected = false;



        try
        {
           GetComponent<ICollectableObjectAction>().finishedToUncollecting();
        }
        catch
        {
            print("no interface");
        }


        next = null;
        StartCoroutine(findNewNext());
    }



}
