﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShallNotPass : MonoBehaviour
{

    public string message;
    public GameObject subtitle;
    public float duration = 10;
    public float speed = 1;
    public float subtitleWait = 3;
    bool hasStartedCouretine= false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
 
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" &&    (hasStartedCouretine == false))
        {
            hasStartedCouretine = true;
            if (message != null&&subtitle!=null)
            {
                StartCoroutine(InvokeSubtitle(subtitle,message));

            }
            StartCoroutine(Rotate(col.gameObject,180f));

            //deneme(col.gameObject);

        }

    }
   
    IEnumerator InvokeSubtitle(GameObject sub, string txt)
    {
        Text t = sub.GetComponent<Text>();
        t.text = txt;
        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        t.text = "";
    }

    IEnumerator Rotate(GameObject go,float angle)
    {
        PlayerComponentController pcc = go.GetComponent<PlayerComponentController>();
        CharacterController rb = go.GetComponent<CharacterController>();
        if (pcc != null)
            pcc.StopToWalk();
        float ratio = 0;
        Vector3 initialRot = go.transform.eulerAngles;
        while (ratio < 1)
        {
            ratio += Time.deltaTime;
            go.transform.eulerAngles = Vector3.Lerp(initialRot, initialRot + angle * Vector3.up, ratio);
            yield return null;

        }

        StartCoroutine(TurnBackAndWalk(go));
    }

    IEnumerator TurnBackAndWalk(GameObject go)
    {

        Vector3 back = go.transform.forward;
        PlayerComponentController pcc = go.GetComponent<PlayerComponentController>();
        CharacterController rb = go.GetComponent<CharacterController>();
        if (pcc != null)
            pcc.StopToWalk();

        rb.enabled = true;

     


        float timer = duration;

        while (timer > 0)
        {

            timer -= Time.deltaTime;

            rb.Move(back * speed);
            yield return null;




        }

        timer = 0;
        if (pcc != null)
        {
            pcc.ContinueToWalk();
        }
        hasStartedCouretine = false;
    }


}
