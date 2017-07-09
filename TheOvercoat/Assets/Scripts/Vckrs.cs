using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine.UI;
using UnityEditor;
//This is a module for generic methods, classes, enums and some other things.


//Animation type enum. Use this enum for taking actions according to animation parameter type.
public enum AnimType { Trigger, Boolean, Int,Float };
public enum Axis { X,Y,Z};
public enum Plane { XY,XZ,YZ};
public enum Language { ENG=0, TR=1}


public class Vckrs : MonoBehaviour
{
    static float cameraConvertSizeToDistance = 6;


    public static IEnumerator<float> _setLightIntensity(GameObject light, float speed, float intensity)
    {
        Light l = light.GetComponent<Light>();
        float initialIntesity = l.intensity;
        float ratio = 0;

        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            l.intensity = Mathf.Lerp(initialIntesity, intensity, ratio);
            yield return 0;
        }



    }

    public static IEnumerator<float> waitUntilStop(GameObject obj, float tol = 0.000001f)
    {

        Vector3 pos = obj.transform.position;
        yield return Timing.WaitForSeconds(0.5f);
        while (Vector3.Distance(obj.transform.position, pos) > tol)
        {
            //Debug.Log((Vector3.Distance(obj.transform.position, pos)));
            pos = obj.transform.position;
            yield return 0;

        }
        //Debug.Log("break");
        yield break;
    }

    public static IEnumerator<float> _setDestination(GameObject obj, Vector3 pos)
    {
        UnityEngine.AI.NavMeshAgent agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (!agent) yield break;

        agent.SetDestination(pos);
        IEnumerator<float> handler = Timing.RunCoroutine(waitUntilStop(obj));
        yield return Timing.WaitUntilDone(handler);

        yield break;

    }


    public static IEnumerator<float> _Tween(GameObject go, GameObject aim, float speed)
    {

        Vector3 initialPosition = go.transform.position;

        float ratio = 0;



        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;


            go.transform.position = Vector3.Lerp(initialPosition, aim.transform.position, ratio);
            yield return 0;

        }
        go.transform.position = aim.transform.position;

        yield break;
    }




    public static IEnumerator<float> _TweenRootFunc(GameObject go, Vector3 aim, float factor, float lowestLimitSpeed)
    {


        Vector3 initialPosition = go.transform.position;

        float ratio = 0;

        float delta = Vector3.Distance(go.transform.position, aim);

        float speed = factor * Vector3.Distance(go.transform.position, aim) / delta;
        speed = Mathf.Clamp(speed, lowestLimitSpeed,speed);

        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;
            go.transform.position = Vector3.Lerp(initialPosition, aim, ratio);
            speed = factor * (Mathf.Abs(Vector3.Distance(go.transform.position, aim))) / delta;
            yield return 0;

        }
        go.transform.position = aim;

        yield break;
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
        go.transform.position = aim;

        yield break;
    }




    //Lerp position with heigh.
    //Height depends on sin function
    public static IEnumerator<float> _TweenSinHeight(GameObject go, Vector3 aim, float speed, float maxHeight = 1)
    {

        Vector3 initialPosition = go.transform.position;

        float ratio = 0;

        while (ratio < 1)
        {
            ratio += Time.deltaTime * speed;

            Vector3 newPosition = Vector3.Lerp(initialPosition, aim, ratio);
            //Debug.Log(Mathf.Sin(3.14f*ratio));
            newPosition += Vector3.up * Mathf.Sin(3.14f * ratio) * maxHeight;
            go.transform.position = newPosition;

            yield return 0;

        }
        go.transform.position = aim;


    }

    static public IEnumerator<float> _lookTo(GameObject go, Vector3 aim, float speed)
    {

        Vector3 localAim = go.transform.position + aim;


        localAim.y = go.transform.position.y;


        Quaternion initialRot = go.transform.rotation;
        Quaternion aimRot = Quaternion.LookRotation(localAim - go.transform.position);
        float ratio = 0;


        while (ratio < 1)
        {

            ratio += Time.deltaTime * speed;
            go.transform.rotation = Quaternion.Lerp(initialRot, aimRot, ratio);

            yield return 0;
        }
        go.transform.rotation = aimRot;

    }


    static public IEnumerator<float> _lookTo(GameObject go, GameObject aim, float speed)
    {

        Vector3 localAim = aim.transform.position;


        localAim.y = go.transform.position.y;


        Quaternion initialRot = go.transform.rotation;
        Quaternion aimRot = Quaternion.LookRotation(localAim - go.transform.position);
        float ratio = 0;


        while (ratio < 1)
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
        UnityEngine.AI.NavMeshAgent nav = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.enabled = true;
        nav.Resume();
        nav.SetDestination(aim1);
        IEnumerator<float> handler = Timing.RunCoroutine(Vckrs.waitUntilStop(obj, 0.0001f));

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
            } else
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

    public static IEnumerator<float> followObject(UnityEngine.AI.NavMeshAgent agent, GameObject obj)
    {
        agent.Resume();
        while (true)
        {
            agent.SetDestination(obj.transform.position);
            yield return 0;
        }
    }


    //public static void setCameraSize(Camera cam, float size)
    //{
    //    if (cam.orthographic)
    //    {
    //        cam.orthographicSize = size;
    //    }
    //    else
    //    {
    //        cam.transform.position = cam.transform.position - cam.transform.forward * size * cameraConvertSizeToDistance;
    //    }
    //}

    //public static IEnumerator<float> _cameraSize(Camera cam, float size, float speed)
    //{

    //    if (cam.orthographic)
    //    {

    //        if (cam.orthographicSize > size)
    //        {

    //            while (cam.orthographicSize > size)
    //            {
    //                cam.orthographicSize -= Time.deltaTime * speed;
    //                yield return 0;
    //            }
    //            cam.orthographicSize = size;

    //        }
    //        else
    //        {

    //            while (cam.orthographicSize < size)
    //            {
    //                //print("increase");
    //                cam.orthographicSize += Time.deltaTime * speed;
    //                yield return 0;
    //            }
    //            cam.orthographicSize = size;
    //        }
    //    }
    //    else
    //    {

    //        while (size > 0)
    //        {

    //            cam.transform.position += cam.transform.forward * Time.deltaTime * speed * cameraConvertSizeToDistance;
    //            size -= Time.deltaTime * speed * cameraConvertSizeToDistance;
    //            yield return 0;

    //        }
    //        //if (cam.fieldOfView > size)
    //        //{

    //        //    while (cam.fieldOfView > size)
    //        //    {
    //        //        cam.fieldOfView -= Time.deltaTime * speed;
    //        //        yield return 0;
    //        //    }
    //        //    cam.fieldOfView = size;

    //        //}
    //        //else
    //        //{

    //        //    while (cam.fieldOfView < size)
    //        //    {
    //        //        //print("increase");
    //        //        cam.fieldOfView += Time.deltaTime * speed;
    //        //        yield return 0;
    //        //    }
    //        //    cam.fieldOfView = size;
    //        //}

    //    }
    
    
    //}

    //public static IEnumerator<float> _cameraSizeRootFunc(Camera cam, float size, float factor, float limitSpeed=0.2f)
    //{
   
    //        float speed = factor;
    //        float delta = Mathf.Abs(cam.orthographicSize - size);


    //    if (cam.orthographic)
    //    {
    //        if (cam.orthographicSize > size)
    //        {

    //            while (cam.orthographicSize > size)
    //            {
    //                speed = factor * (Mathf.Abs(cam.orthographicSize - size)) / delta;
    //                speed = Mathf.Clamp(speed, limitSpeed, speed);
    //                cam.orthographicSize -= Time.deltaTime * speed;
    //                yield return 0;
    //            }
    //            cam.orthographicSize = size;

    //        }
    //        else
    //        {

    //            while (cam.orthographicSize < size)
    //            {
    //                //print("increase");
    //                speed = factor * (Mathf.Abs(cam.orthographicSize - size)) / delta;
    //                speed = Mathf.Clamp(speed, speed, limitSpeed);
    //                cam.orthographicSize += Time.deltaTime * speed;
    //                yield return 0;
    //            }
    //            cam.orthographicSize = size;
    //        }

    //    }
    //    else
    //    {


    //        while (size > 0)
    //        {

    //            cam.transform.position += cam.transform.forward * Time.deltaTime * speed * cameraConvertSizeToDistance;
    //            size -= Time.deltaTime * speed * cameraConvertSizeToDistance;
    //            yield return 0;

    //        }
    //        //if (cam.fieldOfView > size)
    //        //{

    //        //    while (cam.fieldOfView > size)
    //        //    {
    //        //        speed = factor * (Mathf.Abs(cam.orthographicSize - size)) / delta;
    //        //        speed = Mathf.Clamp(speed, limitSpeed, speed);
    //        //        cam.orthographicSize -= Time.deltaTime * speed;
    //        //        yield return 0;
    //        //    }
    //        //    cam.fieldOfView = size;

    //        //}
    //        //else
    //        //{

    //        //    while (cam.fieldOfView < size)
    //        //    {
    //        //        //print("increase");
    //        //        speed = factor * (Mathf.Abs(cam.orthographicSize - size)) / delta;
    //        //        speed = Mathf.Clamp(speed, speed, limitSpeed);
    //        //        cam.fieldOfView += Time.deltaTime * speed;
    //        //        yield return 0;
    //        //    }
    //        //    cam.fieldOfView = size;
    //        //}


    //    }
    //}

    


    public static IEnumerator<float> _fadeObject(GameObject obj, float speed, bool fullFade=false)
    {
       

        Renderer rend = obj.GetComponent<Renderer>();


        if (!rend) rend = obj.GetComponentInChildren<Renderer>();

        Color textureColor = rend.material.color;
        float a = textureColor.a;

        IEnumerator<float> handler;
        if (a == 1)
        {
            handler = Timing.RunCoroutine(_fadeObjectOut(obj, speed, fullFade));
        }else
        {
            handler = Timing.RunCoroutine(_fadeObjectIn(obj, speed, fullFade));
        }

        yield return Timing.WaitUntilDone(handler);

        yield break;

    }

    public static IEnumerator<float> _fadeObjectIn(GameObject obj, float speed, bool fullFade = false)
    {

        Renderer rend = obj.GetComponent<Renderer>();

        if (!rend) rend = obj.GetComponentInChildren<Renderer>();

        Color textureColor = rend.material.color;
        float a = textureColor.a;

        //If it is already visible in then break
        if (a == 1) yield break;

        //It is for changing rendered mode at right time
        bool willBeTransparent = false;
        StandardShaderUtils.BlendMode mode = (fullFade) ? StandardShaderUtils.BlendMode.Fade : StandardShaderUtils.BlendMode.Transparent;

        if (willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, mode);
        }


        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                rend.material.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            rend.material.color = textureColor;

        }

        if (!willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
        }

        yield break;
    }


    public static IEnumerator<float> _fadeObjectOut(GameObject obj, float speed, bool fullFade = false)
    {
        Renderer rend = obj.GetComponent<Renderer>();

        if (!rend) rend = obj.GetComponentInChildren<Renderer>();

        //Debug.Log(rend.material.name);

        Color textureColor = rend.material.color;
        float a = textureColor.a;

        //If object is already faded
        if (a == 0) yield break;

        //It is for changing rendered mode at right time
        bool willBeTransparent = true;
        StandardShaderUtils.BlendMode mode = (fullFade) ? StandardShaderUtils.BlendMode.Fade : StandardShaderUtils.BlendMode.Transparent;

        if (willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, mode);
        }


        if(a==1)
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                rend.material.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            rend.material.color = textureColor;

        }



        if (!willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
        }

        yield break;
    }

    public static IEnumerator<float> _fadeObjectOut(Renderer rend, float speed, bool fullFade = false)
    {

        Color textureColor = rend.material.color;
        float a = textureColor.a;

        //If object is already faded
        if (a == 0) yield break;

        //It is for changing rendered mode at right time
        bool willBeTransparent = true;
        StandardShaderUtils.BlendMode mode = (fullFade) ? StandardShaderUtils.BlendMode.Fade : StandardShaderUtils.BlendMode.Transparent;

        if (willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, mode);
        }


        if (a == 1)
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                rend.material.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            rend.material.color = textureColor;

        }



        if (!willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
        }

        yield break;
    }


    public static void makeObjectTransparent(GameObject obj, bool fullFade = false)
    {


        Renderer rend = obj.GetComponent<Renderer>();
        Color textureColor = rend.material.color;
        float a = textureColor.a;

        //It is for changing rendered mode at right time
        bool willBeTransparent = true;
        StandardShaderUtils.BlendMode mode = (fullFade) ? StandardShaderUtils.BlendMode.Fade : StandardShaderUtils.BlendMode.Transparent;

        if (willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, mode);
        }


        textureColor.a = 0;
        rend.material.color = textureColor;

        if (!willBeTransparent)
        {
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
        }


    }



    public static void makeObjectOpaque(GameObject obj)
    {


        Renderer rend = obj.GetComponent<Renderer>();
        Color textureColor = rend.material.color;
        float a = textureColor.a;

      
  
        textureColor.a = 1;
        rend.material.color = textureColor;

    
            StandardShaderUtils.ChangeRenderMode(rend.material, StandardShaderUtils.BlendMode.Opaque);
        


    }

    public static void setAlpha<T>(GameObject obj, float a) where T : MaskableGraphic
    {
       
          a = Mathf.Clamp(a, 0, 1);
          T t = obj.GetComponent<T>();
          Color textureColor = t.color;
          textureColor.a=a;
        t.color = textureColor;
        //Debug.Log("setting " + obj.name + " invisible" + textur);
    }


public static IEnumerator<float> _fadeInfadeOut<T>(GameObject obj, float speed) where T: MaskableGraphic
    {
        T t = obj.GetComponent<T>();

        Color textureColor = t.color;
        float a = textureColor.a;


        if (a == 0)
        {
            while (a < 1)
            {

                a += Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            t.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            t.color = textureColor;

        }
    }

    public static IEnumerator<float> _fadeInfadeOut(GameObject rawImage, float speed)
    {
        RawImage r = rawImage.GetComponent<RawImage>();

        Color textureColor = r.color;
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

        } else
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

    public static IEnumerator<float> _fadeInfadeOutImage(GameObject image, float speed)
    {
        Image i = image.GetComponent<Image>();

        Color textureColor = i.color;
        float a = textureColor.a;



        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                i.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            i.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                i.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            i.color = textureColor;

        }

    }

    public static IEnumerator<float> _fadeInfadeOutText(GameObject text, float speed)
    {
        Text t = text.GetComponent<Text>();

        Color textureColor = t.color;
        float a = textureColor.a;



        if (a == 0)
        {
            while (a < 1)
            {
                a += Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 1;
            t.color = textureColor;

        }
        else
        {
            while (a > 0)
            {
                a -= Time.deltaTime * speed;
                textureColor.a = a;
                t.color = textureColor;
                yield return 0;
            }
            textureColor.a = 0;
            t.color = textureColor;

        }

    }


    public static IEnumerator<float> _changeLight(float speed, Light light, float intensity)
    {

        float currentIntesity = light.intensity;



        if (currentIntesity<intensity)
        {
            while (currentIntesity < intensity)
            {
                currentIntesity += Time.deltaTime * speed;
                light.intensity = currentIntesity;

          
                yield return 0;
            }
                       

            currentIntesity = intensity;
            light.intensity = currentIntesity;
            

            yield break;
        }
        else
        {
            while (currentIntesity > intensity)
            {
                currentIntesity -= Time.deltaTime * speed;
                light.intensity = currentIntesity;

        

                yield return 0;
            }
            currentIntesity = intensity;
            light.intensity = currentIntesity;

          

            yield break;
        }
    }


    public static IEnumerator<float> InstantiateIn(GameObject obj, float delay, Vector3 pos)
    {
        yield return Timing.WaitForSeconds(delay);
        GameObject spawnedObj = Instantiate(obj) as GameObject;
        spawnedObj.transform.position = pos;
        yield break;

    }
    public static IEnumerator<float> InstantiateIn(GameObject obj, float delay, GameObject posObj,Vector3 offset)
    {
        yield return Timing.WaitForSeconds(delay);
        GameObject spawnedObj = Instantiate(obj) as GameObject;
        spawnedObj.transform.position = posObj.transform.position+offset;
        yield break;

    }

    //Returns nearest position to pos on navmesh.
    //If fails returns false
    public static bool findNearestPositionOnNavMesh(Vector3 pos, int areaMask, float radiusToSearch, out Vector3 foundPosition)
    {
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, radiusToSearch, areaMask))
        {
            foundPosition = hit.position;
            return true;
        }

        foundPosition = Vector3.zero;
        return false;

    }  

    public static GameObject findNearestObjectToPos(Vector3 pos, GameObject[] objects)
    {
        GameObject nearesrObj=null;
        float minDistance = Mathf.Infinity;
        foreach(GameObject obj in objects)
        {
            float dis = Vector3.Distance(obj.transform.position, pos);
            if (minDistance < dis)
            {
                nearesrObj = obj;
                minDistance = dis;
            }
        }

        return nearesrObj;

    }

    public static GameObject findNearestObjectToPos(Vector3 pos, List<GameObject> objects)
    {
        GameObject nearesrObj = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject obj in objects)
        {
            float dis = Vector3.Distance(obj.transform.position, pos);
            if (minDistance > dis)
            {
                nearesrObj = obj;
                minDistance = dis;
            }
        }

        if (nearesrObj == null) Debug.Log("Couldn't fin nearest object");

        return nearesrObj;

    }

    void testGenerateRandomPosition()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = generateRandomPositionOnCircle(transform.position, 10f);
        }
    }

    //Return random position on a circle which is around center and have radius of indicated in paramaters
    public static Vector3 generateRandomPositionOnCircle(Vector3 center, float radius)
    {
        int angle = Random.Range(0, 360); // generates numbers in the range 0 ... 359

        float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        float z = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

        return new Vector3(x, center.y, z);

    }


    //Return random position on a circle which is around center and have radius of indicated in paramaters
    public static Vector3 generateRandomPositionBetweenCircles(Vector3 center, float radius1, float radius2)
    {
        int angle = Random.Range(0, 360); // generates numbers in the range 0 ... 359
        float radius = Random.Range(radius1, radius2); //generates random radius


        float x = center.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
        float z = center.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

        return new Vector3(x, center.y, z);

    }

    public static Vector3 generateRandomPositionInBox(GameObject boxObject)
    {
        Vector3 randomPositionInBox;
        randomPositionInBox = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomPositionInBox = boxObject.transform.TransformPoint(randomPositionInBox * .5f);
        return randomPositionInBox;
    }


    //Returns a list holding all children objects.
    public static List<Transform> getAllChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {

            children.Add(child);

            if (child.childCount > 0)
            {
                children.AddRange(getAllChildren(child));
            }

        }

        return children;

    }

    public static bool canCameraSeeObject(GameObject obj, Camera cam)
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(obj.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    public static bool canCameraSeeObject(Vector3 pos, Camera cam)
    {
        
        Vector3 screenPoint = cam.WorldToViewportPoint(pos);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    public static void disableAllChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++) parent.GetChild(i).gameObject.SetActive(false);

    }

    public static void disableAllChildren(Transform parent, List<Transform> exceptions)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if(!exceptions.Contains(child))    parent.GetChild(i).gameObject.SetActive(false);
        }

    }
    
    public static void enableAllChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++) parent.GetChild(i).gameObject.SetActive(true);

    }

    public static IEnumerator<float> showMessageForSeconds(string message, Text textComp, float seconds)
    {
        textComp.text = message;
        yield return Timing.WaitForSeconds(seconds);
        textComp.text = "";
        yield break;
    }

    public delegate void myDelegate();

    public static void doItAfterFrame(myDelegate theDelegate, int frameCount)
    {
        Timing.RunCoroutine(_doItAfterFrame(theDelegate,frameCount));
    }

    static IEnumerator<float> _doItAfterFrame(myDelegate theDelegate, int frameCount)
    {
        int frame = 0;
        while (frame < frameCount)
        {
            frame++;
            yield return 0;
        }
        theDelegate();
        yield break;

    }

    //Generates random position on floor which seen by camera
    public static bool generateRandomVisiblePosition(Camera cam, string floorTag, out Vector3 result)
    {
        Vector2 randomPos = randomPosOnScreen(10);
        //Debug.Log(randomPos);
        Ray ray=cam.ScreenPointToRay(randomPos);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == floorTag)
            {
                result = hit.point;
                return true;
            }
            else
            {
                result = Vector3.zero;
                return false;
            }
        }
        else
        {
            Debug.Log("Couldbt ray cast");
            result = Vector3.zero;
            return false;
        }


    }


    //Sets object position outside of camera with trying to find suitable place on navmesh. 
    //It generates tries to find position between tow circle areas like a torus
    //If it fails does nothing. It does in one frame so choose try number suitable.
    public static bool setPositionToOutsideOfCameraAndOnNavmesh(GameObject obj, Vector3 center, int numberOfTrial,Camera cam, float smallRadius=30f, float bigRadius=50f)
    {


        //If object doesnt have navmesh then return null 
        UnityEngine.AI.NavMeshAgent nma = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (nma == null) return false;
        if (smallRadius > bigRadius) return false;
        
        

        
        bool onNavmesh = false;
        bool outsideOfCam = false;

        int trial = numberOfTrial;

        Vector3 position=Vector3.zero;

        while ((!onNavmesh || !outsideOfCam) && trial > 0)
        {
            Vector3 positionLimit1 = Vckrs.generateRandomPositionOnCircle(center, smallRadius);
            Vector3 positionLimit2 = Vckrs.generateRandomPositionOnCircle(center, bigRadius);
            position = Vector3.Lerp(positionLimit1, positionLimit2, Random.Range(0, 1));

            //Cast obj position to navmesh and check it is on navmesh
            UnityEngine.AI.NavMeshHit nmh;
            if ((UnityEngine.AI.NavMesh.SamplePosition(position, out nmh, 2f, nma.areaMask)))
            {
                position = nmh.position;
                onNavmesh = true;
            }
            else
            {
                onNavmesh = false;
            }

            //Check obj is outisde of camera
            outsideOfCam = !canCameraSeeObject(position, CharGameController.getCamera().GetComponent<Camera>());


            trial--;
        }

        if (trial <= 0)
        {
            //Debug.Log("Couldn't find appropiate position for girl");
            return false;
        }
        else
        {
            //Debug.Log("Found appropiate position for girl");
            //Debug.Log("Number of trial is " + (numberOfTrial - trial));
            obj.transform.position = position;
            return true;
        }



    }

    //Makes Y axis of vector 0 with fast way. Yeah I know I am too lazy.
    public static Vector3 eliminiteY(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    //Adds impact effect using character controller
    public static IEnumerator<float> addImpactForceCC(GameObject subjectOfForce, Vector3 force, float mass=1)
    {
        
        //Debug.Log(force);
        //Time.timeScale = 0;
        Vector3 impact = force / mass;
        CharacterController cc = subjectOfForce.GetComponent<CharacterController>();

        while (impact.magnitude > 0)
        {
            cc.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

            yield return 0;
        }

        yield break;

    }


    public static float randomSign()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            return -1;
        }else
        {
            return 1;
        }
    }
    
    public static Vector2 centerOfScreen()
    {
        return new Vector2(Screen.width/2,Screen.height/2);
    }

    public static Vector2 screenRatioToPosition(float width,float height)
    {
        width=Mathf.Clamp(width, 0, 1);
        height = Mathf.Clamp(height, 0, 1);

        return new Vector2(width * Screen.width, height * Screen.height);
    }

    public static Vector2 randomPosOnScreen(float offset=0)
    {
        return new Vector2(Random.Range(0 + offset, Screen.width - offset), Random.Range(0 + offset, Screen.height - offset));
    }

    //public static void printList(List<Object> list)
    //{
    //    foreach (Object obj in list)
    //    {
    //        print(obj);
    //    }
    //}



    public static Vector3 getRandomPosInCircle(Vector3 center, float radius, Plane p)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;

        if (p == Plane.XY)
        {
            return (new Vector3(randomPos.x, randomPos.y,0)) + center;

        }else if (p == Plane.XZ)
        {
            return (new Vector3(randomPos.x, 0, randomPos.y)) + center;
        }
        else
        {
            return (new Vector3( 0, randomPos.x, randomPos.y)) + center;
        }

    

    }
    
    /// <summary>
    /// extract string according to input language enum. text asset must be in the fallowing format
    /// TR
    /// string
    /// -----
    /// ENG
    /// string
    /// -----
    /// </summary>
    /// <param name="language"></param>
    /// <param name="ta"></param>
    public static string getStringAccordingToLanguage(Language language, TextAsset ta, string finishSignature = "-----")
    {
        
        string languageCode = language.ToString();

        //Debug.Log(languageCode);

        int startIndex = findLine(languageCode,ta) + 1;
        int finishIndex = findLine(startIndex, finishSignature,ta) - 1;


        //If text asset doesn't have text for languge settings that user set, then import turkish subtitles by default. Because 
        // I am sure there will be turkish subtitle. Because I am turk :)
        if (startIndex > finishIndex || startIndex == 0)
        {
            //Debug.Log("Couldn't find subtite for " + languageCode + " in "+ fileName+" so importing turkish");
            startIndex = findLine("TR",ta) + 1;
            finishIndex = findLine(startIndex, finishSignature,ta) - 1;

            //If stille couldn't find any subtitle from text asset then return and don't do aything
            if (startIndex > finishIndex) return "";
        }


        //Debug.Log("start index is " + startIndex + " finish index is" + finishIndex);

        return readLinesFromTA(startIndex, finishIndex, ta);
    }


    //Returns line of input string if it is exist, starting to search after startIndex.
    public static int findLine(int startIndex, string line, TextAsset textAsset)
    {

        //StreamReader theReader = new StreamReader(Application.dataPath + "/Resources/" + folderName + "/" + fileName + ".txt");

        string wholeFile =/* theReader.ReadToEnd();*/ textAsset.text;
        string[] lines = wholeFile.Split('\n');
        string[] sublines = new string[lines.Length - startIndex];

        for (int i = 0; i < sublines.Length; i++)
        {

            sublines[i] = lines[startIndex + i];
            //Debug.Log(sublines[i].Length+ " line is "+line.Length);
            //Debug.Log(string.Compare(line, sublines[i].TrimEnd()));

            if (string.Compare(line, sublines[i].TrimEnd()) == 0)
            {
                //Debug.Log("Returned " + (startIndex+ i));
                return startIndex + i;

            }
        }

        return -1;
    }

    //Returns line of input string if it is exist
    public static int findLine(string line, TextAsset textAsset)
    {

        //StreamReader theReader = new StreamReader(Application.dataPath + "/Resources/" + folderName + "/" + fileName+".txt");

        string wholeFile =/* theReader.ReadToEnd();*/ textAsset.text;
        string[] lines = wholeFile.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            if (string.Compare(line, lines[i].TrimEnd()) == 0)
            {
                return i;
            }
        }

        //Debug.Log("Coduln2t fine line " + line + " in text asset");
        return -1;
    }

    //Read lines from text asset with starting fromt startIndex and finishing at finishIndex lines.
    public static string readLinesFromTA(int startIndex, int finishIndex, TextAsset ta)
    {
        string[] allLines = textFileToStringArray(ta);
        string[] rangeLines = new string[finishIndex - startIndex + 1];

        for (int i = 0; i < rangeLines.Length; i++)
        {
            rangeLines[i] = allLines[startIndex + i];
            //Debug.Log(rangeLines[i]);
        }

        string result="";
        foreach(string s in rangeLines)
        {
            result += s;
        }

        return result;

    }
    public static string[] textFileToStringArray(TextAsset ta)
    {
        string allText = ta.text;
        string[] lines = allText.Split('\n');

        //foreach (string line in lines) Debug.Log(line);

        return lines;
    }

}


//Class for holding basic components of characters
//It is for shortcut
public class characterComponents
{
    public GameObject player;
    public Animator animator;
    public UnityEngine.AI.NavMeshAgent navmashagent;


    public characterComponents(GameObject p)
    {
        player = p;
        animator = p.GetComponent<Animator>();
        navmashagent = p.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


}
public static class StandardShaderUtils
{
    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }
}

public class Timer
{
    float timer = 0;
    float time = 0;

    public Timer(float time)
    {
        this.time = time;
        timer = time;
    }

    public bool ticTac(float delta)
    {
        timer -= delta;

        if (timer <= 0)
        {
            timer = time;
            return true;
        }

        return false;

    }

    
    public void resetTimet()
    {
        timer = time;
    }


}
 
// public class ReadInputManager
//{
//    public enum Axes { }


//    public static void ReadAxes()
//    {
        
        
//        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];

//        SerializedObject obj = new SerializedObject(inputManager);

//        SerializedProperty axisArray = obj.FindProperty("m_Axes");

//        if (axisArray.arraySize == 0)
//            Debug.Log("No Axes");

//        for (int i = 0; i < axisArray.arraySize; ++i)
//        {
//            var axis = axisArray.GetArrayElementAtIndex(i);

//            var name = axis.FindPropertyRelative("m_Name").stringValue;
//            //var axisVal = axis.FindPropertyRelative("axis").intValue;
//            //var inputType = /*(InputType)*/axis.FindPropertyRelative("type").intValue;

//            Debug.Log(name);
//            Debug.Log(axisVal);
//            Debug.Log(inputType);
//        }
//    }

//    //public enum InputType
//    //{
//    //    KeyOrMouseButton,
//    //    MouseMovement,
//    //    JoystickAxis,
//    //};

//    //[MenuItem("Assets/ReadInputManager")]
//    //public static void DoRead()
//    //{
//    //    ReadAxes();
//    //}

//}
