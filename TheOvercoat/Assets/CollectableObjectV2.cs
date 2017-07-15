using MovementEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//More stable version of collectable object
public class CollectableObjectV2 : MonoBehaviour, IClickAction{

    public enum handToCollect { Right,Left,Both};
    public handToCollect Hand = handToCollect.Right;

    public static GameObject rightHandObj,leftHandObj;
    public float scale=1;

    GameObject rightHand, leftHand;
    Collider col;
    Rigidbody rb;

    bool collected=false;

    public Texture2D cursorTexture;
    CursorImageScript cis;

    public void Action()
    {
        //Debug.Log("aCTİPOMN");
        collect();
    }

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        cis = CharGameController.getOwner().GetComponent<CursorImageScript>();

    }

    // Use this for initialization
    void Start () {

        rightHand = CharGameController.getHand(CharGameController.hand.RightHand);
        leftHand = CharGameController.getHand(CharGameController.hand.LeftHand);
        
    }
	
	void collect()
    {


        if (Hand == handToCollect.Right && (rightHand.transform.childCount > 1 || rightHandObj!=null)) return;

        if (Hand == handToCollect.Left && (leftHand.transform.childCount > 1 || leftHandObj!=null)) return;

        if (Hand == handToCollect.Both)
        {

            if(rightHand.transform.childCount == 0 && rightHandObj==null)
            {
                Hand = handToCollect.Right;
            }else if(leftHand.transform.childCount == 0 && leftHandObj==null)
            {
                Hand = handToCollect.Left;
            }
            else
            {
                return;
            }
        
        }

        if (Hand == handToCollect.Right)
        {
            transform.parent = rightHand.transform;
            rightHandObj = gameObject;
        }
        else
        {
            transform.parent = leftHand.transform;
            leftHandObj = gameObject;

        }

        Debug.Log("passed conditions");
        
        transform.localScale = Vector3.one * scale;
        transform.localPosition = Vector3.zero;

        //if (rb) rb.isKinematic = true;
        //if (col) col.enabled = false;

        collected = true;

        if (cursorTexture != null) cis.setExternalTexture(cursorTexture);

    }

    public void unCollect(Vector3 pos, float scale)
    {
        unCollect(pos);
        transform.localScale = scale * Vector3.one;
    }

    public void unCollect(float scale)
    {
        unCollect();
        transform.localScale = scale * Vector3.one;
    }

    public void unCollect(Vector3 pos)
    {
        unCollect();
        transform.position = pos;
    }

    public void unCollect()
    {
        //Debug.Log("Uncollected");
        if(!isCollected()) return;

        if (rightHandObj == gameObject) rightHandObj = null;
        if (leftHandObj == gameObject) leftHandObj = null;



        transform.parent = null;


        //if (rb) rb.isKinematic = true;
        //if (col) col.enabled = false;

        collected = false;

        if (cursorTexture != null) cis.resetExternalCursor();
    }

    public bool isCollected() { return collected; }
    
    public void goAndCollectObject(UnityEngine.AI.NavMeshAgent agent) { Timing.RunCoroutine(_goAndCollectObject(agent)); }

    public IEnumerator<float> _goAndCollectObject(UnityEngine.AI.NavMeshAgent agent)
    {
        Vector3 posOnNavmesh = gameObject.transform.position;
        Vckrs.findNearestPositionOnNavMesh(posOnNavmesh, agent.areaMask, 20f, out posOnNavmesh);

        agent.SetDestination(posOnNavmesh);
        agent.isStopped = false;

        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(agent.gameObject));
        yield return Timing.WaitUntilDone(handler);

        collect();

    }
}
