using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
//This is a module for generic methods.


public class Vckrs : MonoBehaviour
{

    //Tween test
    //public bool tweenTest;
    //public Vector3 aim;

    //void Update()
    //{
    //    if (tweenTest)
    //    {
    //        tweenTest = false;
    //        Timing.RunCoroutine(Vckrs._Tween(gameObject, aim, 3f));
    //    }
    //}

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

}
