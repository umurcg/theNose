using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//This script force to player to turn and go back.
//It is an obstacle for player to prevent it to reach disabled areas.
//While it turns back it also provide a subtitle.


public class ShallNotPass : MonoBehaviour
{


    public string message;
    public float duration = 3;
    public float speed = 0.05f;
    public float subtitleWait = 3;
    public float waitBeforeMove = 3;
    bool hasStartedCouretine= false;
    Text subtitle;

    // Use this for initialization
    void Start()
    {
        subtitle = SubtitleFade.subtitles["CharacterSubtitle"];
        if (!subtitle) Debug.Log("Couldnt find character subtitle.");
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
   
    IEnumerator InvokeSubtitle(Text sub, string txt)
    {

        sub.text = txt;
        float time = 3;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        sub.text = "";
    }

    

    IEnumerator Rotate(GameObject go,float angle)
    {
        PlayerComponentController pcc = go.GetComponent<PlayerComponentController>();
        //CharacterController rb = go.GetComponent<CharacterController>();
        

        if (pcc != null)
            pcc.StopToWalk();

        //Wait
        while (waitBeforeMove > 0)
        {
            waitBeforeMove -= Time.deltaTime;
            yield return null;
        }

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
