﻿using UnityEngine;
using System.Collections;

//This script switches gamme objects.
//It disables the oldmodel and enables the newModel after a delay and animation of player.
//rootGameobject is animation owner


public class SwitchObjects : MonoBehaviour, IClickAction {

    public string animationName;
    public float delay;
    public GameObject oldModel;
    public GameObject newModel;
    public GameObject rootGameObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Action()
    {
        StartCoroutine("change");
    }

    IEnumerator change()
    {
        float t = delay;
        PlayerComponentController pcc = rootGameObject.GetComponent<PlayerComponentController>();
        if (pcc != null) 
        pcc.StopToWalk();


       Animator anim= rootGameObject.GetComponent<Animator>();
        if(anim!=null&&animationName!="")
        anim.SetBool(animationName, true);

        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

  

        oldModel.SetActive(false);
        newModel.SetActive(true);



     

        if (anim != null && animationName != "")
            anim.SetBool(animationName, false);

        yield return new WaitForSeconds(1);

        if (pcc != null)
            pcc.ContinueToWalk();

        
        finished();
    }

    void finished()
    {
        //This methods are interface methods. So finished function includes special case methods.
      IFinishedSwitching[] ifss=  GetComponents<IFinishedSwitching>();
      foreach (IFinishedSwitching iff in ifss)
        {
            iff.finishedSwitching();
        }
    }



}