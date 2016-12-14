﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
//This is a module for generic methods.


public class Vckrs : MonoBehaviour
{

 
    public static IEnumerator<float> _setLightIntensity(GameObject light, float speed,float intensity)
    {
        Light l = light.GetComponent<Light>();
        float initialIntesity = l.intensity;
        float ratio = 0;
         
       while (ratio < 1)
       {
             ratio += Time.deltaTime * speed;
            l.intensity = Mathf.Lerp(initialIntesity, intensity,ratio);
            yield return 0;
        }
           


    }

    public static IEnumerator<float> waitUntilStop(GameObject obj, float tol)
    {
        Vector3 pos = obj.transform.position;
        yield return Timing.WaitForSeconds(0.5f);
        while (Vector3.Distance(obj.transform.position, pos) > tol)
        {
            //print((Vector3.Distance(obj.transform.position, pos)));
            pos = obj.transform.position;
            yield return 0;
            
        }

    }



    public static IEnumerator<float> _Tween(GameObject go, Vector3 aim, float speed)
    {

        Vector3 initialPosition = go.transform.position;

        float ratio = 0;


        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;

            go.transform.position = Vector3.Lerp(initialPosition, aim, ratio);



            yield return 0;

        }


    }

    static public IEnumerator<float> _lookTo(GameObject go, Vector3 aim, float speed)
    {
   
        Vector3 localAim = go.transform.position + aim;
  

        localAim.y = go.transform.position.y;


       Quaternion initialRot = go.transform.rotation;
       Quaternion aimRot = Quaternion.LookRotation(localAim - go.transform.position);
       float  ratio = 0;


        while (ratio<1)
        {

            ratio += Time.deltaTime * speed;
            go.transform.rotation = Quaternion.Lerp(initialRot, aimRot, ratio);

            yield return 0;
        }
        go.transform.rotation = aimRot;

    }


    //This method pace object between two position.
    //It uses nav mesh.
    static public IEnumerator<float> _pace(GameObject obj, Vector3 aim1, Vector3 aim2)
    {
        NavMeshAgent nav = obj.GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.Resume();
        nav.SetDestination(aim1);
        IEnumerator<float> handler=Timing.RunCoroutine(Vckrs.waitUntilStop(obj, 0.0001f));

        yield return Timing.WaitUntilDone(handler);

        
        Vector3 aim = aim2; 
        while (true)
        {
            //print("while");
            nav.SetDestination(aim);
            handler = Timing.RunCoroutine(Vckrs.waitUntilStop(obj, 0.0001f));

            //print("waiting");
            yield return Timing.WaitUntilDone(handler);

            //print("waiting finished");

            if (aim == aim1)
            {
                aim = aim2;
            }else
            {
                aim = aim1;
            }

            
        }
        
    }


    //This method activates all scripts in an object.
    //Alsa it change its tag to ActiveObject
    static public void ActivateAnotherObject(GameObject obj)
    {
        obj.SetActive(true);
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;

            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }
        }
        
        obj.transform.tag = "ActiveObject";

    }

    public static void DisableAnotherObject(GameObject go)
    {
        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {

            script.enabled = false;


            if (script is ChangeMaterial)
            {
                ChangeMaterial cm = (ChangeMaterial)(script);
                cm.change();
            }
        }



        go.transform.tag = "Untagged";

    }

    public static void testPosition(Vector3 pos)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = pos;
    }

    public static IEnumerator<float> followObject(NavMeshAgent agent, GameObject obj)
    {
        agent.Resume();
        while (true)
        {
            agent.SetDestination(obj.transform.position);
            yield return 0;
        }
    }

   


    public static IEnumerator<float> _cameraSize(Camera cam,float size,float speed)
    {
       
        if (cam.orthographicSize > size)
        {
            
            while (cam.fieldOfView > size)
            {
                cam.orthographicSize -= Time.deltaTime * speed;
                yield return 0;
            }

        }
        else
        {
           
            while (cam.orthographicSize < size)
            {
                //print("increase");
                cam.orthographicSize += Time.deltaTime * speed;
                yield return 0;
            }
        }
    }


    public static IEnumerator<float> _fadeInfadeOut(GameObject black,float speed)
    {
        RawImage r = black.GetComponent<RawImage>();
        
        Color textureColor=r.color;
        float a = textureColor.a;

     

        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                r.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            r.color = textureColor;

        }else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                r.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            r.color = textureColor;

        }
        
    }


}
